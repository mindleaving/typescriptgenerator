using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace TypescriptGenerator
{
    public class TypescriptClassToInterfaceConverter
    {
        private readonly TypescriptClassToInterfaceConverterSettings settings;
        private readonly TypescriptPropertyConverter propertyConverter;

        public TypescriptClassToInterfaceConverter(TypescriptClassToInterfaceConverterSettings settings = null)
        {
            this.settings = settings ?? new TypescriptClassToInterfaceConverterSettings();
            propertyConverter = new TypescriptPropertyConverter();
        }

        public TypescriptInterface Convert(Type type)
        {
            if(!type.IsClass)
                throw new ArgumentException("Type is not a class");

            var typescriptProperties = type.GetProperties()
                .Where(ShouldIncludeProperty)
                .Select(propertyConverter.Convert)
                .ToList();

            var directDependencies = type.GetProperties()
                .Select(x => x.PropertyType)
                .Where(x => !IsPrimitiveType(x))
                .ToList();
            return new TypescriptInterface(
                type.Namespace,
                type.Name, // TODO: Apply transforms
                typescriptProperties,
                directDependencies);
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
            { typeof(double), "number"},
            { typeof(long), "number"},
            { typeof(ulong), "number"},
            { typeof(Guid), "string"},
            { typeof(object), "{}"}
        };
        private bool IsPrimitiveType(Type type)
        {
            return PrimitiveTypes.ContainsKey(type);
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
