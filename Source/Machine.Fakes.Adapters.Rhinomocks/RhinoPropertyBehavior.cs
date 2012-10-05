using System;
using System.Linq;
using System.Reflection;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;


// Based on implementation used in Rhino Mocks original Stubs creation

namespace Machine.Fakes.Adapters.Rhinomocks
{
    internal static class RhinoPropertyBehavior
    {
        static bool CanWriteToPropertyThroughPublicSignature(PropertyInfo property)
        {
            if (property.CanWrite)
                return property.GetSetMethod(false) != null;
            return false;
        }

        static void CreateDefaultValueForValueTypeProperty(IMockedObject mockedObject, PropertyInfo property)
        {
            mockedObject.HandleProperty(property.GetSetMethod(true), new[]
                {
                    Activator.CreateInstance(property.PropertyType)
                });
        }

        static void SetPropertyBehavior(IMockedObject mockedObject, Type[] implementedTypes)
        {
            foreach (Type type in implementedTypes)
            {
                if (type.BaseType != null && type.BaseType != typeof (object))
                    SetPropertyBehavior(mockedObject, new[] {type.BaseType});

                SetPropertyBehavior(mockedObject, type.GetInterfaces());
                foreach (PropertyInfo propertyInfo in type.GetProperties())
                {
                    if (propertyInfo.CanRead && CanWriteToPropertyThroughPublicSignature(propertyInfo))
                    {
                        if (!mockedObject.RegisterPropertyBehaviorFor(propertyInfo) &&
                            propertyInfo.PropertyType.IsValueType)
                        {
                            CreateDefaultValueForValueTypeProperty(mockedObject, propertyInfo);
                        }
                    }
                }
            }
        }

        public static void RegisterPropertyBehavior(IMockedObject mockedObject)
        {
            SetPropertyBehavior(mockedObject, mockedObject.ImplementedTypes);
        }

        public static void RemovePropertiesBehavior(object fake)
        {
            var mockedObject = fake as IMockedObject;
            if (mockedObject != null)
                mockedObject.ClearState(BackToRecordOptions.PropertyBehavior);
        }
    }
}