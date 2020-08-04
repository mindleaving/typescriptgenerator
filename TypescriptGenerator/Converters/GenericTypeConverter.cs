using System;

namespace TypescriptGenerator.Converters
{
    public class GenericTypeConverter : ITypeConverter
    {
        private readonly Func<Type, bool> matchFunc;
        private readonly Func<Type, string> convertFunc;

        public GenericTypeConverter(
            Func<Type, bool> matchFunc,
            Func<Type, string> convertFunc)
        {
            this.matchFunc = matchFunc;
            this.convertFunc = convertFunc;
        }

        public bool IsMatchingType(Type type)
        {
            return matchFunc(type);
        }

        public string Convert(Type type)
        {
            return convertFunc(type);
        }
    }
}