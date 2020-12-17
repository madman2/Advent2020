using Advent2020.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent2020
{
    class Day17 : ISolver
    {
        public string FirstStarInputFile { get; } = "input.txt";
        public string SecondStarInputFile { get; } = "input.txt";

        private IDictionary<ValueTuple<int, int, int, int>, bool> CubeSpace = new Dictionary<ValueTuple<int, int, int, int>, bool>();

        // Note: Gross, lazy solution

        public string SolveFirstStar(StreamReader reader)
        {
            CubeSpace = new Dictionary<ValueTuple<int, int, int, int>, bool>();
            var cubes = StreamParsers.GetStreamAsStringList(reader);
            for (int y = 0; y < cubes.Count; y++)
            {
                for (int x = 0; x < cubes[y].Count(); x++)
                {
                    if (cubes[y][x] == '#')
                    {
                        CubeSpace.Add((x, y, 0, 0), true);
                    }
                }
            }

            int xMin = -1;
            int yMin = -1;
            int zMin = -1;
            int xRange = cubes[0].Count() + 2;
            int yRange = cubes.Count + 2;
            int zRange = 3;

            var activeCubes = 0;
            for (int i = 1; i <= 6; i++)
            {
                activeCubes = 0;
                var newCubeSpace = new Dictionary<ValueTuple<int, int, int, int>, bool>();
                for (int z = zMin; z < zMin + zRange; z++)
                {
                    for (int y = yMin; y < yMin + yRange; y++)
                    {
                        for (int x = xMin; x < xMin + xRange; x++)
                        {
                            var isActive = CubeSpace.ContainsKey((x, y, z, 0)) && CubeSpace[(x, y, z, 0)];
                            var activeNeighbors = CountActiveNeighbors(x, y, z);
                            if (isActive)
                            {
                                if (activeNeighbors == 2 || activeNeighbors == 3)
                                {
                                    newCubeSpace.Add((x, y, z, 0), true);
                                    activeCubes++;
                                }
                                else
                                {
                                    newCubeSpace.Add((x, y, z, 0), false);
                                }
                            }
                            else
                            {
                                if (activeNeighbors == 3)
                                {
                                    newCubeSpace.Add((x, y, z, 0), true);
                                    activeCubes++;
                                }
                                else
                                {
                                    newCubeSpace.Add((x, y, z, 0), false);
                                }
                            }
                        }
                    }
                }

                CubeSpace = newCubeSpace;

                xMin--;
                yMin--;
                zMin--;
                xRange += 2;
                yRange += 2;
                zRange += 2;
            }

            return activeCubes.ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            CubeSpace = new Dictionary<ValueTuple<int, int, int, int>, bool>();
            var cubes = StreamParsers.GetStreamAsStringList(reader);
            for (int y = 0; y < cubes.Count; y++)
            {
                for (int x = 0; x < cubes[y].Count(); x++)
                {
                    if (cubes[y][x] == '#')
                    {
                        CubeSpace.Add((x, y, 0, 0), true);
                    }
                }
            }

            int xMin = -1;
            int yMin = -1;
            int zMin = -1;
            int wMin = -1;
            int xRange = cubes[0].Count() + 2;
            int yRange = cubes.Count + 2;
            int zRange = 3;
            int wRange = 3;

            var activeCubes = 0;
            for (int i = 1; i <= 6; i++)
            {
                activeCubes = 0;
                var newCubeSpace = new Dictionary<ValueTuple<int, int, int, int>, bool>();
                for (int w = wMin; w < wMin + wRange; w++)
                {
                    for (int z = zMin; z < zMin + zRange; z++)
                    {
                        for (int y = yMin; y < yMin + yRange; y++)
                        {
                            for (int x = xMin; x < xMin + xRange; x++)
                            {
                                var isActive = CubeSpace.ContainsKey((x, y, z, w)) && CubeSpace[(x, y, z, w)];
                                var activeNeighbors = CountActiveNeighbors(x, y, z, w);
                                if (isActive)
                                {
                                    if (activeNeighbors == 2 || activeNeighbors == 3)
                                    {
                                        newCubeSpace.Add((x, y, z, w), true);
                                        activeCubes++;
                                    }
                                    else
                                    {
                                        newCubeSpace.Add((x, y, z, w), false);
                                    }
                                }
                                else
                                {
                                    if (activeNeighbors == 3)
                                    {
                                        newCubeSpace.Add((x, y, z, w), true);
                                        activeCubes++;
                                    }
                                    else
                                    {
                                        newCubeSpace.Add((x, y, z, w), false);
                                    }
                                }
                            }
                        }
                    }
                }


                CubeSpace = newCubeSpace;

                xMin--;
                yMin--;
                zMin--;
                wMin--;
                xRange += 2;
                yRange += 2;
                zRange += 2;
                wRange += 2;
            }

            return activeCubes.ToString();
        }

        private int CountActiveNeighbors(int cubeX, int cubeY, int cubeZ)
        {
            int activeNeighbors = 0;
            for (int z = cubeZ - 1; z <= cubeZ + 1; z++)
            {
                for (int y = cubeY - 1; y <= cubeY + 1; y++)
                {
                    for (int x = cubeX - 1; x <= cubeX + 1; x++)
                    {
                        if (x == cubeX && y == cubeY && z == cubeZ)
                            continue;

                        if (CubeSpace.ContainsKey((x, y, z, 0)) && CubeSpace[(x, y, z, 0)])
                        {
                            activeNeighbors++;
                        }
                    }
                }
            }

            return activeNeighbors;
        }

        private int CountActiveNeighbors(int cubeX, int cubeY, int cubeZ, int cubeW)
        {
            int activeNeighbors = 0;
            for (int w = cubeW - 1; w <= cubeW + 1; w++)
            {
                for (int z = cubeZ - 1; z <= cubeZ + 1; z++)
                {
                    for (int y = cubeY - 1; y <= cubeY + 1; y++)
                    {
                        for (int x = cubeX - 1; x <= cubeX + 1; x++)
                        {
                            if (x == cubeX && y == cubeY && z == cubeZ && w == cubeW)
                                continue;

                            if (CubeSpace.ContainsKey((x, y, z, w)) && CubeSpace[(x, y, z, w)])
                            {
                                activeNeighbors++;
                            }
                        }
                    }
                }
            }

            return activeNeighbors;
        }
    }
}