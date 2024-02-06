using System;
using System.Linq;
using System.Reflection;
using TypescriptGenerator.Converters;
using TypescriptGenerator.Settings;

namespace TypescriptGenerator
{
    public static class TypescriptGeneratorBuilder
    {
        public static TypescriptGenerator IncludeAllInAssemblyContainingType<T>(
            this TypescriptGenerator generator)
        {
            var assembly = Assembly.GetAssembly(typeof(T));
            generator.IncludedTypes.AddRange(assembly.GetExportedTypes());
            return generator;
        }

        public static TypescriptGenerator IncludeAllInNamespace(
            this TypescriptGenerator generator,
            Assembly assembly,
            string namespaceName)
        {
            var matchingTypes = assembly.GetExportedTypes()
                .Where(x => x.Namespace != null && (x.Namespace == namespaceName || x.Namespace.StartsWith(namespaceName + ".")));
            generator.IncludedTypes.AddRange(matchingTypes);
            return generator;
        }
        public static TypescriptGenerator Include<T>(this TypescriptGenerator generator)
        {
            generator.IncludedTypes.Add(typeof(T));
            return generator;
        }
        public static TypescriptGenerator ExcludeAllInAssemblyContainingType<T>(
            this TypescriptGenerator generator)
        {
            var assembly = Assembly.GetAssembly(typeof(T));
            generator.ExcludedTypes.AddRange(assembly.GetExportedTypes());
            return generator;
        }
        public static TypescriptGenerator ExcludeAllInNamespace(
            this TypescriptGenerator generator,
            Assembly assembly,
            string namespaceName)
        {
            var matchingTypes = assembly.GetExportedTypes()
                .Where(x => x.Namespace != null && (x.Namespace == namespaceName || x.Namespace.StartsWith(namespaceName + ".")));
            generator.ExcludedTypes.AddRange(matchingTypes);
            return generator;
        }
        public static TypescriptGenerator Exclude<T>(this TypescriptGenerator generator)
        {
            generator.ExcludedTypes.Add(typeof(T));
            return generator;
        }
        public static TypescriptGenerator EnumsIntoSeparateFile(this TypescriptGenerator generator)
        {
            generator.EnumSettings.EnumsIntoSeparateFile = true;
            return generator;
        }
        public static TypescriptGenerator EnumModifiers(this TypescriptGenerator generator, params string[] modifiers)
        {
            foreach (var modifier in modifiers)
            {
                if(generator.EnumSettings.Modifiers.Contains(modifier))
                    continue;
                generator.EnumSettings.Modifiers.Add(modifier);
            }
            return generator;
        }
        public static TypescriptGenerator NamespaceModifiers(this TypescriptGenerator generator, params string[] modifiers)
        {
            foreach (var modifier in modifiers)
            {
                if(generator.NamespaceModifiers.Contains(modifier))
                    continue;
                generator.NamespaceModifiers.Add(modifier);
            }
            return generator;
        }
        public static TypescriptGenerator ConfigureNamespace(this TypescriptGenerator generator, string namespaceName, Action<NamespaceSettings> options)
        {
            if(generator.NamespaceSettings.Exists(x => x.Namespace == namespaceName))
                throw new InvalidOperationException($"Namespace '{namespaceName}' is already configured");
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
        public static TypescriptGenerator SetDefaultFilenameForInterfaces(this TypescriptGenerator generator, string filename)
        {
            generator.DefaultFilename = filename;
            return generator;
        }
        public static TypescriptGenerator SetDefaultFilenameForEnums(this TypescriptGenerator generator, string filename)
        {
            generator.DefaultEnumFilename = filename;
            return generator;
        }
        public static TypescriptGenerator SetIndent(this TypescriptGenerator generator, string indentString)
        {
            generator.FormatterSettings.IndentString = indentString;
            return generator;
        }
        public static TypescriptGenerator CustomizeType(this TypescriptGenerator generator, Func<Type, bool> matchFunc, Func<Type, string> convertFunc)
        {
            generator.CustomTypeConverters.Add(new GenericTypeConverter(matchFunc, convertFunc));
            return generator;
        }
        public static TypescriptGenerator ReactDefaults(this TypescriptGenerator generator)
        {
            return generator
                .EnumsIntoSeparateFile()
                .EnumModifiers("export")
                .NamespaceModifiers("export");
        }
    }
}
