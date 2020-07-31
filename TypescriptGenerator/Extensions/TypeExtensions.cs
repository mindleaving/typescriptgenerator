using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TypescriptGenerator.Extensions
{
    public static class TypeExtensions
    {
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

            var ienumerableInterface = propertyType.GetInterfaces().Concat(new []{ propertyType })
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
    }
}
