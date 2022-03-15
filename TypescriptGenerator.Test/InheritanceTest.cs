using System.Collections.Generic;
using NUnit.Framework;
using TypescriptGenerator.Converters;
using TypescriptGenerator.Settings;

namespace TypescriptGenerator.Test
{
    public class InheritanceTest
    {
        private class BaseClass : IBaseInterface
        {
            public string BaseName { get; set; }
        }
        private interface IBaseInterface
        {
            string BaseName { get; set; }
        }
        private class ClassWithBaseClass : BaseClass
        {
            public string ChildName { get; set; }
        }
        private readonly TypescriptClassToInterfaceConverterSettings settings = new()
        {
            PropertySettings =
            {
                Casing = CasingType.PascalCase
            }
        };
        private readonly TypescriptEnumConverterSettings enumSettings = new();
        private readonly List<NamespaceSettings> namespaceSettings = new();

        [Test]
        public void BaseClassResultsInExtendedInterface()
        {
            var sut = new TypescriptClassToInterfaceConverter(settings, enumSettings, namespaceSettings);
            var actual = sut.Convert(typeof(ClassWithBaseClass));

            Assert.That(actual.BaseClassAndInterfaces, Is.EqualTo(new[] { typeof(BaseClass) }));
            Assert.That(actual.Properties.Count, Is.EqualTo(1));
            Assert.That(actual.Properties[0].Name, Is.EqualTo(nameof(ClassWithBaseClass.ChildName)));
        }

        private class DeepInheritanceClass : IInterface1
        {
            public string OwnProperty { get; set; }
            public string Property1 { get; set; }
            public string Property2 { get; set; }
        }
        private interface IInterface1 : IInterface2
        {
            string Property1 { get; set; }
        }
        private interface IInterface2
        {
            string Property2 { get; set; }
        }
        [Test]
        public void OnlyDirectInterfacesAreIncludedInBaseClassAndInterfacesList()
        {
            var sut = new TypescriptClassToInterfaceConverter(settings, enumSettings, namespaceSettings);
            var actual = sut.Convert(typeof(DeepInheritanceClass));

            Assert.That(actual.BaseClassAndInterfaces, Is.EqualTo(new[] { typeof(IInterface1) }));
            Assert.That(actual.Properties.Count, Is.EqualTo(1));
            Assert.That(actual.Properties[0].Name, Is.EqualTo(nameof(DeepInheritanceClass.OwnProperty)));
        }
    }
}
