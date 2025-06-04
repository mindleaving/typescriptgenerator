using System.Collections.Generic;

namespace TypescriptGenerator.Settings
{
    public class TypescriptClassToInterfaceConverterSettings
    {
        public List<string> Modifiers { get; } = [];
        public TypescriptPropertyConverterSettings PropertySettings { get; } = new TypescriptPropertyConverterSettings();
    }
}