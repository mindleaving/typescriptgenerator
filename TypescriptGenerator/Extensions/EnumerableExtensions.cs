using System;
using System.Collections.Generic;

namespace TypescriptGenerator.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TResult> RecursiveSelect<T, TResult>(
            this IEnumerable<T> enumerable,
            Func<T, IEnumerable<T>> recursivePath,
            Func<T, TResult> selector)
        {
            foreach (var item in enumerable)
            {
                yield return selector(item);
                foreach (var result in recursivePath(item).RecursiveSelect(recursivePath, selector))
                {
                    yield return result;
                }
            }
        }

        public static IEnumerable<TResult> RecursiveSelectMany<T, TResult>(
            this IEnumerable<T> enumerable,
            Func<T, IEnumerable<T>> recursivePath,
            Func<T, IEnumerable<TResult>> selector)
        {
            foreach (var item in enumerable)
            {
                foreach (var selectorItem in selector(item))
                {
                    yield return selectorItem;
                }
                foreach (var result in recursivePath(item).RecursiveSelectMany(recursivePath, selector))
                {
                    yield return result;
                }
            }
        }
    }
}
