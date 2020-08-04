using System.Collections.Generic;
using Commons.Physics;
using Newtonsoft.Json;
using NUnit.Framework;
using TypescriptGenerator.Converters;
using TypescriptGenerator.Settings;
using TypescriptGenerator.Test.CustomTypeConverters;

namespace TypescriptGenerator.Test
{
    [TestFixture]
    public class TypescriptClassToInterfaceConverterTest
    {
        private readonly TypescriptEnumConverterSettings enumSettings = new TypescriptEnumConverterSettings();
        private readonly List<NamespaceSettings> namespaceSettings = new List<NamespaceSettings>();

        [Test]
        public void AllPublicPropertiesAreTransferred()
        {
            var settings = new TypescriptClassToInterfaceConverterSettings
            {
                PropertySettings =
                {
                    Casing = CasingType.CamelCase
                }
            };
            var sut = new TypescriptClassToInterfaceConverter(settings, enumSettings, namespaceSettings);

            var actual = sut.Convert(typeof(TestClass1));

            Assert.That(actual.Properties.Count, Is.EqualTo(3));
            Assert.That(actual.Properties.Exists(x => x.Name == "title"));
            Assert.That(actual.Properties.Exists(x => x.Name == "number"));
            Assert.That(actual.Properties.Exists(x => x.Name == "longName"));
        }

        [Test]
        public void NewtonsoftJsonIgnoreAndJsonPropertyAttributesAreUsed()
        {
            var settings = new TypescriptClassToInterfaceConverterSettings
            {
                PropertySettings =
                {
                    Casing = CasingType.CamelCase
                }
            };
            var sut = new TypescriptClassToInterfaceConverter(settings, enumSettings, namespaceSettings);

            var actual = sut.Convert(typeof(TestClass2));

            Assert.That(actual.Properties.Count, Is.EqualTo(2));
            Assert.That(actual.Properties.Exists(x => x.Name == "title"));
            Assert.That(!actual.Properties.Exists(x => x.Name == "number"));
            Assert.That(actual.Properties.Exists(x => x.Name == "customName"));
        }

        [Test]
        public void AllPublicPropertiesAreTransferredForDerivedClass()
        {
            var settings = new TypescriptClassToInterfaceConverterSettings
            {
                PropertySettings =
                {
                    Casing = CasingType.CamelCase
                }
            };
            var sut = new TypescriptClassToInterfaceConverter(settings, enumSettings, namespaceSettings);

            var actual = sut.Convert(typeof(DerivedClass));

            Assert.That(actual.Properties.Count, Is.EqualTo(4));
            Assert.That(actual.Properties.Exists(x => x.Name == "title"));
            Assert.That(actual.Properties.Exists(x => x.Name == "number"));
            Assert.That(actual.Properties.Exists(x => x.Name == "longName"));
            Assert.That(actual.Properties.Exists(x => x.Name == "version"));
        }

        [Test]
        public void CustomTypeConverterIsApplied()
        {
            var settings = new TypescriptClassToInterfaceConverterSettings
            {
                PropertySettings =
                {
                    Casing = CasingType.CamelCase
                }
            };
            settings.PropertySettings.TypeConverters.Add(new UnitValueConverter());
            var sut = new TypescriptClassToInterfaceConverter(settings, enumSettings, namespaceSettings);

            var actual = sut.Convert(typeof(Bag));

            Assert.That(actual.Properties.Count, Is.EqualTo(2));
            Assert.That(actual.Properties.Exists(x => x.Name == "volume"));
            var volumeProperty = actual.Properties.Find(x => x.Name == "volume");
            Assert.That(volumeProperty.Type, Is.EqualTo("math.Unit"));
        }

        private class TestClass1
        {
            public string Title { get; }
            public int Number { get; }
            public int LongName { get; }
        }

        private class TestClass2
        {
            public string Title { get; }
            [JsonIgnore]
            public int Number { get; }
            [JsonProperty("customName")]
            public int LongName { get; }
        }

        private class DerivedClass : TestClass1
        {
            public string Version { get; }
        }

        private class Bag
        {
            public string Id { get; }
            public UnitValue Volume { get; }
        }
    }
}