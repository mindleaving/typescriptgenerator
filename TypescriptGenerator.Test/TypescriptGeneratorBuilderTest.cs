using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using TestObjects;
using TestObjects.Food;
using TestObjects.Food.SeaFood;

namespace TypescriptGenerator.Test
{
    [TestFixture]
    public class TypescriptGeneratorBuilderTest
    {
        [Test]
        [Category("Tool")]
        public void SyntaxDemo()
        {
            TypescriptGenerator.Builder
                .IncludeAllInAssemblyContainingType<Product>()
                .Exclude<ProductNamingHelper>()
                .Include<LocalType>()
                .SetOutputDirectory(@"C:\Temp")
                .EnumsIntoSeparateFile()
                .EnumModifiers("export")
                .NamespaceModifiers("export")
                .ConfigureNamespace(
                    "TestObjects",
                    options =>
                    {
                        options.Translation = "Models";
                        options.Filename = "models.d.ts";
                    })
                .ConfigureNamespace(
                    "TypescriptGenerator.Test",
                    options =>
                    {
                        options.Translation = "ViewModels";
                        options.Filename = "viewModels.d.ts";
                    })
                .Generate();
        }

        public class LocalType
        {
            public List<Product> Products { get; set; }
        }

        [Test]
        public void AllTypesInNamespaceAreAdded()
        {
            var sut = TypescriptGenerator.Builder;

            sut = sut.IncludeAllInNamespace(Assembly.GetAssembly(typeof(Product)), "TestObjects.Food");

            Assert.That(sut.IncludedTypes, Is.EquivalentTo(new[] { typeof(Apple), typeof(Milk), typeof(Fish) }));
        }
    }
}
