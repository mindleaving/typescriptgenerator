using NUnit.Framework;
using TestObjects;

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
        }
    }
}
