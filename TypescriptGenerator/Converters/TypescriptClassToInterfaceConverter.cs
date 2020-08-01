using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
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
            if(!type.IsClass && !type.IsInterface)
                throw new ArgumentException("Type is not a class nor an interface");

            var typescriptProperties = type.GetProperties()
                .Where(ShouldIncludeProperty)
                .Select(propertyConverter.Convert)
                .ToList();

            var directDependencies = type.GetProperties()
                .Select(x => x.PropertyType)
                .Where(x => !TypeDeterminer.IsPrimitiveType(x))
                .ToList();
            var translatedNamespace = NamespaceTranslator.Translate(type.Namespace, namespaceSettings);
            return new TypescriptInterface(
                translatedNamespace,
                type.Name, // TODO: Apply transforms
                typescriptProperties,
                directDependencies,
                settings.Modifiers);
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
