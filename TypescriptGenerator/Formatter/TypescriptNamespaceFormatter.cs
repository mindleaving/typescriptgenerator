using System;
using System.Linq;

namespace TypescriptGenerator.Formatter
{
    public class TypescriptNamespaceFormatter
    {
        private readonly TypescriptEnumFormatter enumFormatter;
        private readonly TypescriptInterfaceFormatter interfaceFormatter;

        public TypescriptNamespaceFormatter()
        {
            enumFormatter = new TypescriptEnumFormatter();
            interfaceFormatter = new TypescriptInterfaceFormatter();
        }

        public string Format(TypescriptNamespace typescriptNamespace)
        {
            var formattedTypes = typescriptNamespace.Types
                .Select(FormatType)
                .Select(Indent);
return $@"{string.Join(" ", typescriptNamespace.Modifiers)} namespace {typescriptNamespace.Name} {{
    {string.Join(Environment.NewLine + Environment.NewLine, formattedTypes)}
}}";
        }

        private string FormatType(ITypescriptObject arg)
        {
            switch (arg)
            {
                case TypescriptEnum typescriptEnum:
                    return enumFormatter.Format(typescriptEnum);
                case TypescriptInterface typescriptInterface:
                    return interfaceFormatter.Format(typescriptInterface);
                default:
                    throw new ArgumentOutOfRangeException(nameof(arg));
            }
        }

        private static string Indent(string typeString)
        {
            return "    " + typeString.Replace(Environment.NewLine, Environment.NewLine + "    ");
        }
    }
}
