using Advent2020.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace Advent2020
{
    class Day05: ISolver
    {
        public string FirstStarInputFile { get; } = "input.txt";
        public string SecondStarInputFile { get; } = "input.txt";

        private ISet<int> seatsFound = new HashSet<int>();

        public string SolveFirstStar(StreamReader reader)
        {
            var boardingPasses = StreamParsers.GetStreamAsStringList(reader);
            int maxId = 0;
            foreach (var pass in boardingPasses)
            {
                var rowEncoding = pass.Substring(0, 7);
                var columnEncoding = pass.Substring(7);

                var row = GetRow(rowEncoding);
                var column = GetColumn(columnEncoding);

                var id = row * 8 + column;

                if (id > maxId)
                    maxId = id;
            }

            return maxId.ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var boardingPasses = StreamParsers.GetStreamAsStringList(reader);
            foreach (var pass in boardingPasses)
            {
                var rowEncoding = pass.Substring(0, 7);
                var columnEncoding = pass.Substring(7);

                var row = GetRow(rowEncoding);
                var column = GetColumn(columnEncoding);

                var id = row * 8 + column;
                seatsFound.Add(id);
            }

            for (int i = 0; i <= 1024; ++i)
            {
                if (!seatsFound.Contains(i))
                {
                    if (seatsFound.Contains(i - 1) && seatsFound.Contains(i + 1))
                        return i.ToString();
                }
            }

            throw new Exception("Unable to find seat");
        }

        private int GetRow(string rowEncoding)
        {
            int minRow = 0;
            int maxRow = 127;
            foreach (char c in rowEncoding)
            {
                if (c == 'F')
                {
                    maxRow = maxRow - divideAndRoundUp(maxRow - minRow, 2);
                }
                else if (c == 'B')
                {
                    minRow = minRow + divideAndRoundUp(maxRow - minRow, 2);
                }
            }

            return minRow;
        }

        private int GetColumn(string columnEncoding)
        {
            int minColumn = 0;
            int maxColumn = 7;
            foreach (char c in columnEncoding)
            {
                if (c == 'R')
                {
                    minColumn = minColumn + divideAndRoundUp(maxColumn - minColumn, 2);
                }
                else if (c == 'L')
                {
                    maxColumn = maxColumn - divideAndRoundUp(maxColumn - minColumn, 2);
                }
            }

            return maxColumn;
        }

        private int divideAndRoundUp(int dividend, int divisor)
        {
            return (dividend + divisor - 1) / divisor;
        }
    }
}