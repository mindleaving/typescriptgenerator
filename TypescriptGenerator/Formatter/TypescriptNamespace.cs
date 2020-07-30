using System.Collections.Generic;

namespace TypescriptGenerator.Formatter
{
    public class TypescriptNamespace
    {
        public string Name { get; }
        public List<string> Modifiers { get; }
        public List<ITypescriptObject> Types { get; }
    }
}