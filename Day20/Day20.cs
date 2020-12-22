using Advent2020.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Advent2020
{
    class Day20 : ISolver
    {
        public string FirstStarInputFile { get; } = "input.txt";
        public string SecondStarInputFile { get; } = "input.txt";

        private readonly string TileNumberRegex = @"Tile (\d+):";

        private readonly char[][] SeaMonster =
        {
            "                  # ".ToCharArray(),
            "#    ##    ##    ###".ToCharArray(),
            " #  #  #  #  #  #   ".ToCharArray()
        };

        public string SolveFirstStar(StreamReader reader)
        {
            var tiles = new List<Tile>();
            int tileCount = 0;

            string tileTitle;
            while ((tileTitle = reader.ReadLine()) != null)
            {
                tileCount++;
                var tileNumber = int.Parse(Regex.Match(tileTitle, TileNumberRegex).Groups[1].Value);
                var line = reader.ReadLine();
                var tilePixels = new char[line.Length][];
                int row = 0;
                while (!string.IsNullOrWhiteSpace(line))
                {
                    tilePixels[row] = line.ToCharArray();
                    line = reader.ReadLine();
                    row++;
                }
                for (int i = 0; i < 8; i++)
                {
                    tiles.Add(new Tile(tileNumber, tilePixels.Length, tilePixels, i));
                }
            }

            var imageDim = (int)Math.Sqrt(tileCount);
            var image = new Image(imageDim, tiles);
            image.Solve();
            if (!image.Solved)
            {
                throw new Exception("Could not solve image");
            }

            long cornerProduct = 1;
            foreach (var corner in image.GetCornerTileIds())
            {
                cornerProduct *= corner;
            }
            return cornerProduct.ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var tiles = new List<Tile>();
            int tileCount = 0;

            string tileTitle;
            while ((tileTitle = reader.ReadLine()) != null)
            {
                tileCount++;
                var tileNumber = int.Parse(Regex.Match(tileTitle, TileNumberRegex).Groups[1].Value);
                var line = reader.ReadLine();
                var tilePixels = new char[line.Length][];
                int row = 0;
                while (!string.IsNullOrWhiteSpace(line))
                {
                    tilePixels[row] = line.ToCharArray();
                    line = reader.ReadLine();
                    row++;
                }
                for (int i = 0; i < 8; i++)
                {
                    tiles.Add(new Tile(tileNumber, tilePixels.Length, tilePixels, i));
                }
            }

            var imageDim = (int)Math.Sqrt(tileCount);
            var image = new Image(imageDim, tiles);
            image.Solve();
            if (!image.Solved)
            {
                throw new Exception("Could not solve image");
            }

            image.ReplaceChar(SeaMonster, '#', 'O');
            int waterRoughness = image.CountCharInstances('#');

            image.PrintTrueImage();

            return waterRoughness.ToString();
        }

        
        private class Tile
        {
            private int dim;
            private char[][] pixels;

            public int Id { get; private set; }
            public List<int> Edges { get; private set; }

            public Tile(int id, int dim, char[][] tilePixels, int orientation)
            {
                Id = id;
                this.dim = dim;
                pixels = tilePixels.Select(s => s.ToArray()).ToArray();
                ChangeOrientation(orientation);
                ComputeEdges();
            }

            public char[][] GetPixels()
            {
                return pixels;
            }

            private void ComputeEdges()
            {
                var left = new char[dim];
                var right = new char[dim];
                for (int row = 0; row < dim; row++)
                {
                    left[row] = pixels[row].First();
                    right[row] = pixels[row].Last();
                }
                var top = pixels.First();
                var bottom = pixels.Last();
                var rawEdges = new List<char[]>() { top, right, bottom, left };

                Edges = new List<int>();
                foreach (var edge in rawEdges)
                {
                    Edges.Add(EdgeToInt(edge));
                }
            }

            private void ChangeOrientation(int offset)
            {
                for (int i = 0; i < offset / 2; i++)
                {
                    RotateTile();
                }

                if (offset % 2 == 1)
                {
                    FlipTile();
                }
            }

            private void FlipTile()
            {
                Matrix.ReverseRows(pixels);
            }

            private void RotateTile()
            {
                pixels = Matrix.Transpose(pixels);
                Matrix.ReverseRows(pixels);
            }

            private int EdgeToInt(char[] edge)
            {
                var bitVector = new BitVector32();
                int mask = 1;
                for (int i = edge.Length - 1; i >= 0; i--)
                {
                    if (edge.ElementAt(i) == '#')
                    {
                        bitVector[mask] = true;
                    }
                    mask <<= 1;
                }
                return bitVector.Data;
            }
        }

        private class Image
        {
            private Tile[][] rawImage;
            private List<Tile> tiles;
            private int dim;
            private char[][] trueImage;
            private Dictionary<(int edgePos, int edgeVal), List<Tile>> edgeLookup;

            public bool Solved { get; private set; }

            public Image(int dim, List<Tile> tiles)
            {
                this.dim = dim;
                this.tiles = tiles;
                edgeLookup = new Dictionary<(int edgePos, int edgeVal), List<Tile>>();
                foreach (var tile in tiles)
                {
                    for (int edgePos = 0; edgePos < tile.Edges.Count; edgePos++)
                    {
                        var edgeEntry = (edgePos, tile.Edges[edgePos]);
                        if (edgeLookup.ContainsKey(edgeEntry))
                        {
                            edgeLookup[edgeEntry].Add(tile);
                        }
                        else
                        {
                            edgeLookup[edgeEntry] = new List<Tile>() { tile };
                        }
                    }
                }
            }

            public void Solve()
            {
                rawImage = new Tile[dim][];
                for (int row = 0; row < dim; row++)
                {
                    rawImage[row] = new Tile[dim];
                }
                var usedTiles = new HashSet<int>();

                Solved = Solve(0, 0, usedTiles);

                if (Solved)
                {
                    RevealTrueImage();
                }
            }

            private bool Solve(int row, int col, ISet<int> usedTiles)
            {
                if (row == dim - 1 && col == dim)
                {
                    return true;
                }

                if (col == dim)
                {
                    col = 0;
                    row++;
                }

                var possibleTiles = GetPossibleTiles(row, col, usedTiles);
                foreach (var tile in possibleTiles)
                {
                    usedTiles.Add(tile.Id);
                    rawImage[row][col] = tile;
                    if (Solve(row, col + 1, usedTiles))
                    {
                        return true;
                    }
                    usedTiles.Remove(tile.Id);
                    rawImage[row][col] = null;
                }

                return false;
            }

            private List<Tile> GetPossibleTiles(int row, int col, ISet<int> usedTiles)
            {
                if (row == 0 && col == 0)
                {
                    return tiles;
                }

                var matchingTiles = new HashSet<Tile>();
                if (row > 0)
                {
                    if (rawImage[row - 1][col] != null)
                    {
                        var validTilesTop = edgeLookup[(0, rawImage[row - 1][col].Edges[2])];
                        foreach (var tile in validTilesTop)
                        {
                            matchingTiles.Add(tile);
                        }
                    }
                }
                if (col > 0)
                {
                    if (rawImage[row][col - 1] != null)
                    {
                        var validTilesLeft = edgeLookup[(3, rawImage[row][col - 1].Edges[1])];
                        foreach (var tile in validTilesLeft)
                        {
                            matchingTiles.Add(tile);
                        }
                    }
                }

                matchingTiles.RemoveWhere(x => usedTiles.Contains(x.Id));

                return matchingTiles.ToList();
            }

            public List<int> GetCornerTileIds()
            {
                if (!Solved)
                {
                    return null;
                }
                var result = new List<int>();
                result.Add(rawImage.First().First().Id);
                result.Add(rawImage.First().Last().Id);
                result.Add(rawImage.Last().First().Id);
                result.Add(rawImage.Last().Last().Id);
                return result;
            }

            public int CountCharInstances(char charToFind)
            {
                if (!Solved)
                {
                    throw new Exception("Image has not been solved yet");
                }

                int count = 0;
                foreach (var line in trueImage)
                {
                    count += line.Count(c => c == charToFind);
                }

                return count;
            }

            public void ReplaceChar(char[][] pattern, char findChar, char replaceChar)
            {
                if (!Solved)
                {
                    throw new Exception("Image not solved yet");
                }

                for (int i = 0; i < 8; i++)
                {
                    if (i % 2 == 0)
                    {
                        if (i > 0)
                        {
                            if (i == 4)
                            {
                                RotateImage(false);
                            }
                            else
                            {
                                RotateImage();
                            }
                        }
                    }
                    else
                    {
                        FlipImage();
                    }

                    if (FindAndMarkPattern(pattern, findChar, replaceChar))
                    {
                        return;
                    }
                }
            }

            public void PrintRawImage()
            {
                if (!Solved)
                {
                    throw new Exception("Image not solved yet");
                }

                foreach (var tileRow in rawImage)
                {
                    for (int pixelRow = 0; pixelRow < rawImage.First().First().GetPixels().Length; pixelRow++)
                    {
                        string line = "";
                        foreach (var tile in tileRow)
                        {
                            var row = tile.GetPixels()[pixelRow];
                            line += new string(row) + " ";
                        }
                        Console.WriteLine(line);
                    }
                    Console.WriteLine();
                }
            }

            public void PrintTrueImage()
            {
                if (!Solved)
                {
                    throw new Exception("Image not solved yet");
                }

                foreach (var row in trueImage)
                {
                    Console.WriteLine(new string(row));
                }
            }

            private void RevealTrueImage()
            {
                var tileDim = rawImage.First().First().GetPixels().Length;
                trueImage = new char[(tileDim - 2) * rawImage.First().Length][];
                int trueImageRow = 0;
                foreach (var imageRow in rawImage)
                {
                    for (int pixelRow = 1; pixelRow < tileDim - 1; pixelRow++)
                    {
                        string line = "";
                        foreach (var tile in imageRow)
                        {
                            for (int pixelCol = 1; pixelCol < tileDim - 1; pixelCol++)
                            {
                                line += tile.GetPixels()[pixelRow][pixelCol];
                            }
                        }
                        trueImage[trueImageRow] = line.ToCharArray();
                        trueImageRow++;
                    }
                }
            }

            private void PrintEdges()
            {
                var top = trueImage.First();
                var bottom = trueImage.Last();
                var left = new char[trueImage.Length];
                var right = new char[trueImage.Length];
                for (int i = 0; i < trueImage.Length; i++)
                {
                    left[i] = trueImage[i].First();
                    right[i] = trueImage[i].Last();
                }

                Console.WriteLine($"Top: {new string(top)}");
                Console.WriteLine($"Right: {new string(right)}");
                Console.WriteLine($"Bottom: {new string(bottom)}");
                Console.WriteLine($"Left: {new string(left)}");
            }

            private bool FindAndMarkPattern(char[][] pattern, char findChar, char replaceChar)
            {
                var pWidth = pattern[0].Length;
                var pHeight = pattern.Length;

                bool foundPattern = false;
                for (int iRow = 0; iRow < trueImage.Length - (pHeight - 1); iRow++)
                {
                    for (int iCol = 0; iCol < trueImage.First().Length - (pWidth - 1); iCol++)
                    {
                        bool match = true;
                        for (int pRow = 0; pRow < pHeight; pRow++)
                        {
                            for (int pCol = 0; pCol < pWidth; pCol++)
                            {

                                if (pattern[pRow][pCol] == findChar &&
                                    trueImage[iRow + pRow][iCol + pCol] != findChar)
                                {
                                    match = false;
                                    break;
                                }
                            }
                            if (!match)
                            {
                                break;
                            }
                        }

                        if (match)
                        {
                            foundPattern = true;
                            for (int pRow = 0; pRow < pHeight; pRow++)
                            {
                                for (int pCol = 0; pCol < pWidth; pCol++)
                                {
                                    if (pattern[pRow][pCol] == findChar)
                                    {
                                        trueImage[iRow + pRow][iCol + pCol] = replaceChar;
                                    }
                                }
                            }
                        }
                    }
                }
                return foundPattern;
            }

            private void RotateImage(bool right = true)
            {
                if (right)
                {
                    trueImage = Matrix.Transpose(trueImage);
                    Matrix.ReverseRows(trueImage);
                }
                else
                {
                    Matrix.ReverseRows(trueImage);
                    trueImage = Matrix.Transpose(trueImage);
                }
            }

            private void FlipImage()
            {
                Matrix.ReverseRows(trueImage);
            }
        }
    }
}