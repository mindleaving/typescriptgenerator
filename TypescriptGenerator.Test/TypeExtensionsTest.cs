using System;
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

        [Test]
        public void IsDictionaryReturnsCorrectKeyValueTypes()
        {
            var isDictionary = typeof(Dictionary<int, Guid>).IsDictionary(out var keyType, out var valueType);

            Assert.That(isDictionary, Is.True);
            Assert.That(keyType, Is.EqualTo(typeof(int)));
            Assert.That(valueType, Is.EqualTo(typeof(Guid)));
        }

        [Test]
        public void ProductIsNotDictionary()
        {
            var actual = typeof(Product).IsDictionary(out _, out _);

            Assert.That(actual, Is.False);
        }

        [Test]
        public void StripGenericTypeSuffixReturnsUnchangedStringIfNoGenericSuffix()
        {
            var input = "MyNonGenericClass";
            Assert.That(input.StripGenericTypeSuffix(), Is.EqualTo(input));
        }

        [Test]
        public void StripGenericTypeSuffixRemovesGenericSuffix()
        {
            var input = "MyNonGenericClass`2";
            Assert.That(input.StripGenericTypeSuffix(), Is.EqualTo("MyNonGenericClass"));
        }
    }
}
