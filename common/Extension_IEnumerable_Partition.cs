using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Extension_IEnumerable_Partition
{
    public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> list, int partitionSize)
    {
        while (list.Any())
        {
            yield return list.Take(partitionSize);
            list = list.Skip(partitionSize);
        }
    }

    public static IEnumerable<IEnumerable<T>> PartitionInto<T>(this IEnumerable<T> list, int numPartitions)
    {
        var numElements = list.Count();
        var partitionSize = numElements / numPartitions;
        var remainder = numElements % partitionSize;

        for(var i = 0; i < numPartitions; ++i)
        {
            // first n partitions will have 1 more element to cover the remainder, till the remainder is fully allocated
            var toTake = partitionSize + ((remainder-- > 0) ? 1 : 0);
            yield return list.Take(toTake);
            list = list.Skip(toTake);
        }
    }
}

namespace AdventOfCode.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Extension_IEnumerable_Partition_Test
    {
        [TestMethod]
        public void Partition()
        {
            // splitting on _ 
            var sequence = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K"};
            var partitions = sequence.Partition(3); // split 11 elements into groups of 3

            Assert.AreEqual(partitions.Count(), 4);
            Assert.IsTrue(partitions.ElementAt(0).SequenceEqual(new List<string>() { "A", "B", "C"}));
            Assert.IsTrue(partitions.ElementAt(1).SequenceEqual(new List<string>() { "D", "E", "F" }));
            Assert.IsTrue(partitions.ElementAt(2).SequenceEqual(new List<string>() { "G", "H", "I" }));
            Assert.IsTrue(partitions.ElementAt(3).SequenceEqual(new List<string>() { "J", "K" }));
        }

        [TestMethod]
        public void PartitionInto()
        {
            // splitting on _ 
            var sequence = new List<string>() { "A", "B", "C", "D", "E", "F", "G" };
            var partitions = sequence.PartitionInto(3); // splits 7 elements into 3 equal partitions

            Assert.AreEqual(partitions.Count(), 3);
            Assert.IsTrue(partitions.ElementAt(0).SequenceEqual(new List<string>() { "A", "B", "C" }));
            Assert.IsTrue(partitions.ElementAt(1).SequenceEqual(new List<string>() { "D", "E", }));
            Assert.IsTrue(partitions.ElementAt(2).SequenceEqual(new List<string>() { "F", "G" }));
        }

    }
}