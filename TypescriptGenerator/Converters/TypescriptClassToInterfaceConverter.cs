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
        private readonly TypescriptClassToInterfaceConverterSettings settings;
        private readonly TypescriptPropertyConverter propertyConverter;

        public TypescriptClassToInterfaceConverter(
            TypescriptClassToInterfaceConverterSettings settings = null,
            List<NamespaceSettings> namespaceSettings = null)
        {
            this.settings = settings ?? new TypescriptClassToInterfaceConverterSettings();
            propertyConverter = new TypescriptPropertyConverter(
                this.settings.PropertySettings, 
                namespaceSettings ?? new List<NamespaceSettings>());
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
            return new TypescriptInterface(
                type.Namespace,
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
