using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode.Day1
{

    [TestClass]
    public class Solution
    {
        [TestMethod]
        public void PartOneAndTwo()
        {
            var input = TestData.ReadLines("day1");

            // group each elf's rows together
            var itemsPerElf = input.Split((x) => x.Length == 0);

            // sum the totals of each group
            var totalsPerElf = itemsPerElf.Select((items) => items.Select(x => int.Parse(x))
                                                                  .Aggregate((acc, x) => acc + x));

            // part one solution: find elf with the most calories
            var highestCalories = totalsPerElf.Max();
            Console.WriteLine("answer 1: " + highestCalories.ToString());
            Assert.AreEqual(68802, highestCalories);


            // part two solution: find top 3 elves with most calories
            var topThreeElfs = totalsPerElf.OrderByDescending(x => x)
                                             .Take(3);
            var topThreeTotal = topThreeElfs.Aggregate((acc, x) => acc + x);
            Console.WriteLine("answer 2: " + topThreeTotal);
            Assert.AreEqual(205370, topThreeTotal);
        }
    }
}