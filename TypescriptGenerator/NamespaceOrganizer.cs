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
            string commonTranslatedPrefix = null)
        {
            IEnumerable<ITypescriptObject> matchingTypes = typescriptObjects;
            if (commonTranslatedPrefix != null)
                matchingTypes = matchingTypes.Where(x => x.TranslatedNamespace.StartsWith(commonTranslatedPrefix));
            var distinctTranslatedNamespaceNames = matchingTypes
                .Select(x => commonTranslatedPrefix != null 
                    ? x.TranslatedNamespace.RemovePrefix(commonTranslatedPrefix) 
                    : x.TranslatedNamespace)
                .Select(x => x.Split('.')[0])
                .Distinct();
            var namespaces = new List<TypescriptNamespace>();
            foreach (var translatedName in distinctTranslatedNamespaceNames)
            {
                var translatedFullName = commonTranslatedPrefix + translatedName;
                var matchingSettings = settings.NamespaceSettings
                    .Where(x => translatedFullName.StartsWith(x.Translation))
                    .OrderByDescending(x => x.Translation.Length)
                    .FirstOrDefault();

                var namespaceTypes = typescriptObjects.Where(x => x.TranslatedNamespace == translatedFullName).ToList();
                var subNamespaceTypes = typescriptObjects
                    .Where(x => x.TranslatedNamespace.StartsWith(translatedFullName + "."))
                    .ToList();
                var subNamespaces = OrganizeSubnamespaces(subNamespaceTypes, translatedFullName + ".");
                if(namespaceTypes.Count == 0 && subNamespaces.Count == 0)
                    continue;

                var typescriptNamespace = new TypescriptNamespace(
                    translatedName,
                    translatedFullName,
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
