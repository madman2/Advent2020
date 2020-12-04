using System;
using System.Linq;

namespace Advent2020.Utils
{
    public static class StringParsers
    {
        public static string[] SplitDelimitedStringIntoListOfStrings(string stringToSplit, char[] delimiter)
        {
            return stringToSplit.Trim().Split(delimiter)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x))
                .ToArray();
        }

        public static int[] SplitDelimitedStringIntoListOfInts(string stringToSplit, char[] delimiter, int fromBase)
        {
            return stringToSplit.Trim().Split(delimiter)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => Convert.ToInt32(x, fromBase))
                .ToArray();
        }
    }
}
