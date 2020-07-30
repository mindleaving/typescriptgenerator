using System;
using System.Collections.Generic;
using System.Linq;

namespace TypescriptGenerator
{
    public class TypescriptGenerator
    {
        public static TypescriptGenerator Builder => new TypescriptGenerator();
        public List<Type> IncludedTypes { get; } = new List<Type>();
        public List<Type> ExcludedTypes { get; } = new List<Type>();

        public bool EnumsIntoSeparateFile { get; set; }
        public Dictionary<string, string> NamespaceTranslations { get; } = new Dictionary<string, string>();
        public Dictionary<string, string> NamespaceFilenameMap { get; } = new Dictionary<string, string>();
        public string OutputDirectory { get; set; }

        public void Generate()
        {
            var types = new Dictionary<Type, ITypescriptObject>();
            var typeQueue = new Queue<Type>(IncludedTypes.Except(ExcludedTypes));
            var classConverter = new TypescriptClassToInterfaceConverter(new TypescriptClassToInterfaceConverterSettings());
            var enumConverter = new TypescriptEnumConverter();
            while (typeQueue.Count > 0)
            {
                var type = typeQueue.Dequeue();
                if (type.IsEnum)
                {
                    var typescriptEnum = enumConverter.Convert(type);
                    types.Add(type, typescriptEnum);
                }
                else
                {
                    var typescriptInterface = classConverter.Convert(type);
                    types.Add(type, typescriptInterface);
                    foreach (var dependency in typescriptInterface.DirectDependencies)
                    {
                        if(types.ContainsKey(dependency))
                            continue;
                        if(typeQueue.Contains(dependency))
                            continue;
                        if(ExcludedTypes.Contains(dependency))
                            continue;
                        typeQueue.Enqueue(dependency);
                    }
                }
            }
        }
    }
}
 