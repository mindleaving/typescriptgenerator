using TestObjects;

namespace TypescriptGenerator.Test
{
    public class TypescriptGeneratorBuilderTest
    {
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
