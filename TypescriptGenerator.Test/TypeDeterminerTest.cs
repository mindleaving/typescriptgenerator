using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using TestObjects;
using TypescriptGenerator.Attributes;
using TypescriptGenerator.Converters;
using TypescriptGenerator.Settings;

namespace TypescriptGenerator.Test
{
    [TestFixture]
    public class TypeDeterminerTest
    {
        private TypescriptPropertyConverterSettings CreatePropertySettings() => new TypescriptPropertyConverterSettings
        {
            TypeConverters = { new GenericTypeConverter(x => typeof(JToken).IsAssignableFrom(x), x => "any")}
        };
        private readonly TypescriptEnumConverterSettings enumSettings = new TypescriptEnumConverterSettings();
        private readonly List<NamespaceSettings> namespaceSettings = new List<NamespaceSettings>();

        [Test]
        [TestCase(typeof(int), "number")]
        [TestCase(typeof(int?), "number | null")]
        [TestCase(typeof(double), "number")]
        [TestCase(typeof(double?), "number | null")]
        [TestCase(typeof(string), "string")]
        [TestCase(typeof(Guid), "string")]
        [TestCase(typeof(object), "any")]
        public void PrimitiveTypesAsExpected(Type input, string expected)
        {
            var sut = new TypeDeterminer(CreatePropertySettings(), enumSettings, namespaceSettings);

            var actual = sut.Determine(input);

            Assert.That(actual.FormattedType, Is.EqualTo(expected));
            Assert.That(actual.Dependencies, Is.Empty);
        }

        [Test]
        public void DictionaryAsExpected()
        {
            var input = typeof(Dictionary<int, string>);
            var sut = new TypeDeterminer(CreatePropertySettings(), enumSettings, namespaceSettings);

            var actual = sut.Determine(input);

            Assert.That(actual.FormattedType, Is.EqualTo("{ [key: number]: string }"));
            Assert.That(actual.Dependencies, Is.Empty);
        }

        [Test]
        public void CollectionAsExpected()
        {
            var input = typeof(List<string>);
            var nullableStringConverter = new GenericTypeConverter(x => x == typeof(string), x => "string | null");
            var propertySettings = CreatePropertySettings();
            propertySettings.TypeConverters.Add(nullableStringConverter);
            var sut = new TypeDeterminer(propertySettings, enumSettings, namespaceSettings);

            var actual = sut.Determine(input);

            Assert.That(actual.FormattedType, Is.EqualTo("(string | null)[]"));
            Assert.That(actual.Dependencies, Is.Empty);
        }

        [Test]
        [TestCase(typeof(JToken))]
        [TestCase(typeof(JObject))]
        [TestCase(typeof(JProperty))]
        [TestCase(typeof(JValue))]
        public void JTokensAreConvertedToAny(Type input)
        {
            var sut = new TypeDeterminer(CreatePropertySettings(), enumSettings, namespaceSettings);

            var actual = sut.Determine(input);

            Assert.That(actual.FormattedType, Is.EqualTo("any"));
            Assert.That(actual.Dependencies, Is.Empty);
        }

        [Test]
        public void GenericTypeIsTranslatedToGeneric()
        {
            var input = typeof(GenericClass<string, Product>);
            var sut = new TypeDeterminer(CreatePropertySettings(), enumSettings, namespaceSettings);

            var actual = sut.Determine(input);

            Assert.That(actual.FormattedType, Is.EqualTo("TypescriptGenerator.Test.GenericClass<string,TestObjects.Product>"));
            Assert.That(actual.Dependencies, Is.EquivalentTo(new [] { typeof(GenericClass<,>), typeof(Product) }));
        }

        private class GenericClass<T1,T2>
        {
            public T1 Item1 { get; set;  }
            public T2 Item2 { get; set; }
        }

    }
}
