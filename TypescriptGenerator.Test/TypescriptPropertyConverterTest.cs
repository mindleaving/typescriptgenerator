using System.Collections.Generic;
using NUnit.Framework;
using TypescriptGenerator.Attributes;
using TypescriptGenerator.Converters;
using TypescriptGenerator.Settings;

namespace TypescriptGenerator.Test
{
    [TestFixture]
    public class TypescriptPropertyConverterTest
    {
        private readonly TypescriptPropertyConverterSettings settings = new TypescriptPropertyConverterSettings();
        private readonly TypescriptEnumConverterSettings enumSettings = new TypescriptEnumConverterSettings();
        private readonly List<NamespaceSettings> namespaceSettings = new List<NamespaceSettings>();

        [Test]
        public void TypescriptTypeAttributesIsUsed()
        {
            var nullableString = typeof(AttributeAnnotatedClass).GetProperty("NullableString");
            var sut = new TypescriptPropertyConverter(settings, enumSettings, namespaceSettings);

            var actual = sut.Convert(nullableString);

            Assert.That(actual.FormattedType, Is.EqualTo("string | null"));
        }

        [Test]
        public void TypescriptIsOptionalAttributesIsUsed()
        {
            var optionalString = typeof(AttributeAnnotatedClass).GetProperty("OptionalString");
            var sut = new TypescriptPropertyConverter(settings, enumSettings, namespaceSettings);

            var actual = sut.Convert(optionalString);

            Assert.That(actual.IsOptional, Is.True);
            Assert.That(actual.FormattedType, Is.EqualTo("string"));
        }

        private class AttributeAnnotatedClass
        {
            [TypescriptType("string | null")]
            public string NullableString { get; }

            [TypescriptIsOptional]
            public string OptionalString { get; }
        }
    }
}
