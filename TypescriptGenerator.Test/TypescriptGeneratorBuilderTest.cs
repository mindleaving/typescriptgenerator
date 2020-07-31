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
                .ConfigureNamespace(
                    "TestObjects",
                    options =>
                    {
                        options.Translation = "Models";
                        options.Filename = "models.d.ts";
                        options.Modifiers.Add("export");
                    })
                .ConfigureNamespace(
                    "TypescriptGenerator.Test",
                    options =>
                    {
                        options.Translation = "ViewModels";
                        options.Filename = "viewModels.d.ts";
                        options.Modifiers.Add("export");
                    })
                .Generate();
        }

        public class LocalType
        {
        }
    }
}
