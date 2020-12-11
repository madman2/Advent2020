using Advent2020.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Advent2020
{
    class Day11 : ISolver
    {
        public string FirstStarInputFile { get; } = "input.txt";
        public string SecondStarInputFile { get; } = "input.txt";

        private enum Version
        {
            Silver,
            Gold
        }

        private List<string> map;

        public string SolveFirstStar(StreamReader reader)
        {
            map = StreamParsers.GetStreamAsStringList(reader);
            var lastFlippedSeats = int.MaxValue;
            for (var flippedSeats = ApplyRules(Version.Silver); lastFlippedSeats - flippedSeats > 0; flippedSeats = ApplyRules(Version.Silver))
            {
                lastFlippedSeats = flippedSeats;
                //PrintMap();
            }
            return CountAllOccupiedSeats().ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            map = StreamParsers.GetStreamAsStringList(reader);
            var lastFlippedSeats = int.MaxValue;
            for (var flippedSeats = ApplyRules(Version.Gold); lastFlippedSeats - flippedSeats > 0; flippedSeats = ApplyRules(Version.Gold))
            {
                lastFlippedSeats = flippedSeats;
                //PrintMap();
            }
            return CountAllOccupiedSeats().ToString();
        }

        private int ApplyRules(Version version)
        {
            var seatThreshold = version == Version.Silver ? 4 : 5;

            var newMap = new List<string>();
            int flippedSeats = 0;
            for (int i = 0; i < map.Count(); i++)
            {
                var row = map[i];
                var newRow = "";
                for (int j = 0; j < row.Length; j++)
                {
                    var occupiedSeats = CountOccupiedSeats(i, j, version);
                    if (row[j] == 'L' && occupiedSeats == 0)
                    {
                        flippedSeats++;
                        newRow += '#';
                    }
                    else if (row[j] == '#' && occupiedSeats >= seatThreshold)
                    {
                        flippedSeats++;
                        newRow += 'L';
                    }
                    else
                    {
                        newRow += row[j];
                    }
                }
                newMap.Add(newRow);
            }

            map = newMap;
            return flippedSeats;
        }

        private int CountOccupiedSeats(int row, int aisle, Version version)
        {
            int count = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                        continue;

                    if (IsOccupied(row, aisle, i, j, version))
                        count++;
                }
            }
            return count;
        }

        private bool IsOccupied(int row, int aisle, int rowDiff, int aisleDiff, Version version)
        {
            var newRow = row + rowDiff;
            var newAisle = aisle + aisleDiff;
            while (version == Version.Gold && IsValidIndex(newRow, newAisle) && map[newRow][newAisle] == '.')
            {
                newRow = newRow + rowDiff;
                newAisle = newAisle + aisleDiff;
            }
            return IsOccupied(newRow, newAisle);
        }

        private bool IsValidIndex(int row, int aisle)
        {
            return row >= 0 && aisle >= 0 && row < map.Count() && aisle < map[row].Length;
        }

        private bool IsOccupied(int row, int aisle)
        {
            return IsValidIndex(row, aisle) && map[row][aisle] == '#';
        }

        private int CountAllOccupiedSeats()
        {
            int count = 0;
            foreach (var row in map)
            {
                foreach (char c in row)
                {
                    if (c == '#')
                        count++;
                }
            }

            return count;
        }

        private void PrintMap()
        {
            foreach (var row in map)
            {
                Console.WriteLine(row);
            }
            Console.WriteLine();
        }
    }
}