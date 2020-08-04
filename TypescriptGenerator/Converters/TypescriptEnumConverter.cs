using System;
using System.Collections.Generic;
using System.Linq;
using TypescriptGenerator.Objects;
using TypescriptGenerator.Settings;

namespace TypescriptGenerator.Converters
{
    public class TypescriptEnumConverter
    {
        private readonly TypescriptEnumConverterSettings settings;
        private readonly List<NamespaceSettings> namespaceSettings;

        public TypescriptEnumConverter(
            TypescriptEnumConverterSettings settings,
            List<NamespaceSettings> namespaceSettings)
        {
            this.settings = settings;
            this.namespaceSettings = namespaceSettings;
        }

        public TypescriptEnum Convert(Type type)
        {
            if(!type.IsEnum)
                throw new ArgumentException($"Type '{type.FullName}' is not an enum");

            var translatedNamespace = settings.EnumsIntoSeparateFile
                ? "Enums"
                : NamespaceTranslator.Translate(type.Namespace, namespaceSettings);
            return new TypescriptEnum(
                type.Namespace, 
                translatedNamespace,
                type.Name, // TODO: Apply transforms
                settings.Modifiers, 
                Enum.GetNames(type).ToList());
        }
    }
}
