using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TypescriptGenerator.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsStruct(this Type type)
        {
            return type.IsValueType && !type.IsEnum;
        }

        public static bool IsNullable(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        public static bool IsCollection(this Type propertyType, out Type itemType)
        {
            if (propertyType.IsArray)
            {
                itemType = propertyType.GetElementType();
                return true;
            }

            var ienumerableInterface = propertyType.GetInterfaces().Concat(new [] {propertyType})
                .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            if (ienumerableInterface != null)
            {
                itemType = ienumerableInterface.GetGenericArguments()[0];
                return true;
            }

            if (typeof(IEnumerable).IsAssignableFrom(propertyType))
            {
                itemType = typeof(object);
                return true;
            }

            itemType = null;
            return false;
        }

        public static bool IsDictionary(
            this Type propertyType,
            out Type keyType,
            out Type valueType)
        {
            if (!typeof(IDictionary).IsAssignableFrom(propertyType))
            {
                keyType = null;
                valueType = null;
                return false;
            }

            if(propertyType.GetGenericArguments().Length == 2)
            {
                keyType = propertyType.GetGenericArguments()[0];
                valueType = propertyType.GetGenericArguments()[1];
            }
            else
            {
                keyType = typeof(string);
                valueType = typeof(object);
            }

            return true;
        }
    }
}
