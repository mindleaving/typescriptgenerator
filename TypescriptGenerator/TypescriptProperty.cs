namespace TypescriptGenerator
{
    public class TypescriptProperty
    {
        public TypescriptProperty(string name,
            string type,
            bool isOptional)
        {
            Name = name;
            Type = type;
            IsOptional = isOptional;
        }

        public string Name { get; }
        public string Type { get; }
        public bool IsOptional { get; }
    }
}