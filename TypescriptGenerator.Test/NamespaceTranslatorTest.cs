using System.Collections.Generic;
using NUnit.Framework;
using TypescriptGenerator.Settings;

namespace TypescriptGenerator.Test
{
    [TestFixture]
    public class NamespaceTranslatorTest
    {
        [Test]
        public void TranslateReturnsNullForNull()
        {
            var actual = NamespaceTranslator.Translate(null, new List<NamespaceSettings>());
            Assert.That(actual, Is.Null);
        }

        [Test]
        public void TranslateReturnsInputIfNoSettings()
        {
            var input = "MyNamespace.Test";
            var actual = NamespaceTranslator.Translate(input, new List<NamespaceSettings>());
            Assert.That(actual, Is.EqualTo(input));
        }

        [Test]
        public void TranslateReturnsTranslatedForExactMatch()
        {
            var input = "MyNamespace.Test";
            var translation = "Models";
            var settings = new NamespaceSettings(input)
            {
                Translation = translation
            };
            var actual = NamespaceTranslator.Translate(input, new List<NamespaceSettings> { settings });
            Assert.That(actual, Is.EqualTo(translation));
        }

        [Test]
        public void TranslateReplacesFirstPartOfNamespace()
        {
            var input = "MyNamespace.Test";
            var translation = "Models";
            var settings = new NamespaceSettings("MyNamespace")
            {
                Translation = translation
            };
            var actual = NamespaceTranslator.Translate(input, new List<NamespaceSettings> { settings });
            Assert.That(actual, Is.EqualTo("Models.Test"));
        }

        [Test]
        public void TranslateOnlyReplacesLeadingPart()
        {
            var input = "MyNamespace.MyNamespace.Test";
            var translation = "Models";
            var settings = new NamespaceSettings("MyNamespace")
            {
                Translation = translation
            };
            var actual = NamespaceTranslator.Translate(input, new List<NamespaceSettings> { settings });
            Assert.That(actual, Is.EqualTo("Models.MyNamespace.Test"));
        }
    }
}
