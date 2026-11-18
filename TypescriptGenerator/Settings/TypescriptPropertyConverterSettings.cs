using System;
using System.Collections.Generic;
using System.Reflection;
using TypescriptGenerator.Converters;

namespace TypescriptGenerator.Settings
{
    public class TypescriptPropertyConverterSettings
    {
        public CasingType Casing { get; set; } = CasingType.CamelCase;
        public Func<PropertyInfo, string?>? CustomPropertyNamingFunc { get; set; }
        public List<ITypeConverter> TypeConverters { get; } = [];
    }
}