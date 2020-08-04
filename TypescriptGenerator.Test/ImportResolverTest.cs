using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TestObjects;
using TypescriptGenerator.Objects;
using TypescriptGenerator.Settings;

namespace TypescriptGenerator.Test
{
    [TestFixture]
    public class ImportResolverTest
    {
        private readonly string defaultFilename = "default.d.ts";

        [Test]
        public void ResolveForFileReturnsExpectedImports()
        {
            var namespaces = CreateTestNamespaces();
            var namespaceSettings = CreateNamespaceSettings();
            var files = namespaces.GroupBy(x => x.OutputFilename).ToDictionary(g => g.Key, g => g.ToList());
            var modelsNamespace = namespaces.Take(1).ToList();
            Assume.That(modelsNamespace[0].TranslatedName, Is.EqualTo("Models"));
            var sut = new ImportResolver(files, namespaceSettings, defaultFilename);

            var actual = sut.ResolveForFile(modelsNamespace, "models.d.ts").ToList();

            Assert.That(actual.Count, Is.EqualTo(1));
            Assert.That(actual[0], Is.EqualTo("import { ViewModels } from './viewModels.d.ts'"));
        }

        private List<NamespaceSettings> CreateNamespaceSettings()
        {
            return new List<NamespaceSettings>
            {
                new NamespaceSettings("TypescriptGenerator")
                {
                    Translation = "Models",
                    Filename = "models.d.ts"
                },
                new NamespaceSettings("TestObjects")
                {
                    Translation = "ViewModels",
                    Filename = "viewModels.d.ts"
                }
            };
        }

        private static List<TypescriptNamespace> CreateTestNamespaces()
        {
            var namespaces = new List<TypescriptNamespace>
            {
                new TypescriptNamespace(
                    "Models",
                    "Models",
                    "TypescriptGenerator",
                    "TypescriptGenerator",
                    new List<string>(),
                    new List<ITypescriptObject>
                    {
                        new TypescriptInterface(
                            "TypescriptGenerator",
                            "Models",
                            "MyType",
                            new List<TypescriptProperty>(),
                            new List<Type>
                            {
                                typeof(Product)
                            },
                            new List<string>())
                    },
                    new List<TypescriptNamespace>(),
                    "models.d.ts"),
                new TypescriptNamespace(
                    "ViewModels",
                    "ViewModels",
                    "TestObjects",
                    "TestObjects",
                    new List<string>(),
                    new List<ITypescriptObject>
                    {
                        new TypescriptInterface(
                            "Testobjects",
                            "ViewModels",
                            "Product",
                            new List<TypescriptProperty>(),
                            new List<Type>(),
                            new List<string>())
                    },
                    new List<TypescriptNamespace>(),
                    "viewModels.d.ts")
            };
            return namespaces;
        }
    }
}
