using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode.Day6
{
    public class MarkerFinder
    {
        public static int FindMarkerIndex(string signal, int markerLength)
        {
            for (var i = markerLength; i < signal.Length; ++i)
            {
                var potentialMarker = signal.Substring(i - markerLength, markerLength);
                if (potentialMarker.ToHashSet().Count == markerLength)
                {
                    return i;
                }
            }
            throw new ArgumentException("Provided signal does not contain a valid marker");
        }
    }

    [TestClass]
    public class Solution
    {
        [TestMethod]
        public void PartOne()
        {
            var input = TestData.ReadLines("day6").First(); // one-liner puzzle

            var answer = MarkerFinder.FindMarkerIndex(input, 4);

            Console.WriteLine("answer: " + answer);
            Assert.AreEqual(1275, answer);
        }

        [TestMethod]
        public void PartTwo()
        {
            var input = TestData.ReadLines("day6").First(); // one-liner puzzle

            var answer = MarkerFinder.FindMarkerIndex(input, 14);

            Console.WriteLine("answer: " + answer);
            Assert.AreEqual(3605, answer);
        }
    }
}