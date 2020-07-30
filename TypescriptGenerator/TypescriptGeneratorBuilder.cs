using System;
using System.Collections.Generic;
using System.Reflection;

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
            var baseSettings = new NamespaceSettings(namespaceName);
            options(baseSettings);
            if(baseSettings.Translation != null)
                generator.NamespaceTranslations.Add(namespaceName, baseSettings.Translation);
            if(baseSettings.Filename != null)
                generator.NamespaceFilenameMap.Add(namespaceName, baseSettings.Filename);
            return generator;
        }
        public static TypescriptGenerator SetOutputDirectory(this TypescriptGenerator generator, string directory)
        {
            generator.OutputDirectory = directory;
            return generator;
        }
    }
}
