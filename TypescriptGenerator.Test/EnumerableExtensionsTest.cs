using System.Collections.Generic;
using NUnit.Framework;
using TypescriptGenerator.Extensions;

namespace TypescriptGenerator.Test
{
    [TestFixture]
    public class EnumerableExtensionsTest
    {
        [Test]
        public void RecursiveSelectReturnsNoItemForEmptyInput()
        {
            var input = new List<RecursiveClass>();
            var actual = input.RecursiveSelect(x => x.Items, x => x.Name);
            Assert.That(actual, Is.Empty);
        }

        [Test]
        public void RecursiveSelectReturnsAllItems()
        {
            var input = new List<RecursiveClass>
            {
                new RecursiveClass("A")
                {
                    Items =
                    {
                        new RecursiveClass("AA"),
                        new RecursiveClass("AB"),
                        new RecursiveClass("AC")
                        {
                            Items =
                            {
                                new RecursiveClass("ACA")
                            }
                        }
                    }
                },
                new RecursiveClass("B")
                {
                    Items =
                    {
                        new RecursiveClass("BA")
                    }
                }
            };
            var actual = input.RecursiveSelect(x => x.Items, x => x.Name);
            Assert.That(actual, Is.EquivalentTo(new[] { "A", "AA", "AB", "AC", "ACA", "B", "BA" }));
        }

        [Test]
        public void RecursiveSelectManyReturnsNoItemForEmptyInput()
        {
            var input = new List<RecursiveClass>();
            var actual = input.RecursiveSelectMany(x => x.Items, x => x.Categories);
            Assert.That(actual, Is.Empty);
        }

        [Test]
        public void RecursiveSelectManyReturnsAllItems()
        {
            var input = new List<RecursiveClass>
            {
                new RecursiveClass("A")
                {
                    Items =
                    {
                        new RecursiveClass("AA"),
                        new RecursiveClass("AB")
                        {
                            Categories = { "Cat3" }
                        },
                        new RecursiveClass("AC")
                        {
                            Items =
                            {
                                new RecursiveClass("ACA")
                                {
                                    Categories = { "Cat4" }
                                }
                            }
                        }
                    },
                    Categories = { "Cat1", "Cat2"}
                },
                new RecursiveClass("B")
                {
                    Items =
                    {
                        new RecursiveClass("BA")
                        {
                            Categories = { "Cat6"}
                        }
                    },
                    Categories = { "Cat5" }
                }
            };
            var actual = input.RecursiveSelectMany(x => x.Items, x => x.Categories);
            Assert.That(actual, Is.EquivalentTo(new[] { "Cat1", "Cat2", "Cat3", "Cat4", "Cat5", "Cat6" }));
        }

        private class RecursiveClass
        {
            public RecursiveClass(string name)
            {
                Name = name;
            }

            public string Name { get; }
            public List<RecursiveClass> Items { get; } = new List<RecursiveClass>();
            public List<string> Categories { get; } = new List<string>();
        }
    }
}
