using System.Collections.Generic;
using System.Linq;
using TypescriptGenerator.Extensions;
using TypescriptGenerator.Objects;
using TypescriptGenerator.Settings;

namespace TypescriptGenerator
{
    public class NamespaceOrganizer
    {
        private readonly NamespaceOrganizerSettings settings;

        public NamespaceOrganizer(NamespaceOrganizerSettings settings)
        {
            this.settings = settings;
        }

        public List<TypescriptNamespace> Organize(List<ITypescriptObject> typescriptObjects)
        {
            return OrganizeSubnamespaces(typescriptObjects);
        }

        private List<TypescriptNamespace> OrganizeSubnamespaces(
            List<ITypescriptObject> typescriptObjects,
            string commonPrefix = null)
        {
            IEnumerable<ITypescriptObject> matchingTypes = typescriptObjects;
            if (commonPrefix != null)
                matchingTypes = matchingTypes.Where(x => x.Namespace.StartsWith(commonPrefix));
            var distinctNamespaceNames = matchingTypes
                .Select(x => (commonPrefix != null ? x.Namespace.RemovePrefix(commonPrefix) : x.Namespace).Split('.')[0])
                .Distinct();
            var namespaces = new List<TypescriptNamespace>();
            foreach (var namespaceName in distinctNamespaceNames)
            {
                var fullNamespaceName = commonPrefix + namespaceName;
                var namespaceTypes = typescriptObjects.Where(x => x.Namespace == fullNamespaceName).ToList();
                var subNamespaceTypes = typescriptObjects
                    .Where(x => x.Namespace.StartsWith(fullNamespaceName + "."))
                    .ToList();
                var subNamespaces = OrganizeSubnamespaces(subNamespaceTypes, namespaceName + ".");
                if(namespaceTypes.Count == 0 && subNamespaces.Count == 0)
                    continue;

                var matchingSettings = settings.NamespaceSettings.GetMostSpecificMatch(fullNamespaceName);
                var translatedName = NamespaceTranslator.Translate(fullNamespaceName, settings.NamespaceSettings);
                var typescriptNamespace = new TypescriptNamespace(
                    translatedName,
                    settings.Modifiers,
                    namespaceTypes,
                    subNamespaces,
                    matchingSettings?.Filename);
                namespaces.Add(typescriptNamespace);
            }

            return namespaces;
        }
    }
}
