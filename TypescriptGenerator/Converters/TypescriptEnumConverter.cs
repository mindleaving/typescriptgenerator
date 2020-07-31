﻿using System;
using System.Collections.Generic;
using System.Linq;
using TypescriptGenerator.Objects;

namespace TypescriptGenerator.Converters
{
    public class TypescriptEnumConverter
    {
        public TypescriptEnum Convert(Type type)
        {
            if(!type.IsEnum)
                throw new ArgumentException("Type is not an enum");

            return new TypescriptEnum(
                type.Namespace, 
                type.Name, // TODO: Apply transforms
                new List<string>(), 
                Enum.GetNames(type).ToList());
        }
    }
}