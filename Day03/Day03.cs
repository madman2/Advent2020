using Advent2020.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Advent2020
{
    class Day03: ISolver
    {
        public string FirstStarInputFile { get; } = "input.txt";
        public string SecondStarInputFile { get; } = "input.txt";

        private List<int[]> treeMap;

        public string SolveFirstStar(StreamReader reader)
        {
            treeMap = StreamParsers.GetStreamAs2DIntArray(reader);

            return CountTrees(3, 1).ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            treeMap = StreamParsers.GetStreamAs2DIntArray(reader);

            var treeProduct = CountTrees(1, 1) * CountTrees(3, 1) * CountTrees(5, 1) * CountTrees(7, 1) * CountTrees(1, 2);
            return treeProduct.ToString();
        }

        private long CountTrees(int right, int down)
        {
            int x = 0;
            int y = 0;
            long treeCount = 0;

            while (y < (treeMap.Count - down))
            {
                y += down;
                x = (x + right) % treeMap.First().Length;
                treeCount += treeMap[y][x];
            }

            return treeCount;
        }
    }
}
