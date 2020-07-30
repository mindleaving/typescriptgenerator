using System;

namespace TypescriptGenerator
{
    public interface ITypeConverter
    {
        bool IsMatchingType(Type type);
        string Convert(Type type);
    }
}