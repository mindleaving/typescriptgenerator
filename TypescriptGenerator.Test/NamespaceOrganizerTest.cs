using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TypescriptGenerator.Objects;
using TypescriptGenerator.Settings;

namespace TypescriptGenerator.Test
{
    [TestFixture]
    public class NamespaceOrganizerTest
    {
        [Test]
        public void NestedNamespacesAreExtracted()
        {
            var settings = new NamespaceOrganizerSettings();
            var sut = new NamespaceOrganizer(settings);
            var types = new List<ITypescriptObject>
            {
                CreateInterface("TypescriptGenerator", "TypescriptGenerator"),
                CreateInterface("TypescriptGenerator.Test", "TypeChecker"),
                CreateInterface("TypescriptGenerator.Test", "Constants"),
                CreateInterface("TestObjects", "Product")
            };

            var namespaces = sut.Organize(types);

            Assert.That(namespaces.Count, Is.EqualTo(2));
            Assert.That(namespaces, Has.One.Matches<TypescriptNamespace>(x => x.TranslatedName == "TypescriptGenerator"));
            var typescriptGeneratorNamespace = namespaces.FirstOrDefault(x => x.TranslatedName == "TypescriptGenerator");
            Assert.That(typescriptGeneratorNamespace.Types.Count, Is.EqualTo(1));
            Assert.That(typescriptGeneratorNamespace.SubNamespaces.Count, Is.EqualTo(1));
            Assert.That(typescriptGeneratorNamespace.SubNamespaces[0].TranslatedName, Is.EqualTo("Test"));
            var typescriptGeneratorTestNamespace = typescriptGeneratorNamespace.SubNamespaces[0];
            Assert.That(typescriptGeneratorTestNamespace.Types.Count, Is.EqualTo(2));
            Assert.That(namespaces, Has.One.Matches<TypescriptNamespace>(x => x.TranslatedName == "TestObjects"));
        }

        private static TypescriptInterface CreateInterface(string ns, string name)
        {
            return new TypescriptInterface(
                ns,
                ns,
                name,
                new List<TypescriptProperty>(),
                new List<Type>(),
                new List<string>());
        }
    }
}
