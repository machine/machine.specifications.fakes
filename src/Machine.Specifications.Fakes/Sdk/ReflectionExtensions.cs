using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Machine.Specifications.Fakes.Sdk
{
    /// <summary>
    ///     Helper class which contains all the helper method needed for reflection.
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        ///     Checks whether the supplied type is one of the machine.fakes
        ///     inline constraint types.
        /// </summary>
        /// <param name = "type">
        ///     Specifies the type to check.
        /// </param>
        /// <returns>
        ///     <c>true</c> if it's one of the constraint types. Otherwise not.
        /// </returns>
        public static bool IsMFakesConstraint(this Type type)
        {
            return type == typeof(Param) || type.ClosesGenericParamType();
        }

        /// <summary>
        ///     Checks whether the supplied type closes the <see cref = "Param{T}" /> class.
        /// </summary>
        /// <param name = "type">
        ///     Specifies the type to be checked.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the type closes the <see cref = "Param{T}" /> type. Otherwise <c>false</c>.
        /// </returns>
        public static bool ClosesGenericParamType(this Type type)
        {
            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Param<>);
        }

        /// <summary>
        ///     Gets the first generic type argument of the specified type.
        /// </summary>
        /// <param name = "type">
        ///     Specifies the type to extract the type argument from.
        /// </param>
        /// <returns>
        ///     The extracted type.
        /// </returns>
        public static Type GetFirstTypeArgument(this Type type)
        {
            if (!type.GetTypeInfo().IsGenericType)
            {
                throw new ArgumentException("Specified type is not a generic type", "type");
            }

            return type.GetGenericArguments()[0];
        }

        /// <summary>
        ///     Gets the first generic type argument of the specified type.
        /// </summary>
        /// <param name = "method">
        ///     Specifies the method to extract the type argument from.
        /// </param>
        /// <returns>
        ///     The extracted type.
        /// </returns>
        public static Type GetFirstTypeArgument(this MethodInfo method)
        {
            if (!method.IsGenericMethod)
            {
                throw new ArgumentException("Specified method is not a generic method", "method");
            }

            return method.GetGenericArguments()[0];
        }

        /// <summary>
        ///     Creates a <see cref = "MemberExpression" /> on a public instance property.
        /// </summary>
        /// <param name = "targetType">
        ///     Specifies the target type.
        /// </param>
        /// <param name = "property">
        ///     Specifies the name of the property to be accessed.
        /// </param>
        /// <param name = "instanceExpression">
        ///     Specifies an instance via an <see cref = "Expression" />.
        /// </param>
        /// <returns>
        ///     The created <see cref = "MemberExpression" />.
        /// </returns>
        public static MemberExpression MakePropertyAccess(this Type targetType, string property, Expression instanceExpression)
        {
            Guard.AgainstArgumentNull(targetType, "targetType");
            Guard.AgainstArgumentNull(property, "property");
            Guard.AgainstArgumentNull(instanceExpression, "instanceExpression");

            return MakePropertyAccess(
                targetType,
                property,
                BindingFlags.Public | BindingFlags.Instance,
                instanceExpression);
        }

        /// <summary>
        ///     Creates a <see cref = "MemberExpression" /> on a public static property.
        /// </summary>
        /// <param name = "targetType">
        ///     Specifies the target type.
        /// </param>
        /// <param name = "property">
        ///     Specifies the name of the property to be accessed.
        /// </param>
        /// <returns>
        ///     The created <see cref = "MemberExpression" />.
        /// </returns>
        public static MemberExpression MakeStaticPropertyAccess(this Type targetType, string property)
        {
            Guard.AgainstArgumentNull(targetType, "targetType");
            Guard.AgainstArgumentNull(property, "property");

            return MakePropertyAccess(
                targetType,
                property,
                BindingFlags.Public | BindingFlags.Static,
                null);
        }

        /// <summary>
        ///     Get all field values of the type specified by <typeparamref name = "TFieldType" />.
        /// </summary>
        /// <typeparam name = "TFieldType">
        ///     Specifies the field type.
        /// </typeparam>
        /// <param name = "instance">
        ///     Specifies the instance to extract the field values from.
        /// </param>
        /// <returns>
        ///     A collection of all field values of the specified type.
        /// </returns>
        public static IEnumerable<TFieldType> GetFieldValues<TFieldType>(this object instance)
        {
            Guard.AgainstArgumentNull(instance, "instance");

            var fieldValues = instance
                .GetType()
                .GetAllFields()
                .Where(field => field.FieldType == typeof(TFieldType))
                .Select(field => (TFieldType)field.GetValue(instance))
                .Where(value => !Equals(value, null));

            return fieldValues;
        }

        /// <summary>
        /// Resets the references in the instance specified by <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public static void ResetReferences(this object instance)
        {
            var fields = instance
                .GetType()
                .GetAllFields()
                .Where(x => !x.FieldType.GetTypeInfo().IsValueType);

            fields.Each(x => x.SetValue(instance, null));
        }

        static IEnumerable<FieldInfo> GetAllFields(this Type type)
        {
            return type
                .GetFields(
                    BindingFlags.Static |
                    BindingFlags.Instance |
                    BindingFlags.NonPublic |
                    BindingFlags.Public |
                    BindingFlags.FlattenHierarchy)
                .Where(t => !t.Name.StartsWith("CS$"));
        }

        static MemberExpression MakePropertyAccess(this Type targetType, string property, BindingFlags flags, Expression instanceExpression)
        {
            Guard.AgainstArgumentNull(targetType, "targetType");
            Guard.AgainstArgumentNull(property, "property");

            var targetProperty = targetType.GetProperty(property, flags);

            if (targetProperty == null)
            {
                throw new InvalidOperationException(
                    string.Format("Unable to find target property {0} on instance of target type {1}", property, targetType.FullName));
            }

            return Expression.MakeMemberAccess(instanceExpression, targetProperty);
        }
    }
}
