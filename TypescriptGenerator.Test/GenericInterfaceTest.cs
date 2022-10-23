using System.Linq;
using System.Reflection;
using NUnit.Framework;
using TypescriptGenerator.Objects;

namespace TypescriptGenerator.Test
{
    public class GenericInterfaceTest
    {
        [Test]
        public void OnlyGenericInterfaceIsIncluded()
        {
            var generator = new TypescriptGenerator();
            generator.IncludeAllInNamespace(Assembly.GetAssembly(typeof(GenericInterfaceTest)), "TypescriptGenerator.Test.GenericInterfaceTestTypes");
            var collectedTypes = generator.CollectTypes();
            var collectedInterfaces = collectedTypes.OfType<TypescriptInterface>().ToList();
            Assert.That(collectedInterfaces.Any(x => x.Name == "IGenericInterface<T>"));
            Assert.That(!collectedInterfaces.Any(x => x.Name == "IGenericInterface<Address>"));
            Assert.That(collectedInterfaces.Any(x => x.Name == "IExternalGenericInterface<S>"));
            Assert.That(!collectedInterfaces.Any(x => x.Name == "IExternalGenericInterface<Person>"));
        }

        
    }
}
