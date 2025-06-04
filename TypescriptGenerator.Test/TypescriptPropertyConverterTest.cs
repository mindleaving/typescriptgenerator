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
        private readonly TypescriptPropertyConverterSettings settings = new();
        private readonly TypescriptEnumConverterSettings enumSettings = new();
        private readonly List<NamespaceSettings> namespaceSettings = new();

        [Test]
        public void TypescriptTypeAttributesIsUsed()
        {
            var typeAnnotatedString = typeof(AttributeAnnotatedClass).GetProperty(nameof(AttributeAnnotatedClass.TypeAnnotatedString));
            var sut = new TypescriptPropertyConverter(settings, enumSettings, namespaceSettings);

            var actual = sut.Convert(typeAnnotatedString);

            Assert.That(actual.IsOptional, Is.False);
            Assert.That(actual.FormattedType, Is.EqualTo("string | null"));
        }

        [Test]
        public void TypescriptIsOptionalAttributesIsUsed()
        {
            var optionalString = typeof(AttributeAnnotatedClass).GetProperty(nameof(AttributeAnnotatedClass.OptionalString));
            var sut = new TypescriptPropertyConverter(settings, enumSettings, namespaceSettings);

            var actual = sut.Convert(optionalString);

            Assert.That(actual.IsOptional, Is.True);
            Assert.That(actual.FormattedType, Is.EqualTo("string"));
        }

        [Test]
        public void NullableTypeIsMarkedAsOptional()
        {
            // Part 1: String (reference type)
            var nullableString = typeof(AttributeAnnotatedClass).GetProperty(nameof(AttributeAnnotatedClass.NullableString));
            var sut = new TypescriptPropertyConverter(settings, enumSettings, namespaceSettings);

            var actual = sut.Convert(nullableString);

            Assert.That(actual.IsOptional, Is.True);
            Assert.That(actual.FormattedType, Is.EqualTo("string"));

            // Part 1: Integer (value type)
            var nullableNumber = typeof(AttributeAnnotatedClass).GetProperty(nameof(AttributeAnnotatedClass.NullableNumber));

            actual = sut.Convert(nullableNumber);

            Assert.That(actual.IsOptional, Is.True);
            Assert.That(actual.FormattedType, Is.EqualTo("number | null"));
        }

        private class AttributeAnnotatedClass
        {
            [TypescriptType("string | null")]
            public string TypeAnnotatedString { get; set; }

            [TypescriptIsOptional]
            public string OptionalString { get; set; }

            public string? NullableString { get; set; }
            public int? NullableNumber { get; set; }
        }
    }
}
