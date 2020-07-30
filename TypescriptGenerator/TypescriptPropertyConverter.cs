using System;
using System.Reflection;

namespace TypescriptGenerator
{
    public class TypescriptPropertyConverter
    {
        private readonly TypescriptPropertyConverterSettings settings;


        public TypescriptProperty Convert(PropertyInfo property)
        {
            return new TypescriptProperty(
                ApplyCasing(property.Name, settings.Casing),
                GetTypescriptType(property.PropertyType),
                property.PropertyType.IsNullable());
        }

        private string GetTypescriptType(Type propertyType)
        {
            throw new NotImplementedException();
        }

        private string ApplyCasing(
            string propertyName,
            CasingType casing)
        {
            switch (casing)
            {
                case CasingType.Original:
                    return propertyName;
                case CasingType.CamelCase:
                    break;
                case CasingType.PascalCase:
                    break;
                case CasingType.Custom:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(casing), casing, null);
            }
        }
    }
}
