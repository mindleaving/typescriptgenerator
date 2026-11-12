using System;
using System.Linq;
using TypescriptGenerator.Objects;
using TypescriptGenerator.Settings;

namespace TypescriptGenerator.Formatter;

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
            .Select(value => $"{FormatEnumValue(value, settings.EnumNameSource)} = \"{FormatEnumValue(value, settings.StringValueSource)}\"");
        var modifiers = typescriptEnum.Modifiers.Any()
            ? string.Join(" ", typescriptEnum.Modifiers) + " "
            : "";
        return $@"{modifiers}enum {typescriptEnum.Name} {{
{settings.IndentString}{string.Join("," + settings.NewLine + settings.IndentString, values)}
}}";
    }

    private static string FormatEnumValue(
        TypescriptEnumValue enumValue,
        EnumNameSource nameSource)
    {
        return nameSource switch
        {
            EnumNameSource.CSharpName => enumValue.CSharpName,
            EnumNameSource.EnumMemberAttribute => enumValue.EnumMemberName ?? enumValue.CSharpName,
            _ => throw new ArgumentOutOfRangeException(nameof(nameSource), nameSource, null)
        };
    }
}