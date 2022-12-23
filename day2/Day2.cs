using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode.Day2
{
    public enum RPSResultType
    {
        Lose = 0,
        Draw = 3,
        Win = 6
    }

    public enum RPSShapeType
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3
    }

    public class RPSRound
    {
        public int Score
        {
            get => (int)OurShape + (int)Result;
        }

        public readonly RPSShapeType TheirShape;
        public readonly RPSShapeType OurShape;
        public readonly RPSResultType Result;

        protected RPSRound(RPSShapeType theirShape, RPSShapeType ourShape, RPSResultType result)
        {
            TheirShape = theirShape;
            OurShape = ourShape;
            Result = result;
        }

        public static RPSRound FromShapeChoices(RPSShapeType theirShape, RPSShapeType ourShape)
        {
            RPSResultType result;

            if (theirShape == ourShape)
            {
                result = RPSResultType.Draw;
            }
            else
            {
                result = ourShape switch
                {
                    RPSShapeType.Rock =>
                        theirShape == RPSShapeType.Scissors ? RPSResultType.Win : RPSResultType.Lose,

                    RPSShapeType.Scissors =>
                        theirShape == RPSShapeType.Paper ? RPSResultType.Win : RPSResultType.Lose,

                    RPSShapeType.Paper =>
                        theirShape == RPSShapeType.Rock ? RPSResultType.Win : RPSResultType.Lose,

                    _ => throw new NotImplementedException("Not implemented logic for handling the given additional shape")
                };
            }

            return new RPSRound(theirShape, ourShape, result);
        }

        public static RPSRound FromPredictedResult(RPSShapeType theirShape, RPSResultType result)
        {
            RPSShapeType ourShape;


            if (result == RPSResultType.Draw)
            {
                ourShape = theirShape;
            }
            else
            {
                ourShape = theirShape switch
                {
                    RPSShapeType.Rock =>
                        result == RPSResultType.Win ? RPSShapeType.Paper : RPSShapeType.Scissors,

                    RPSShapeType.Paper =>
                        result == RPSResultType.Win ? RPSShapeType.Scissors : RPSShapeType.Rock,

                    RPSShapeType.Scissors =>
                        result == RPSResultType.Win ? RPSShapeType.Rock : RPSShapeType.Paper,

                    _ => throw new NotImplementedException("Not implemented logic for handling the given additional shape")
                };
            }

            return new RPSRound(theirShape, ourShape, result);
        }
    }

    public class PartOneDecoder
    {
        public static RPSRound RoundFromRow(string row)
        {
            return RPSRound.FromShapeChoices(ShapeTypeFromCode(row[0]), ShapeTypeFromCode(row[2]));
        }

        public static RPSShapeType ShapeTypeFromCode(char code)
        {
            return code switch
            {
                'A' => RPSShapeType.Rock,
                'X' => RPSShapeType.Rock,

                'B' => RPSShapeType.Paper,
                'Y' => RPSShapeType.Paper,

                'C' => RPSShapeType.Scissors,
                'Z' => RPSShapeType.Scissors,

                _ => throw new ArgumentException($"Unexpected RockPaperScissors symbol '{code}' should be ABC or XYZ")
            };
        }
    }



    public class PartTwoDecoder
    {
        public static RPSRound RoundFromRow(string row)
        {
            return RPSRound.FromPredictedResult(ShapeTypeFromCode(row[0]), ResultTypeFromCode(row[2]));
        }

        public static RPSShapeType ShapeTypeFromCode(char code)
        {
            return code switch
            {
                'A' => RPSShapeType.Rock,
                'B' => RPSShapeType.Paper,
                'C' => RPSShapeType.Scissors,
                _ => throw new ArgumentException($"Unexpected RockPaperScissors symbol '{code}' should be ABC or XYZ")
            };
        }

        private static RPSResultType ResultTypeFromCode(char code)
        {
            return code switch
            {
                'X' => RPSResultType.Lose,
                'Y' => RPSResultType.Draw,
                'Z' => RPSResultType.Win,
                _ => throw new ArgumentException($"Cannot create result from symbol {code}, expected XYZ")
            };
        }
    }

    [TestClass]
    public class Solution
    {
        [TestMethod]
        public void PartOne()
        {
            var input = TestData.ReadLines("day2");

            // create scoring rounds from each row
            var rounds = input.Select(x => PartOneDecoder.RoundFromRow(x));


            // part one solution: find the total expected score
            var totalScore = rounds.Select(x => x.Score)
                                   .Aggregate((acc, x) => acc + x);
            Console.WriteLine("answer: " + totalScore.ToString());
            Assert.AreEqual(12535, totalScore);
        }


        [TestMethod]
        public void PartTwo()
        {
            var input = TestData.ReadLines("day2");

            // create scoring rounds from each row
            var rounds = input.Select(x => PartTwoDecoder.RoundFromRow(x));


            // part two solution: find the total expected score using the different predicted result
            var totalScore = rounds.Select(x => x.Score)
                                   .Aggregate((acc, x) => acc + x);
            Console.WriteLine("answer: " + totalScore.ToString());
            Assert.AreEqual(15457, totalScore);
        }
    }
}