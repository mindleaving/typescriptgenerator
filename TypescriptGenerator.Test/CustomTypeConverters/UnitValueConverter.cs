using System;
using Commons.Physics;
using TypescriptGenerator.Converters;

namespace TypescriptGenerator.Test.CustomTypeConverters
{
    /// <summary>
    /// Convert mindleaving.Commons.UnitValue to math.Unit of math.js.
    /// 
    /// JavaScript must convert the received string to math.Unit.
    /// Automatic conversion of math.Unit to UnitValue is supported.
    /// </summary>
    public class UnitValueConverter : ITypeConverter
    {
        public bool IsMatchingType(Type type)
        {
            return type == typeof(UnitValue);
        }

        public string Convert(Type type)
        {
            return "math.Unit";
        }
    }
}
