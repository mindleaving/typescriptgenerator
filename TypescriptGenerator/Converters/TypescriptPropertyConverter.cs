using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using TypescriptGenerator.Extensions;
using TypescriptGenerator.Objects;
using TypescriptGenerator.Settings;

namespace TypescriptGenerator.Converters
{
    public class TypescriptPropertyConverter
    {
        private readonly TypescriptPropertyConverterSettings settings;
        private readonly TypeDeterminer typeDeterminer;

        public TypescriptPropertyConverter(
            TypescriptPropertyConverterSettings settings,
            TypescriptEnumConverterSettings enumSettings,
            List<NamespaceSettings> namespaceSettings)
        {
            this.settings = settings;
            typeDeterminer = new TypeDeterminer(settings, enumSettings, namespaceSettings);
        }

        public TypescriptProperty Convert(PropertyInfo property)
        {
            var propertyName = property.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName
                               ?? ApplyCasing(property.Name, settings.Casing);
            return new TypescriptProperty(
                propertyName,
                typeDeterminer.Format(property.PropertyType),
                property.PropertyType.IsNullable());
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
                {
                    var parts = PropertyNameSplitter.Split(propertyName);
                    return string.Join("", parts.Select((part, idx) => idx == 0 ? part : FirstLetterUppercase(part)));
                }
                case CasingType.PascalCase:
                {
                    var parts = PropertyNameSplitter.Split(propertyName);
                    return string.Join("", parts.Select(FirstLetterUppercase));
                }
                case CasingType.Custom:
                    return settings.CustomPropertyNamingFunc(propertyName);
                default:
                    throw new ArgumentOutOfRangeException(nameof(casing), casing, null);
            }
        }

        private static string FirstLetterUppercase(string part)
        {
            return char.ToUpperInvariant(part.First()) + part.Substring(1);
        }
    }
}
