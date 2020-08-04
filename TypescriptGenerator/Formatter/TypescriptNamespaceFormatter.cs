using System;
using System.Linq;
using TypescriptGenerator.Objects;
using TypescriptGenerator.Settings;

namespace TypescriptGenerator.Formatter
{
    public class TypescriptNamespaceFormatter
    {
        private readonly GeneralFormatterSettings settings;
        private readonly TypescriptEnumFormatter enumFormatter;
        private readonly TypescriptInterfaceFormatter interfaceFormatter;

        public TypescriptNamespaceFormatter(GeneralFormatterSettings settings)
        {
            this.settings = settings;
            enumFormatter = new TypescriptEnumFormatter(settings);
            interfaceFormatter = new TypescriptInterfaceFormatter(settings);
        }

        public string Format(TypescriptNamespace typescriptNamespace)
        {
            var modifiers = typescriptNamespace.Modifiers.Any()
                ? string.Join(" ", typescriptNamespace.Modifiers) + " "
                : "";
            var formattedTypes = typescriptNamespace.Types
                .Select(FormatType)
                .Select(Indent);
            var subNamespaces = typescriptNamespace.SubNamespaces
                .Select(Format)
                .Select(Indent);
            return $@"{modifiers}namespace {typescriptNamespace.TranslatedName} {{
{string.Join(Environment.NewLine + Environment.NewLine, formattedTypes.Concat(subNamespaces))}
}}";
        }

        private string FormatType(ITypescriptObject typescriptObject)
        {
            switch (typescriptObject)
            {
                case TypescriptEnum typescriptEnum:
                    return enumFormatter.Format(typescriptEnum);
                case TypescriptInterface typescriptInterface:
                    return interfaceFormatter.Format(typescriptInterface);
                default:
                    throw new ArgumentOutOfRangeException(nameof(typescriptObject));
            }
        }

        private string Indent(string typeString)
        {
            return settings.IndentString + typeString.Replace(Environment.NewLine, Environment.NewLine + settings.IndentString);
        }
    }
}
