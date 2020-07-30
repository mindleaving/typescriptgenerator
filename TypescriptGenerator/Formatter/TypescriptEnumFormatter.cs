using System;
using System.Linq;

namespace TypescriptGenerator.Formatter
{
    public class TypescriptEnumFormatter
    {
        public string Format(TypescriptEnum typescriptEnum)
        {
            var values = typescriptEnum.Values
                .Select(value => $"{value} = \"{value}\"");
return $@"{string.Join(" ", typescriptEnum.Modifiers)} enum {typescriptEnum.Name} {{
    {string.Join(Environment.NewLine + "    ", values)}
}}";
        }
    }
}
