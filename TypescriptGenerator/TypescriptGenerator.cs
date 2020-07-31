using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TypescriptGenerator.Converters;
using TypescriptGenerator.Formatter;
using TypescriptGenerator.Objects;
using TypescriptGenerator.Settings;

namespace TypescriptGenerator
{
    public class TypescriptGenerator
    {
        public static TypescriptGenerator Builder => new TypescriptGenerator();
        public List<Type> IncludedTypes { get; } = new List<Type>();
        public List<Type> ExcludedTypes { get; } = new List<Type>();

        public bool EnumsIntoSeparateFile { get; set; }
        public List<NamespaceSettings> NamespaceSettings { get; } = new List<NamespaceSettings>();
        public string OutputDirectory { get; set; } = ".";
        public GeneralFormatterSettings FormatterSettings { get; } = new GeneralFormatterSettings();

        public string DefaultFilename { get; set; } = "models.d.ts";
        public string DefaultEnumFilename { get; set; } = "enums.d.ts";

        public void Generate()
        {
            var types = CollectTypes();
            IEnumerable<ITypescriptObject> namespacedTypes = types;
            var hasEnums = false;
            if (EnumsIntoSeparateFile)
            {
                var enums = types.OfType<TypescriptEnum>().ToList();
                var enumFormatter = new TypescriptEnumFormatter(FormatterSettings);
                var lines = new List<string>();
                foreach (var @enum in enums)
                {
                    var formattedEnum = enumFormatter.Format(@enum);
                    lines.Add(formattedEnum);
                }
                var outputFilePath = Path.Combine(OutputDirectory, DefaultEnumFilename);
                File.WriteAllLines(outputFilePath, lines);

                namespacedTypes = types.Except(enums);
                hasEnums = enums.Any();
            }

            var namespaceOrganizer = new NamespaceOrganizer(new NamespaceOrganizerSettings(NamespaceSettings));
            var namespaces = namespaceOrganizer.Organize(namespacedTypes.ToList());
            var files = namespaces.GroupBy(x => x.OutputFilename ?? DefaultFilename);
            var namespaceFormatter = new TypescriptNamespaceFormatter(FormatterSettings);
            foreach (var filename in files)
            {
                var fileNamespaces = filename.ToList();
                var lines = new List<string>();
                if(EnumsIntoSeparateFile && hasEnums)
                {
                    lines.Add($"import * as Enums from './{DefaultEnumFilename}'");
                    lines.Add("");
                }
                foreach (var ns in fileNamespaces)
                {
                    var formattedNamespace = namespaceFormatter.Format(ns);
                    lines.Add(formattedNamespace);
                }
                var outputFilePath = Path.Combine(OutputDirectory, filename.Key);
                File.WriteAllLines(outputFilePath, lines);
            }
        }

        private List<ITypescriptObject> CollectTypes()
        {
            var types = new Dictionary<Type, ITypescriptObject>();
            var typeQueue = new Queue<Type>(IncludedTypes.Except(ExcludedTypes));
            var classConverter = new TypescriptClassToInterfaceConverter(new TypescriptClassToInterfaceConverterSettings(), NamespaceSettings);
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
                        if (types.ContainsKey(dependency))
                            continue;
                        if (typeQueue.Contains(dependency))
                            continue;
                        if (ExcludedTypes.Contains(dependency))
                            continue;
                        typeQueue.Enqueue(dependency);
                    }
                }
            }

            return types.Values.ToList();
        }
    }
}
 