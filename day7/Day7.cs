using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode.Day7
{
    public class Directory
    {
        public Directory? Parent;
        public Dictionary<string, Directory> Subdirs = new Dictionary<string, Directory>();
        public Dictionary<string, int> FileSizes = new Dictionary<string, int>();

        public Directory AddChildDir(string name)
        {
            if (!Subdirs.ContainsKey(name))
            {
                Subdirs[name] = new Directory()
                {
                    Parent = this
                };
            }

            return Subdirs[name];
        }

        public int Size
        {
            get
            {
                return FileSizes.Values.Aggregate(0, (acc, x) => acc + x)
                     + Subdirs.Values.Select(x => x.Size).Aggregate(0, (acc, x) => acc + x);
            }
        }
    }

    public class InputParser
    {
        public static Directory BuildDirectoryTree(IEnumerable<string> input)
        {
            var root = new Directory();
            var currentDir = root;

            foreach (var row in input)
            {
                var splits = row.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (row == "$ cd /")
                {
                    currentDir = root;
                }
                else if(row == "$ cd ..")
                {
                    currentDir = currentDir.Parent;
                }
                else if (splits[1] == "cd")
                {
                    var dirToGoInto = splits[2];
                    currentDir = currentDir.Subdirs[dirToGoInto];
                }
                else if (splits[0] == "dir")
                {
                    var dirName = splits[1];
                    currentDir.AddChildDir(dirName);
                }
                else if (splits[0] != "$") // ignore any other command ($ blah blah)
                {
                    var filesize = int.Parse(splits[0]);
                    var fileName = splits[1];
                    currentDir.FileSizes[fileName] = filesize;
                }
            }

            return root;
        }
    }

    [TestClass]
    public class Solution
    {
        [TestMethod]
        public void PartOne()
        {
            var input = TestData.ReadLines("day7");

            var root = InputParser.BuildDirectoryTree(input);

            // answer is the total size of all directories who's size is > 10000
            var allDirectories = TreeFlatten.Flatten(root, (x) => x.Subdirs.Values);
            var smallDirs = allDirectories.Where(x => x.Size <= 100000);

            var answer = smallDirs.Select(x => x.Size).Aggregate((acc, x) => acc + x);
            Console.WriteLine("answer: " + answer);
            Assert.AreEqual(1611443, answer);
        }

        [TestMethod]
        public void PartTwo()
        {
            var input = TestData.ReadLines("day7");

            var root = InputParser.BuildDirectoryTree(input);


            // answer is the size of the smallest directory we can delete, so the root is no larger than 40000000
            var rootSize = root.Size;
            var sizeToClear = rootSize - 40000000;

            var allDirectories = TreeFlatten.Flatten(root, (x) => x.Subdirs.Values);
            var bigDirs = allDirectories.Where(x => x.Size >= sizeToClear);

            var answer = bigDirs.OrderBy(x => x.Size).First(); // sort smallest => biggest, and take the smallest match
            Console.WriteLine("answer: " + answer.Size);
            Assert.AreEqual(2086088, answer.Size);
        }
    }
}