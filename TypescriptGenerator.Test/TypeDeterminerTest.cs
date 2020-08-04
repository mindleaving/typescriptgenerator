using System;
using System.Collections.Generic;
using NUnit.Framework;
using TestObjects;
using TypescriptGenerator.Converters;
using TypescriptGenerator.Settings;

namespace TypescriptGenerator.Test
{
    [TestFixture]
    public class TypeDeterminerTest
    {
        private readonly TypescriptPropertyConverterSettings propertySettings = new TypescriptPropertyConverterSettings();
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
            var sut = new TypeDeterminer(propertySettings, enumSettings, namespaceSettings);
            Assert.That(sut.Format(input), Is.EqualTo(expected));
        }

        [Test]
        public void DictionaryAsExpected()
        {
            var input = typeof(Dictionary<int, string>);
            var sut = new TypeDeterminer(propertySettings, enumSettings, namespaceSettings);

            Assert.That(sut.Format(input), Is.EqualTo("{ [key: number]: string }"));
        }

        [Test]
        public void GenericTypeIsTranslatedToGeneric()
        {
            var input = typeof(GenericClass<string, Product>);
            var sut = new TypeDeterminer(propertySettings, enumSettings, namespaceSettings);

            Assert.That(sut.Format(input), Is.EqualTo("TypescriptGenerator.Test.GenericClass<string,TestObjects.Product>"));
        }

        private class GenericClass<T1,T2>
        {
        }
    }
}
