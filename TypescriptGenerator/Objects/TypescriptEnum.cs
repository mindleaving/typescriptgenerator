using System.Collections.Generic;

namespace TypescriptGenerator.Objects;

public class TypescriptEnum : ITypescriptObject
{
    public TypescriptEnum(
        string originalNamespace,
        string translatedNamespace,
        string name,
        List<string> modifiers,
        List<TypescriptEnumValue> values)
    {
        OriginalNamespace = originalNamespace;
        TranslatedNamespace = translatedNamespace;
        Name = name;
        Modifiers = modifiers;
        Values = values;
    }

    public string OriginalNamespace { get; }
    public string TranslatedNamespace { get; }
    public string Name { get; }
    public List<string> Modifiers { get; }
    public List<TypescriptEnumValue> Values { get; }
}

public class TypescriptEnumValue(
    string cSharpName,
    string? enumMemberName)
{
    public string CSharpName { get; } = cSharpName;
    public string? EnumMemberName { get; } = enumMemberName;
}