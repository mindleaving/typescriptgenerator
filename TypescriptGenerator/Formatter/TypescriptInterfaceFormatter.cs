using System;
using System.Linq;
using TypescriptGenerator.Converters;
using TypescriptGenerator.Objects;
using TypescriptGenerator.Settings;

namespace TypescriptGenerator.Formatter
{
    public class TypescriptInterfaceFormatter
    {
        private readonly GeneralFormatterSettings settings;
        private readonly TypeDeterminer typeDeterminer;

        public TypescriptInterfaceFormatter(GeneralFormatterSettings settings, TypeDeterminer typeDeterminer)
        {
            this.settings = settings;
            this.typeDeterminer = typeDeterminer;
        }

        public string Format(TypescriptInterface tsInterface)
        {
            var properties = tsInterface.Properties.Select(FormatProperty);
            var modifiers = tsInterface.Modifiers.Any()
                ? string.Join(" ", tsInterface.Modifiers) + " "
                : "";
            var extensions = tsInterface.BaseClassAndInterfaces.Any()
                ? " extends " + string.Join(", ", tsInterface.BaseClassAndInterfaces.Select(typeDeterminer.Determine).Select(x => x.FormattedType))
                : "";
            return 
$@"{modifiers}interface {tsInterface.Name}{extensions} {{
{settings.IndentString}{string.Join(settings.NewLine + settings.IndentString, properties)}
}}";
        }

        private string FormatProperty(TypescriptProperty property)
        {
            var optionalModifier = property.IsOptional ? "?" : "";
            return $"{property.Name}{optionalModifier}: {property.FormattedType};";
        }
    }
}
