using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TestObjects;
using TypescriptGenerator.Extensions;

namespace TypescriptGenerator.Test
{
    [TestFixture]
    public class TypeExtensionsTest
    {
        [Test]
        public void ArrayIsCollection()
        {
            var actual = typeof(double[]).IsCollection(out var itemType);

            Assert.That(actual, Is.True);
            Assert.That(itemType, Is.EqualTo(typeof(double)));
        }

        [Test]
        public void ListIsCollection()
        {
            var actual = typeof(List<string>).IsCollection(out var itemType);

            Assert.That(actual, Is.True);
            Assert.That(itemType, Is.EqualTo(typeof(string)));
        }

        [Test]
        public void EnumerableIsCollection()
        {
            var actual = typeof(IEnumerable<int>).IsCollection(out var itemType);

            Assert.That(actual, Is.True);
            Assert.That(itemType, Is.EqualTo(typeof(int)));
        }

        [Test]
        public void TypelessCollectionIsCollection()
        {
            var actual = typeof(ICollection).IsCollection(out var itemType);

            Assert.That(actual, Is.True);
            Assert.That(itemType, Is.EqualTo(typeof(object)));
        }

        [Test]
        public void QueueIsCollection()
        {
            var actual = typeof(Queue<Product>).IsCollection(out var itemType);

            Assert.That(actual, Is.True);
            Assert.That(itemType, Is.EqualTo(typeof(Product)));
        }

        [Test]
        public void ProductIsNotCollection()
        {
            var actual = typeof(Product).IsCollection(out _);

            Assert.That(actual, Is.False);
        }
    }
}
