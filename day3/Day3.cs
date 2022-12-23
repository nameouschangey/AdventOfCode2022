﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode.Day3
{
    public class Rucksack
    {
        public readonly IEnumerable<char> All;
        private readonly IEnumerable<char> Left;
        private readonly IEnumerable<char> Right;

        public Rucksack(string rucksackContents)
        {
            All = rucksackContents;
            Left = rucksackContents.Substring(0, rucksackContents.Length / 2);
            Right = rucksackContents.Substring(rucksackContents.Length / 2);
        }

        public IEnumerable<char> FindCommonItemInCompartements()
        {
            var inLeft = Left.ToHashSet();

            var shared = new HashSet<char>();
            foreach(var c in Right)
            {
                if (inLeft.Contains(c))
                {
                    shared.Add(c);
                }
            }
            return shared;
        }
    }

    public class ElfGroup
    {
        private readonly IEnumerable<Rucksack> Rucksacks;

        public ElfGroup(IEnumerable<Rucksack> rucksacks)
        {
            Rucksacks = rucksacks;
        }

        public char FindCommonItem()
        { // defined in the problem, there will only be 1 item common to ALL three of them

            var candidates = Rucksacks.First().All.ToHashSet();
            foreach (var rucksack in Rucksacks)
            {
                candidates = rucksack.All.Where(x => candidates.Contains(x))
                                         .ToHashSet();
            }
            return candidates.First();
        }
    }

    public class Scoring
    {
        public static int ScorePriority(IEnumerable<char> misplacedItems)
        {
            return misplacedItems.Select(x => ScoreCharacter(x))
                                 .Aggregate((acc, x) => acc + x);
        }

        public static int ScoreCharacter(char c)
        { // assumes c is A-Z or a-z, does not check for badly formed input
            if (c > 96)
            {
                return c - 96; // a at 97, priority 1-26
            }
            else
            {
                return c - 64 + 26; // A at 65, priority 27-52
            }
        }
    }

    [TestClass]
    public class Solution
    {
        [TestMethod]
        public void PartOne()
        {
            var input = TestData.ReadLines("day3");

            // each row is a rucksack
            var rucksacks = input.Select(x => new Rucksack(x));

            // part one solution: find the items common to left/right side of each rucksack
            var totalPriority = rucksacks.Select(x => x.FindCommonItemInCompartements())
                                         .Select(items => Scoring.ScorePriority(items))
                                         .Aggregate((acc, x) => acc + x);

            Console.WriteLine("answer: " + totalPriority.ToString());
            Assert.AreEqual(8493, totalPriority);
        }

        [TestMethod]
        public void PartTwo()
        {
            var input = TestData.ReadLines("day3");

            //each row is a rucksack, every 3 rucksacks is a group of elfs
            var rucksacks = input.Select(x => new Rucksack(x));
            var elfGroups = rucksacks.Partition(3).Select(x => new ElfGroup(x));


            // part two solution: find which item each group of elfs has in common between its 3 rucksacks
            var totalPriority = elfGroups.Select(x => x.FindCommonItem())
                                         .Select(item => Scoring.ScoreCharacter(item))
                                         .Aggregate((acc, x) => acc + x);

            Console.WriteLine("answer: " + totalPriority.ToString());
            Assert.AreEqual(2552, totalPriority);
        }
    }
}