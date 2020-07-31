using NUnit.Framework;

namespace TypescriptGenerator.Test
{
    [TestFixture]
    public class PropertyNameSplitterTest
    {
        [Test]
        public void CanSplitUnderscoreSeparated()
        {
            var input = "My_Cool_variable";
            var actual = PropertyNameSplitter.Split(input);
            Assert.That(actual.Count, Is.EqualTo(3));
            Assert.That(actual[0], Is.EqualTo("my"));
            Assert.That(actual[1], Is.EqualTo("cool"));
            Assert.That(actual[2], Is.EqualTo("variable"));
        }

        [Test]
        public void CanSplitCamelCase()
        {
            var input = "MyCoolVariable";
            var actual = PropertyNameSplitter.Split(input);
            Assert.That(actual.Count, Is.EqualTo(3));
            Assert.That(actual[0], Is.EqualTo("my"));
            Assert.That(actual[1], Is.EqualTo("cool"));
            Assert.That(actual[2], Is.EqualTo("variable"));
        }

        [Test]
        public void CanSplitMixedUnderscoreCamelCase()
        {
            var input = "__myCool_Variable";
            var actual = PropertyNameSplitter.Split(input);
            Assert.That(actual.Count, Is.EqualTo(3));
            Assert.That(actual[0], Is.EqualTo("my"));
            Assert.That(actual[1], Is.EqualTo("cool"));
            Assert.That(actual[2], Is.EqualTo("variable"));
        }
    }
}
