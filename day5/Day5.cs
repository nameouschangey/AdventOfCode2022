using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode.Day5
{
    public class Warehouse
    {
        public List<Stack<char>> Stacks = new List<Stack<char>>();

        public void MoveSingle(InputInstruction instruction)
        { // moves one crate at a time
            for (var i = 0; i < instruction.Count; ++i)
            {
                Stacks[instruction.Dest].Push(Stacks[instruction.Source].Pop());
            }
        }

        public void MoveMultiple(InputInstruction instruction)
        { // picks up and deposits all crates together
            var cratesToMove = new Stack<char>();
            
            for (var i = 0; i < instruction.Count; ++i)
            {
                cratesToMove.Push(Stacks[instruction.Source].Pop());
            }

            while (cratesToMove.TryPop(out var crate))
            {
                Stacks[instruction.Dest].Push(crate);
            }
        }

        public IEnumerable<char> CratesOnTop()
        { // unclear what puzzle expects if any stack is empty
            return Stacks.Select(x => x.TryPeek(out var c).Then(x => x ? c : ' '));
        }
    }

    public struct InputInstruction
    {
        public int Count;
        public int Source;
        public int Dest;
    }

    public class DecodedInput
    {
        public Warehouse Warehouse { get; private set; } = new Warehouse();
        public IEnumerable<InputInstruction> Instructions { get; private set; }

        public DecodedInput(IEnumerable<string> rows)
        {
            var warehouseInstructionsSplit = rows.Split((row) => row.Length == 0);

            var warehouseRows = warehouseInstructionsSplit.First().SkipLast(1);

            // create the right number of stacks for the warehouse
            var warehouseStackNames = warehouseInstructionsSplit.First().Last();
            var numWarehouseStacks = warehouseStackNames.Split(' ', StringSplitOptions.RemoveEmptyEntries).Count();

            for(int i = 0; i < numWarehouseStacks; ++i)
            {
                Warehouse.Stacks.Add(new Stack<char>());
            }

            // add the existing crates to those stacks, starting at the ground level
            foreach (var row in warehouseRows.Reverse())
            {
                for (int i = 0; i < numWarehouseStacks; ++i)
                {
                    // crates input is |[a] [b] [c]|
                    // so (4*i) + 1
                    var item = row[(4 * i) + 1];
                    if (item != ' ')
                    {
                        Warehouse.Stacks[i].Push(item);
                    }
                }
            }

            // parse instructions
            var instructionRows = warehouseInstructionsSplit.ElementAt(1);
            Instructions = instructionRows.Select(x =>
            {
                var splits = x.Split(' ');
                return new InputInstruction()
                {
                    Count = int.Parse(splits[1]),
                    Source = int.Parse(splits[3])-1,
                    Dest = int.Parse(splits[5])-1
                };
            });
        }
    }

    [TestClass]
    public class Solution
    {
        [TestMethod]
        public void PartOne()
        {
            var puzzle = new DecodedInput(TestData.ReadLines("day5"));

            // part one solution: follow given instructions
            foreach (var instruction in puzzle.Instructions)
            {
                puzzle.Warehouse.MoveSingle(instruction);
            }

            var answer = string.Concat(puzzle.Warehouse.CratesOnTop());
            Console.WriteLine("answer: " + answer);
            Assert.AreEqual("MQTPGLLDN", answer);
        }

        [TestMethod]
        public void PartTwo()
        {
            var puzzle = new DecodedInput(TestData.ReadLines("day5"));

            // part one solution: follow given instructions
            foreach (var instruction in puzzle.Instructions)
            {
                puzzle.Warehouse.MoveMultiple(instruction);
            }

            var answer = string.Concat(puzzle.Warehouse.CratesOnTop());
            Console.WriteLine("answer: " + answer);
            Assert.AreEqual("LVZPSTTCZ", answer);
        }
    }
}