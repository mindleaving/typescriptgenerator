using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using TypescriptGenerator.Test.EndToEndTestTypes;
using TypescriptGenerator.Test.EndToEndTestTypes.Health;

namespace TypescriptGenerator.Test
{
    public class EndToEndTest
    {
        [Test]
		[Category("EndToEnd")]
        public void Run()
        {
            var outputDirectory = @"C:\Temp\TypescriptGeneratorTest";
            if(!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);
            TypescriptGenerator.Builder
                .IncludeAllInNamespace(Assembly.GetAssembly(typeof(Person)), "TypescriptGenerator.Test.EndToEndTestTypes")
                .ReactDefaults()
                .ConfigureNamespace("TypescriptGenerator.Test.EndToEndTestTypes",
                    options =>
                    {
                        options.Translation = "Models";
                    })
                .SetOutputDirectory(outputDirectory)
                .Generate();

            var fileContents = string.Join("\n", Directory.EnumerateFiles(outputDirectory).Select(File.ReadAllText));
            Assert.That(fileContents, Does.Not.Contain(nameof(InternalPerson)));
            Assert.That(fileContents, Contains.Substring("interface Person extends Models.IId"));
        }
    }
}
