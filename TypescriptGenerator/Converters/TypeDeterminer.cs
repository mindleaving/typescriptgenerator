using System;
using System.Collections.Generic;
using System.Linq;
using TypescriptGenerator.Extensions;
using TypescriptGenerator.Settings;

namespace TypescriptGenerator.Converters
{
    public class TypeDeterminerResult
    {
        public TypeDeterminerResult(
            string formattedType,
            List<Type> dependencies)
        {
            FormattedType = formattedType;
            Dependencies = dependencies;
        }

        public string FormattedType { get; }
        public List<Type> Dependencies { get; }
    }
    public class TypeDeterminer
    {
        private readonly TypescriptPropertyConverterSettings settings;
        private readonly TypescriptEnumConverterSettings enumSettings;
        private readonly List<NamespaceSettings> namespaceSettings;

        public TypeDeterminer(
            TypescriptPropertyConverterSettings settings, 
            TypescriptEnumConverterSettings enumSettings,
            List<NamespaceSettings> namespaceSettings)
        {
            this.settings = settings;
            this.enumSettings = enumSettings;
            this.namespaceSettings = namespaceSettings;
        }

        private static readonly Dictionary<Type, string> PrimitiveTypes = new Dictionary<Type, string>
        {
            { typeof(string), "string"},
            { typeof(void), "void"},
            { typeof(bool), "boolean"},
            { typeof(short), "number"},
            { typeof(ushort), "number"},
            { typeof(int), "number"},
            { typeof(uint), "number"},
            { typeof(long), "number"},
            { typeof(ulong), "number"},
            { typeof(double), "number"},
            { typeof(decimal), "number"},
            { typeof(Guid), "string"},
            { typeof(object), "any"},
            { typeof(DateTime), "Date"},
            { typeof(DateTimeOffset), "Date"},
            { typeof(TimeSpan), "string"}
        };

        public static bool IsPrimitiveType(Type type)
        {
            return PrimitiveTypes.ContainsKey(type);
        }

        public TypeDeterminerResult Determine(Type propertyType)
        {
            var matchingTypeConverter = settings.TypeConverters.FirstOrDefault(x => x.IsMatchingType(propertyType));
            if (matchingTypeConverter != null)
                return new TypeDeterminerResult(matchingTypeConverter.Convert(propertyType), new List<Type>());
            if(propertyType.IsGenericParameter)
                return new TypeDeterminerResult(propertyType.Name, new List<Type>());
            if (IsPrimitiveType(propertyType))
                return new TypeDeterminerResult(PrimitiveTypes[propertyType], new List<Type>());
            if (propertyType.IsNullable())
            {
                var underlyingType = Nullable.GetUnderlyingType(propertyType);
                var underlyingTypeResult = Determine(underlyingType);
                return new TypeDeterminerResult(
                    $"{underlyingTypeResult.FormattedType} | null",
                    underlyingTypeResult.Dependencies);
            }
            if (propertyType.IsEnum && enumSettings.EnumsIntoSeparateFile)
            {
                return new TypeDeterminerResult("Enums." + propertyType.Name, new List<Type> { propertyType });
            }

            if (propertyType.IsDictionary(out var keyType, out var valueType))
            {
                var keyTypeResult = Determine(keyType);
                var valueTypeResult = Determine(valueType);
                return new TypeDeterminerResult(
                    $"{{ [key: {keyTypeResult.FormattedType}]: {valueTypeResult.FormattedType} }}",
                    keyTypeResult.Dependencies.Concat(valueTypeResult.Dependencies).ToList()
                );
            }
            if (propertyType.IsCollection(out var itemType))
            {
                var itemTypeResult = Determine(itemType);
                return new TypeDeterminerResult(
                    $"{itemTypeResult.FormattedType}[]",
                    itemTypeResult.Dependencies
                );
            }
            var translatedNamespace = NamespaceTranslator.Translate(propertyType.Namespace, namespaceSettings);
            if (propertyType.IsGenericType)
            {
                var genericResults = propertyType.GetGenericArguments()
                    .Select(Determine)
                    .ToList();
                var generics = $"<{string.Join(",", genericResults.Select(x => x.FormattedType))}>";
                return new TypeDeterminerResult(
                    $"{translatedNamespace}.{propertyType.Name.StripGenericTypeSuffix()}{generics}",
                    genericResults.SelectMany(x => x.Dependencies).Concat(new []{ propertyType.GetGenericTypeDefinition() }).ToList()
                );
            }
            return new TypeDeterminerResult(
                $"{translatedNamespace}.{propertyType.Name}",
                new List<Type> { propertyType}
            );
        }
    }
}
