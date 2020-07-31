﻿using System;
using System.Collections.Generic;
using System.Linq;
using TypescriptGenerator.Extensions;
using TypescriptGenerator.Settings;

namespace TypescriptGenerator.Converters
{
    public class TypeDeterminer
    {
        private readonly TypescriptPropertyConverterSettings settings;
        private readonly List<NamespaceSettings> namespaceSettings;

        public TypeDeterminer(
            TypescriptPropertyConverterSettings settings, 
            List<NamespaceSettings> namespaceSettings)
        {
            this.settings = settings;
            this.namespaceSettings = namespaceSettings;
        }

        private static readonly Dictionary<Type, string> PrimitiveTypes = new Dictionary<Type, string>
        {
            { typeof(string), "string"},
            { typeof(void), "void"},
            { typeof(bool), "boolean"},
            { typeof(short), "number"},
            { typeof(ushort), "number"},
            { typeof(int), "number"},
            { typeof(uint), "number"},
            { typeof(long), "number"},
            { typeof(ulong), "number"},
            { typeof(double), "number"},
            { typeof(decimal), "number"},
            { typeof(Guid), "string"},
            { typeof(object), "{}"},
            { typeof(DateTime), "Date"},
            { typeof(DateTimeOffset), "Date"},
            { typeof(TimeSpan), "string"}
        };

        public static bool IsPrimitiveType(Type type)
        {
            return PrimitiveTypes.ContainsKey(type);
        }

        public string Format(Type propertyType)
        {
            if (IsPrimitiveType(propertyType))
                return PrimitiveTypes[propertyType];
            if (propertyType.IsCollection(out var itemType))
            {
                var itemTypescriptType = Format(itemType);
                return $"{itemTypescriptType}[]";
            }
            var matchingTypeConverter = settings.TypeConverters.FirstOrDefault(x => x.IsMatchingType(propertyType));
            if (matchingTypeConverter != null)
                return matchingTypeConverter.Convert(propertyType);
            if (propertyType.Namespace == null)
                return propertyType.Name;
            var matchingNamespaceSettings = namespaceSettings
                .Where(x => propertyType.Namespace.Contains(x.Namespace))
                .OrderByDescending(x => x.Namespace.Length)
                .FirstOrDefault();
            if (matchingNamespaceSettings != null)
            {
                return propertyType.FullName.Replace(
                    matchingNamespaceSettings.Namespace,
                    matchingNamespaceSettings.Translation);
            }
            return propertyType.FullName;
        }
    }
}