using System;
using System.Collections.Generic;

namespace TypescriptGenerator.Objects
{
    public class TypescriptProperty
    {
        public TypescriptProperty(string name,
            string formattedType,
            bool isOptional,
            List<Type> dependencies)
        {
            Name = name;
            FormattedType = formattedType;
            IsOptional = isOptional;
            Dependencies = dependencies;
        }

        public string Name { get; }
        public string FormattedType { get; }
        public bool IsOptional { get; }
        public List<Type> Dependencies { get; }
    }
}