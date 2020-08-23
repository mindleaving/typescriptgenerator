using System;
using System.Collections.Generic;

namespace TypescriptGenerator.Attributes
{
    public class TypescriptTypeAttribute : Attribute
    {
        public string TypeString { get; }
        public List<Type> Dependencies { get; }

        public TypescriptTypeAttribute(string typeString, params Type[] dependencies)
        {
            TypeString = typeString;
            Dependencies = new List<Type>(dependencies);
        }
    }
}
