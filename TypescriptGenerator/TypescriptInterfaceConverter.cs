using System;
using System.Linq;

namespace TypescriptGenerator
{
    public class TypescriptInterfaceConverter
    {
        private readonly TypescriptPropertyConverter propertyConverter;

        public TypescriptInterfaceConverter(TypescriptPropertyConverter propertyConverter)
        {
            this.propertyConverter = propertyConverter;
        }

        public TypescriptInterface Convert(Type type)
        {
            if(!type.IsInterface)
                throw new ArgumentException("Type is not an interface");

            throw new NotImplementedException();
        }
    }
}
