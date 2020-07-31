using System.Collections.Generic;

namespace TypescriptGenerator.Settings
{
    public class TypescriptClassToInterfaceConverterSettings
    {
        public List<string> Modifiers { get; } = new List<string>();
        public TypescriptPropertyConverterSettings PropertySettings { get; } = new TypescriptPropertyConverterSettings();
    }
}