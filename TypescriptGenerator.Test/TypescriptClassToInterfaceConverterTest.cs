using Newtonsoft.Json;
using NUnit.Framework;

namespace TypescriptGenerator.Test
{
    [TestFixture]
    public class TypescriptClassToInterfaceConverterTest
    {
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
            var sut = new TypescriptClassToInterfaceConverter(settings);

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
            var sut = new TypescriptClassToInterfaceConverter(settings);

            var actual = sut.Convert(typeof(TestClass2));

            Assert.That(actual.Properties.Count, Is.EqualTo(2));
            Assert.That(actual.Properties.Exists(x => x.Name == "title"));
            Assert.That(!actual.Properties.Exists(x => x.Name == "number"));
            Assert.That(actual.Properties.Exists(x => x.Name == "customName"));
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
    }
}