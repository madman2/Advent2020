using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent2020.Utils
{
    public static class StreamParsers
    {
        public static List<string> GetStreamAsStringList(StreamReader reader)
        {
            var listToReturn = new List<string>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                listToReturn.Add(line);
            }

            return listToReturn;
        }

        public static List<int> GetStreamAsIntList(StreamReader reader, int fromBase = 10)
        {
            var listToReturn = new List<int>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var number = Convert.ToInt32(line, fromBase);
                listToReturn.Add(number);
            }

            return listToReturn;
        }

        public static List<long> GetStreamAsLongIntList(StreamReader reader, int fromBase = 10)
        {
            var listToReturn = new List<long>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var number = Convert.ToInt64(line, fromBase);
                listToReturn.Add(number);
            }

            return listToReturn;
        }

        public static List<List<string>> GetStreamAsListOfDelimitedStrings(StreamReader reader, char[] delimiter)
        {
            var listToReturn = new List<List<string>>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                listToReturn.Add(StringParsers.SplitDelimitedStringIntoStringList(line, delimiter));
            }

            return listToReturn;
        }

        public static List<List<int>> GetStreamAsListOfDelimitedInts(StreamReader reader, char[] delimiter, int fromBase = 10)
        {
            var listToReturn = new List<List<int>>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                listToReturn.Add(StringParsers.SplitDelimitedStringIntoIntList(line, delimiter, fromBase));
            }

            return listToReturn;
        }

        public static List<List<long>> GetStreamAsListOfDelimitedLongInts(StreamReader reader, char[] delimiter, int fromBase = 10)
        {
            var listToReturn = new List<List<long>>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                listToReturn.Add(StringParsers.SplitDelimitedStringIntoLongIntList(line, delimiter, fromBase));
            }

            return listToReturn;
        }

        public static List<char[]> GetStreamAs2DCharArray(StreamReader reader)
        {
            var listToReturn = new List<char[]>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                listToReturn.Add(line.ToCharArray());
            }

            return listToReturn;
        }

        public static List<int[]> GetStreamAs2DIntArray(StreamReader reader, char oneRepresentation = '#')
        {
            var listToReturn = new List<int[]>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                listToReturn.Add(line.Select(x => x == oneRepresentation ? 1 : 0).ToArray());
            }

            return listToReturn;
        }
    }
}
