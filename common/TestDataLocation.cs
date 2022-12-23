using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    class TestData
    {
        private const string Root = "../../../";
        
        public static IEnumerable<string> ReadLines(string dayName)
        {
            return System.IO.File.ReadLines($"{Root}/{dayName}/input.txt");
        }

        public static string ReadAllText(string dayName)
        {
            return System.IO.File.ReadAllText($"{Root}/{dayName}/input.txt");
        }
    }
}
