using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Machine.Fakes.Sdk;

namespace Machine.Fakes.Internal
{
    class AutoFakeContainer
    {
        readonly MappingRegistry _mappings = new MappingRegistry();
        readonly IFakeEngine _fakeEngine;

        public AutoFakeContainer(IFakeEngine fakeEngine)
        {
            Guard.AgainstArgumentNull(fakeEngine, "fakeEngine");

            _fakeEngine = fakeEngine;
        }

        internal object CreateFake(Type interfaceType, params object[] args)
        {
            return _fakeEngine.CreateFake(interfaceType, args);
        }

        internal TSubject CreateSubject<TSubject>()
        {
            return (TSubject)CreateInstance(typeof(TSubject), new Stack<Type>());
        }

        object CreateInstance(Type type, Stack<Type> buildStack)
        {
            if (buildStack.Contains(type))
                throw new InstanceCreationException(type, "It has a circular dependency to itself");

            buildStack.Push(type);

            var bestFitConstructor = GetBestFitConstructor(type);
            var parameters = bestFitConstructor
                .GetParameters().Select(x => GetArgument(x.ParameterType, buildStack)).ToArray();

            buildStack.Pop();
            try
            {
                return bestFitConstructor.Invoke(parameters);
            }
            catch (TargetInvocationException targetInvocationException)
            {
                throw new InstanceCreationException(
                    type, "The constructor threw an exception", targetInvocationException.InnerException);
            }
        }

        ConstructorInfo GetBestFitConstructor(Type type)
        {
            AssertPublicConstructors(type);

            var constructors = GetUsableConstructors(type);

            return constructors
                .Where(_ => _.GetParameters().Count() == constructors.Max(x => x.GetParameters().Count()))
                .OrderBy(CountRegisteredArguments)
                .Last();
        }

        void AssertPublicConstructors(Type type)
        {
            if (!type.GetConstructors().Any())
                throw new InstanceCreationException(type, "Please check that the type has at least a single public constructor");
        }

        IEnumerable<ConstructorInfo> GetUsableConstructors(Type type)
        {
            var constructors = type.GetConstructors().Where(x => x.GetParameters().All(_ => !_.ParameterType.IsPointer));

            if (!constructors.Any())
                throw new InstanceCreationException(type, "Constructors with pointer parameters are not supported");

            return constructors;
        }

        int CountRegisteredArguments(ConstructorInfo constructor)
        {
            return constructor.GetParameters().Count(x => CanBeInstantiated(x.ParameterType));
        }

        bool CanBeInstantiated(Type type)
        {
            if (_mappings.IsRegistered(type))
                return true;

            if (type.IsGenericType)
            {
                if (type.IsGenericEnumerable())
                    return _mappings.IsRegistered(type.GetGenericArguments()[0]);

                if (type.IsFunc() || type.IsLazy())
                    return true;
            }

            return type.IsArray && _mappings.IsRegistered(type.GetElementType());
        }

        object GetArgument(Type argumentType, Stack<Type> stack)
        {
            if (_mappings.IsRegistered(argumentType))
                return GetRegisteredInstances(argumentType).Last();

            if (argumentType.IsGenericType)
            {
                if (argumentType.IsGenericEnumerable())
                    return CreateEnumerable(argumentType);

                if (argumentType.IsFunc())
                    return CreateFunc(argumentType, stack);

                if (argumentType.IsLazy())
                    return CreateLazy(argumentType, stack);
            }

            if (argumentType.IsArray)
                return CreateArray(argumentType);

            return GetSimpleArgument(argumentType, stack);
        }

        object CreateLazy(Type type, Stack<Type> stack)
        {
            // ReSharper disable PossibleNullReferenceException : I know this constructor exists
            return typeof(Lazy<>)
                .MakeGenericType(type.GetGenericArguments()[0])
                .GetConstructor(new[] { typeof(Func<>).MakeGenericType(type.GetGenericArguments()[0]) })
                .Invoke(new[] { CreateFunc(type, stack) });
            // ReSharper restore PossibleNullReferenceException
        }

        object CreateFunc(Type type, Stack<Type> stack)
        {
            return Expression.Lambda(
                Expression.Constant(
                    GetArgument(type.GetGenericArguments().Last(), stack)))
                .Compile();
        }

        object CreateArray(Type type)
        {
            Type elementType = type.GetElementType();
            IEnumerable<object> instances = GetRegisteredInstances(elementType);
            Array array = Array.CreateInstance(elementType, instances.Count());
            instances.Aggregate(0, (i, o) => { array.SetValue(o, i); return i + 1; });
            return array;
        }

        object CreateEnumerable(Type argumentType)
        {
            Type underlyingType = argumentType.GetGenericArguments()[0];
            if (_mappings.IsRegistered(underlyingType))
            {
                return typeof(Enumerable)
                    .GetMethod("Cast", new[] { typeof(IEnumerable) })
                    .MakeGenericMethod(underlyingType)
                    .Invoke(null, new object[] { GetRegisteredInstances(underlyingType) });
            }

            return typeof(Enumerable)
                .GetMethod("Empty")
                .MakeGenericMethod(underlyingType)
                .Invoke(null, new object[] { });
        }

        object GetSimpleArgument(Type argumentType, Stack<Type> stack)
        {
            if (argumentType.IsInterface)
            {
                var fake = CreateFake(argumentType);
                Register(new ObjectMapping(argumentType, fake));
                return fake;
            }

            if (argumentType.IsValueType)
                return Activator.CreateInstance(argumentType);

            return CreateInstance(argumentType, stack);
        }

        internal TFakeSingleton Get<TFakeSingleton>() where TFakeSingleton : class
        {
            if (_mappings.IsRegistered(typeof(TFakeSingleton)))
                return (TFakeSingleton)GetRegisteredInstances(typeof(TFakeSingleton)).Last();

            var fake = CreateFake(typeof(TFakeSingleton));
            Register(new ObjectMapping(typeof(TFakeSingleton), fake));
            return (TFakeSingleton)fake;
        }

        IEnumerable<object> GetRegisteredInstances(Type type)
        {
            return _mappings.GetMappingsFor(type).Select(m => m.Resolve(t => CreateInstance(t, new Stack<Type>())));
        }

        public void Register(IMapping mapping)
        {
            _mappings.Register(mapping);
        }
    }
}