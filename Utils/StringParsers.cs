using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent2020.Utils
{
    public static class StringParsers
    {
        public static List<string> SplitDelimitedStringIntoStringList(string stringToSplit, char[] delimiter)
        {
            return stringToSplit.Trim().Split(delimiter)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();
        }

        public static List<int> SplitDelimitedStringIntoIntList(string stringToSplit, char[] delimiter, int fromBase)
        {
            return stringToSplit.Trim().Split(delimiter)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => Convert.ToInt32(x, fromBase))
                .ToList();
        }
    }
}
