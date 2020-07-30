using System;
using System.Linq;

namespace TypescriptGenerator.Formatter
{
    public class TypescriptInterfaceFormatter
    {
        public string Format(TypescriptInterface tsInterface)
        {
            var properties = tsInterface.Properties.Select(FormatProperty);
            return 
$@"{string.Join(" ", tsInterface.Modifiers)} interface {tsInterface.Name} {{
    {string.Join(Environment.NewLine + "    ", properties)}
}}";
        }

        private string FormatProperty(TypescriptProperty property)
        {
            var optionalModifier = property.IsOptional ? "?" : "";
            return $"{property.Name}{optionalModifier}: {property.Type};";
        }
    }
}
