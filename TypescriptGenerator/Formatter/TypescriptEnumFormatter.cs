using System;
using System.Linq;
using TypescriptGenerator.Objects;
using TypescriptGenerator.Settings;

namespace TypescriptGenerator.Formatter
{
    public class TypescriptEnumFormatter
    {
        private readonly GeneralFormatterSettings settings;

        public TypescriptEnumFormatter(GeneralFormatterSettings settings)
        {
            this.settings = settings;
        }

        public string Format(TypescriptEnum typescriptEnum)
        {
            var values = typescriptEnum.Values
                .Select(value => $"{value} = \"{value}\"");
            var modifiers = typescriptEnum.Modifiers.Any()
                ? string.Join(" ", typescriptEnum.Modifiers) + " "
                : "";
return $@"{modifiers}enum {typescriptEnum.Name} {{
{settings.IndentString}{string.Join(Environment.NewLine + settings.IndentString, values)}
}}";
        }
    }
}
