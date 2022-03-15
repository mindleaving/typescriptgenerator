using System.Collections.Generic;
using System.Linq;
using Commons.Mathematics;
using Commons.Physics;
using Newtonsoft.Json;
using NUnit.Framework;
using TestObjects;
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
        public void GenericTypeIsConvertedToGenericInterface()
        {
            var settings = new TypescriptClassToInterfaceConverterSettings
            {
                PropertySettings =
                {
                    Casing = CasingType.CamelCase
                }
            };
            var sut = new TypescriptClassToInterfaceConverter(settings, enumSettings, namespaceSettings);

            var actual = sut.Convert(typeof(Range<>));

            Assert.That(actual.Properties.Count, Is.EqualTo(2));
            Assert.That(actual.Name, Is.EqualTo("Range<T>"));
            Assert.That(actual.Properties.Exists(x => x.Name == "from"));
            Assert.That(actual.Properties.Exists(x => x.Name == "to"));
            Assert.That(actual.Properties.All(x => x.FormattedType == "T"));
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
            Assert.That(volumeProperty.FormattedType, Is.EqualTo("math.Unit"));
        }

        [Test]
        public void InterfaceFromGeneric()
        {
            var sut = new TypescriptClassToInterfaceConverter(new TypescriptClassToInterfaceConverterSettings(), enumSettings, namespaceSettings);

            var actual = sut.Convert(typeof(GenericClass<string,Product>));

            Assert.That(actual.Properties.Find(x => x.Name == "item1").FormattedType, Is.EqualTo("T1"));
            Assert.That(actual.Properties.Find(x => x.Name == "item2").FormattedType, Is.EqualTo("T2"));
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

        private class GenericClass<T1,T2>
        {
            public T1 Item1 { get; }
            public T2 Item2 { get; }
        }
    }
}