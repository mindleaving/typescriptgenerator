using System.Collections.Generic;

namespace TypescriptGenerator.Objects
{
    public class TypescriptEnum : ITypescriptObject
    {
        public TypescriptEnum(
            string namespaceName,
            string name,
            List<string> modifiers,
            List<string> values)
        {
            Namespace = namespaceName;
            Name = name;
            Modifiers = modifiers;
            Values = values;
        }

        public string Namespace { get; }
        public string Name { get; }
        public List<string> Modifiers { get; }
        public List<string> Values { get; }
    }
}