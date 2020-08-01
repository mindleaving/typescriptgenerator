using System.Collections.Generic;

namespace TypescriptGenerator.Settings
{
    public class TypescriptEnumConverterSettings
    {
        public bool EnumsIntoSeparateFile { get; set; }
        public List<string> Modifiers { get; } = new List<string>();
    }
}
