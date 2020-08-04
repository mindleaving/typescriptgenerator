using System.Collections.Generic;
using System.Linq;
using TypescriptGenerator.Extensions;
using TypescriptGenerator.Objects;
using TypescriptGenerator.Settings;

namespace TypescriptGenerator
{
    public class ImportResolver
    {
        private readonly Dictionary<string, List<TypescriptNamespace>> files;
        private readonly List<NamespaceSettings> namespaceSettings;
        private readonly string defaultFilename;

        public ImportResolver(
            Dictionary<string, List<TypescriptNamespace>> files,
            List<NamespaceSettings> namespaceSettings, 
            string defaultFilename)
        {
            this.files = files;
            this.namespaceSettings = namespaceSettings;
            this.defaultFilename = defaultFilename;
        }

        public IEnumerable<string> ResolveForFile(List<TypescriptNamespace> fileNamespaces, string filename)
        {
            var dependencyTypes = fileNamespaces.RecursiveSelectMany(
                    x => x.SubNamespaces,
                    x => x.Types.OfType<TypescriptInterface>()
                        .SelectMany(type => type.DirectDependencies))
                .Distinct();
            var dependentOnFiles = dependencyTypes
                .Select(type => NamespaceFileFinder.GetFileContainingType(type, namespaceSettings, defaultFilename))
                .Distinct()
                .Except(new[] {filename});
            foreach (var dependentOnFile in dependentOnFiles)
            {
                var dependentOnNamespaces = files[dependentOnFile]
                    .Select(x => x.TranslatedName);
                yield return $"import {{ {string.Join(", ", dependentOnNamespaces)} }} from './{dependentOnFile}'";
            }
        }
    }
}
