using System;
using System.Linq;
using TypescriptGenerator.Objects;
using TypescriptGenerator.Settings;

namespace TypescriptGenerator.Formatter
{
    public class TypescriptInterfaceFormatter
    {
        private readonly GeneralFormatterSettings settings;

        public TypescriptInterfaceFormatter(GeneralFormatterSettings settings)
        {
            this.settings = settings;
        }

        public string Format(TypescriptInterface tsInterface)
        {
            var properties = tsInterface.Properties.Select(FormatProperty);
            var modifiers = tsInterface.Modifiers.Any()
                ? string.Join(" ", tsInterface.Modifiers) + " "
                : "";
            return 
$@"{modifiers}interface {tsInterface.Name} {{
{settings.IndentString}{string.Join(Environment.NewLine + settings.IndentString, properties)}
}}";
        }

        private string FormatProperty(TypescriptProperty property)
        {
            var optionalModifier = property.IsOptional ? "?" : "";
            return $"{property.Name}{optionalModifier}: {property.FormattedType};";
        }
    }
}
