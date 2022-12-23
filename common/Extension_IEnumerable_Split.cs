using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Extension_IEnumerable_Split
{
    public enum SplitDirection
    {
        AttachToPrevious,
        AttachToNext,
        Discard
    }

    public static IEnumerable<string> Split(this string list, char delimitter, SplitDirection direction = SplitDirection.Discard)
    {   // note with Discard; will provide same behaviour as String.Split() keeping it compliant
        // as omitting the 2nd param will call the non-extended string.Split() method, and return a string[] which still meets the same contract
        var result = list.Split<char>(x => x == delimitter, direction);
        return result.Select(x => string.Concat(x));
    }

    public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> list, Func<T, bool> delimiter, SplitDirection direction = SplitDirection.Discard)
    {
        var currentEnumeration = new List<T>();
        foreach(var obj in list)
        {
            if (delimiter(obj))
            {
                switch (direction)
                {
                    case SplitDirection.Discard:
                        yield return currentEnumeration;
                        currentEnumeration = new List<T>();
                        break;

                    case SplitDirection.AttachToPrevious:
                        currentEnumeration.Add(obj);
                        yield return currentEnumeration;
                        currentEnumeration = new List<T>();
                        break;

                    case SplitDirection.AttachToNext:
                        yield return currentEnumeration;
                        currentEnumeration = new List<T>();
                        currentEnumeration.Add(obj);
                        break;
                }
            }
            else
            {
                currentEnumeration.Add(obj);
            }
        }

        if (currentEnumeration.Any())
        {
            yield return currentEnumeration;
        }
    }
}

namespace AdventOfCode.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Extension_IEnumerable_Split_Test
    {
        [TestMethod]
        public void Split_Discard()
        {
            // splitting on _ 
            var sequence = new List<string>() { "A", "_", "B", "C", "_", "D", "E" };
            var splits = sequence.Split((x) => x == "_");

            Assert.AreEqual(splits.Count(), 3);
            Assert.IsTrue(splits.ElementAt(0).SequenceEqual(new List<string>() { "A" }));
            Assert.IsTrue(splits.ElementAt(1).SequenceEqual(new List<string>() { "B", "C" }));
            Assert.IsTrue(splits.ElementAt(2).SequenceEqual(new List<string>() { "D", "E" }));
        }

        [TestMethod]
        public void Split_AttachPrevious()
        {
            // splitting on _ 
            var sequence = new List<string>() { "A", "_", "B", "C", "_", "D", "E" };
            var splits = sequence.Split((x) => x == "_", Extension_IEnumerable_Split.SplitDirection.AttachToPrevious);

            Assert.AreEqual(splits.Count(), 3);
            Assert.IsTrue(splits.ElementAt(0).SequenceEqual(new List<string>() { "A", "_" }));
            Assert.IsTrue(splits.ElementAt(1).SequenceEqual(new List<string>() { "B", "C", "_"}));
            Assert.IsTrue(splits.ElementAt(2).SequenceEqual(new List<string>() { "D", "E" }));
        }

        [TestMethod]
        public void Split_AttachNext()
        {
            // splitting on _ 
            var sequence = new List<string>() { "A", "_", "B", "C", "_", "D", "E" };
            var splits = sequence.Split((x) => x == "_", Extension_IEnumerable_Split.SplitDirection.AttachToNext);

            Assert.AreEqual(splits.Count(), 3);
            Assert.IsTrue(splits.ElementAt(0).SequenceEqual(new List<string>() { "A" }));
            Assert.IsTrue(splits.ElementAt(1).SequenceEqual(new List<string>() { "_", "B", "C" }));
            Assert.IsTrue(splits.ElementAt(2).SequenceEqual(new List<string>() { "_", "D", "E" }));
        }

        [TestMethod]
        public void Split_String_AttachPrevious()
        {
            // splitting on _ 
            var sequence = "A_BC_DE";
            var splits = sequence.Split('_', Extension_IEnumerable_Split.SplitDirection.AttachToNext);

            Assert.AreEqual(splits.Count(), 3);
            Assert.IsTrue(splits.ElementAt(0).SequenceEqual(new List<char>() { 'A' }));
            Assert.IsTrue(splits.ElementAt(1).SequenceEqual(new List<char>() { '_', 'B', 'C' }));
            Assert.IsTrue(splits.ElementAt(2).SequenceEqual(new List<char>() { '_', 'D', 'E' }));

            Assert.AreEqual(splits.ElementAt(0), "A");
            Assert.AreEqual(splits.ElementAt(1), "_BC");
            Assert.AreEqual(splits.ElementAt(2), "_DE");
        }
    }
}