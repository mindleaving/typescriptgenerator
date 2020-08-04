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
            string commonPrefix = null,
            string commonTranslatedPrefix = null)
        {
            IEnumerable<ITypescriptObject> matchingTypes = typescriptObjects;
            if (commonPrefix != null)
                matchingTypes = matchingTypes.Where(x => x.OriginalNamespace.StartsWith(commonPrefix));
            var distinctNamespaceNames = matchingTypes
                .Select(x => (commonPrefix != null ? x.OriginalNamespace.RemovePrefix(commonPrefix) : x.OriginalNamespace).Split('.')[0])
                .Distinct();
            var namespaces = new List<TypescriptNamespace>();
            foreach (var originalName in distinctNamespaceNames)
            {
                var originalFullName = commonPrefix + originalName;
                var matchingSettings = settings.NamespaceSettings.GetMostSpecificMatch(originalFullName);
                var translatedFullName = NamespaceTranslator.Translate(originalFullName, settings.NamespaceSettings);
                var translatedName = translatedFullName.RemovePrefix(commonTranslatedPrefix);

                var namespaceTypes = typescriptObjects.Where(x => x.OriginalNamespace == originalFullName).ToList();
                var subNamespaceTypes = typescriptObjects
                    .Where(x => x.OriginalNamespace.StartsWith(originalFullName + "."))
                    .ToList();
                var subNamespaces = OrganizeSubnamespaces(subNamespaceTypes, originalFullName + ".", translatedFullName + ".");
                if(namespaceTypes.Count == 0 && subNamespaces.Count == 0)
                    continue;

                var typescriptNamespace = new TypescriptNamespace(
                    translatedName,
                    translatedFullName,
                    originalName,
                    originalFullName,
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
