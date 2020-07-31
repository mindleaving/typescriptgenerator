using System;
using System.Collections.Generic;
using System.Reflection;
using TypescriptGenerator.Settings;

namespace TypescriptGenerator
{
    public static class TypescriptGeneratorBuilder
    {
        public static TypescriptGenerator IncludeAllInAssemblyContainingType<T>(this TypescriptGenerator generator)
        {
            var assembly = Assembly.GetAssembly(typeof(T));
            generator.IncludedTypes.AddRange(assembly.GetExportedTypes());
            return generator;
        }
        public static TypescriptGenerator Include<T>(this TypescriptGenerator generator)
        {
            generator.IncludedTypes.Add(typeof(T));
            return generator;
        }
        public static TypescriptGenerator Exclude<T>(this TypescriptGenerator generator)
        {
            generator.ExcludedTypes.Add(typeof(T));
            return generator;
        }
        public static TypescriptGenerator EnumsIntoSeparateFile(this TypescriptGenerator generator)
        {
            generator.EnumsIntoSeparateFile = true;
            return generator;
        }
        public static TypescriptGenerator ConfigureNamespace(this TypescriptGenerator generator, string namespaceName, Action<NamespaceSettings> options)
        {
            var namespaceSettings = new NamespaceSettings(namespaceName);
            options(namespaceSettings);
            generator.NamespaceSettings.Add(namespaceSettings);
            return generator;
        }
        public static TypescriptGenerator SetOutputDirectory(this TypescriptGenerator generator, string directory)
        {
            generator.OutputDirectory = directory;
            return generator;
        }
        public static TypescriptGenerator SetIndent(this TypescriptGenerator generator, string indentString)
        {
            generator.FormatterSettings.IndentString = indentString;
            return generator;
        }
    }
}
