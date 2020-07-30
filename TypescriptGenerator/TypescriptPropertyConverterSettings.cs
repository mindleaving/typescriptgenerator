using System;
using System.Collections.Generic;

namespace TypescriptGenerator
{
    public class TypescriptPropertyConverterSettings
    {
        public CasingType Casing { get; set; } = CasingType.CamelCase;
        public Func<string, string> CustomPropertyNamingFunc { get; set; }
        public List<ITypeConverter> TypeConverters { get; } = new List<ITypeConverter>();
    }
}