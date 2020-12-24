using Advent2020.Utils;
using System.Collections.Generic;
using System.IO;

namespace Advent2020
{
    class Day24 : ISolver
    {
        public string FirstStarInputFile { get; } = "input.txt";
        public string SecondStarInputFile { get; } = "input.txt";

        private readonly List<(int row, int col)> Directions = new List<(int row, int col)>()
        {
            (0, 2),
            (1, 1),
            (1, -1),
            (-1, 1),
            (-1, -1),
            (0, -2)
        };

        public string SolveFirstStar(StreamReader reader)
        {
            var lines = StreamParsers.GetStreamAsStringList(reader);
            var tileSet = new HashSet<(int row, int col)>();

            foreach (var line in lines)
            {
                int i = 0;
                var row = 0;
                var col = 0;
                while (i < line.Length)
                {
                    int horizontalDistance = 2;
                    if (line[i] == 'n')
                    {
                        row++;
                        i++;
                        horizontalDistance = 1;
                    }
                    else if (line[i] == 's')
                    {
                        row--;
                        i++;
                        horizontalDistance = 1;
                    }
                    if (line[i] == 'w')
                    {
                        col -= horizontalDistance;
                    }
                    else if (line[i] == 'e')
                    {
                        col += horizontalDistance;
                    }

                    i++;
                }

                if (tileSet.Contains((row, col)))
                {
                    tileSet.Remove((row, col));
                }
                else
                {
                    tileSet.Add((row, col));
                }
            }

            return tileSet.Count.ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var lines = StreamParsers.GetStreamAsStringList(reader);
            var tileSet = new HashSet<(int row, int col)>();
            var limits = new List<int>() { int.MaxValue, int.MinValue, int.MaxValue, int.MinValue };

            foreach (var line in lines)
            {
                int i = 0;
                var row = 0;
                var col = 0;
                while (i < line.Length)
                {
                    int horizontalDistance = 2;
                    if (line[i] == 'n')
                    {
                        row++;
                        i++;
                        horizontalDistance = 1;
                        if (row > limits[1])
                        {
                            limits[1] = row;
                        }
                    }
                    else if (line[i] == 's')
                    {
                        row--;
                        i++;
                        horizontalDistance = 1;
                        if (row < limits[0])
                        {
                            limits[0] = row;
                        }
                    }
                    if (line[i] == 'w')
                    {
                        col -= horizontalDistance;
                        if (col < limits[2])
                        {
                            limits[2] = col;
                        }
                    }
                    else if (line[i] == 'e')
                    {
                        col += horizontalDistance;
                        if (col > limits[3])
                        {
                            limits[3] = col;
                        }
                    }

                    i++;
                }

                if (tileSet.Contains((row, col)))
                {
                    tileSet.Remove((row, col));
                }
                else
                {
                    tileSet.Add((row, col));
                }
            }

            int day = 1;
            while (day <= 100)
            {
                var tilesToFlip = new List<(int row, int col)>();
                for (int row = limits[0] - 2 * day; row < limits[1] + 2 * day; row++)
                {
                    for (int col = limits[2] - 2 * day; col < limits[3] + 2 * day; col++)
                    {
                        var blackTilesNearby = 0;
                        foreach (var dir in Directions)
                        {
                            var neighborTile = (row + dir.row, col + dir.col);
                            if (tileSet.Contains(neighborTile))
                            {
                                blackTilesNearby++;
                            }
                        }

                        // Black tile
                        if (tileSet.Contains((row, col)))
                        {
                            if (blackTilesNearby == 0 || blackTilesNearby > 2)
                            {
                                tilesToFlip.Add((row, col));
                            }
                        }
                        // White tile
                        else
                        {
                            if (blackTilesNearby == 2)
                            {
                                tilesToFlip.Add((row, col));
                            }
                        }
                    }
                }

                foreach (var tile in tilesToFlip)
                {
                    if (tileSet.Contains(tile))
                    {
                        tileSet.Remove(tile);
                    }
                    else
                    {
                        tileSet.Add(tile);
                    }
                }

                day++;
            }

            return tileSet.Count.ToString();
        }
    }
}