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
    public class TypescriptClassToInterfaceConverter
    {
        private readonly List<NamespaceSettings> namespaceSettings;
        private readonly TypescriptClassToInterfaceConverterSettings settings;
        private readonly TypescriptPropertyConverter propertyConverter;

        public TypescriptClassToInterfaceConverter(
            TypescriptClassToInterfaceConverterSettings settings,
            TypescriptEnumConverterSettings enumSettings,
            List<NamespaceSettings> namespaceSettings)
        {
            if (enumSettings == null) throw new ArgumentNullException(nameof(enumSettings));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.namespaceSettings = namespaceSettings ?? throw new ArgumentNullException(nameof(namespaceSettings));
            propertyConverter = new TypescriptPropertyConverter(
                this.settings.PropertySettings, 
                enumSettings,
                this.namespaceSettings);
        }

        public TypescriptInterface Convert(Type type)
        {
            if(!type.IsClass && !type.IsInterface && !type.IsStruct())
                throw new ArgumentException($"Type '{type.FullName}' is neither a class, struct nor an interface");

            var effectiveType = type;
            if (type.IsGenericType)
                effectiveType = type.GetGenericTypeDefinition();
            var typescriptProperties = effectiveType.GetProperties()
                .Where(ShouldIncludeProperty)
                .Select(propertyConverter.Convert)
                .ToList();

            var dependencies = typescriptProperties
                .SelectMany(x => x.Dependencies)
                .ToList();
            var translatedNamespace = NamespaceTranslator.Translate(effectiveType.Namespace, namespaceSettings);
            var typeName = type.IsGenericType ? GetGenericTypeName(type) : type.Name;
            return new TypescriptInterface(
                effectiveType.Namespace,
                translatedNamespace,
                typeName, // TODO: Apply transforms
                typescriptProperties,
                dependencies,
                settings.Modifiers);
        }

        private string GetGenericTypeName(Type type)
        {
            var genericName = type.Name.Substring(0, type.Name.IndexOf('`'));
            var genericTypeNames = type.GetGenericArguments().Select(x => x.Name);
            return $"{genericName}<{string.Join(",", genericTypeNames)}>";
        }

        private bool ShouldIncludeProperty(PropertyInfo property)
        {
            if (!property.GetMethod.IsPublic)
                return false;
            if(property.GetCustomAttributes<JsonIgnoreAttribute>().Any())
                return false;
            return true;
        }
    }
}
