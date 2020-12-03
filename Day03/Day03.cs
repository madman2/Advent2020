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

        private List<List<int>> treeMap;

        public string SolveFirstStar(StreamReader reader)
        {
            BuildMap(reader);
            return CountTrees(3, 1).ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            BuildMap(reader);

            var treeProduct = CountTrees(1, 1) * CountTrees(3, 1) * CountTrees(5, 1) * CountTrees(7, 1) * CountTrees(1, 2);
            return treeProduct.ToString();
        }

        private void BuildMap(StreamReader reader)
        {
            treeMap = new List<List<int>>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var rowList = new List<int>();
                foreach (char c in line)
                {
                    rowList.Add(c == '.' ? 0 : 1);
                }
                treeMap.Add(rowList);
            }
        }

        private double CountTrees(int right, int down)
        {
            int x = 0;
            int y = 0;
            double treeCount = 0;

            while (y < (treeMap.Count - down))
            {
                y += down;
                x = (x + right) % treeMap.First().Count;
                treeCount += treeMap[y][x];
            }

            return treeCount;
        }
    }
}
