using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode.Day4
{
    public class CleaningPair
    {
        public CleaningRange Left;
        public CleaningRange Right;

        public CleaningPair(CleaningRange left, CleaningRange right)
        {
            Left = left;
            Right = right;
        }

        public bool IsFullyOverlappingPair()
        { // whether Left or Right fully contains the other side of the pair range
            return  (Left.Start <= Right.Start && Left.End >= Right.End)
                    || (Right.Start <= Left.Start && Right.End >= Left.End);
        }

        public bool IsOverlappedPair()
        { // whether Left or Right fully contains the other side of the pair range
            return (Left.Start >= Right.Start && Left.Start <= Right.End)
                || (Left.End >= Right.Start && Left.End <= Right.End)
                || IsFullyOverlappingPair();
        }
    }

    public class CleaningRange
    {
        public int Start;
        public int End;

        public CleaningRange(int start, int end)
        {
            Start = start;
            End = end;
        }
    }

    public class RowDecoder
    {
        public static CleaningPair CleaningPairFromRow(string row)
        {
            // e.g. 10-12,15-999 => [10->12] & [15->999]
            var pairs = row.Split(',');
            var elements = pairs.SelectMany(pair => pair.Split('-'))
                                .Select(x => int.Parse(x));

            return new CleaningPair(new CleaningRange(elements.ElementAt(0), elements.ElementAt(1)),
                                    new CleaningRange(elements.ElementAt(2), elements.ElementAt(3)));
        }
    }

    [TestClass]
    public class Solution
    {
        [TestMethod]
        public void PartOne()
        {
            var input = TestData.ReadLines("day4");

            // each row is a rucksack
            var cleaningPairs = input.Select(x => RowDecoder.CleaningPairFromRow(x));

            // part one solution: find the fully overlapping pairs that can be removed
            var redundantPairs = cleaningPairs.Where(x => x.IsFullyOverlappingPair());
            var numRedundantPairs = redundantPairs.Count();

            Console.WriteLine("answer: " + numRedundantPairs.ToString());
            Assert.AreEqual(532, numRedundantPairs);
        }

        [TestMethod]
        public void PartTwo()
        {
            var input = TestData.ReadLines("day4");

            // each row is a rucksack
            var cleaningPairs = input.Select(x => RowDecoder.CleaningPairFromRow(x));

            // part one solution: find the semi-overlapping pairs that can be removed
            var redundantPairs = cleaningPairs.Where(x => x.IsOverlappedPair());
            var numRedundantPairs = redundantPairs.Count();

            Console.WriteLine("answer: " + numRedundantPairs.ToString());
            Assert.AreEqual(854, numRedundantPairs);
        }
    }
}