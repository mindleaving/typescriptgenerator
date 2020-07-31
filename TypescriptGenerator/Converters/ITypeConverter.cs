using System;

namespace TypescriptGenerator.Converters
{
    public interface ITypeConverter
    {
        bool IsMatchingType(Type type);
        string Convert(Type type);
    }
}