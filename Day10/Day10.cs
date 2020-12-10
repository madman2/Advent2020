using Advent2020.Utils;
using System.IO;
using System.Linq;

namespace Advent2020
{
    class Day10 : ISolver
    {
        public string FirstStarInputFile { get; } = "input.txt";
        public string SecondStarInputFile { get; } = "input.txt";

        public string SolveFirstStar(StreamReader reader)
        {
            var adapters = StreamParsers.GetStreamAsLongIntList(reader);
            adapters.Sort();

            long currentJoltage = 0;
            int smallJumps = 0;
            int bigJumps = 0;
            foreach (var adapter in adapters)
            {
                if (adapter - currentJoltage == 1)
                    smallJumps++;
                else
                    bigJumps++;
                currentJoltage = adapter;
            }
            bigJumps++;
            return (smallJumps * bigJumps).ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var adapters = StreamParsers.GetStreamAsLongIntList(reader);
            adapters.Sort();

            // Build up an array of possibilities, where possibilities[i] represents
            // the number of possible ways to configure a valid adapter chain
            // ending in i
            long[] possibilities = new long[adapters.Max() + 1];

            // Add port, which has effective joltage 0
            adapters.Add(0);

            // Initialize possibilities[0] to 1, because there is only one way
            // to configure the port
            possibilities[0] = 1;
            foreach (var adapter in adapters)
            {
                if (adapters.Contains(adapter))
                {
                    if (adapters.Contains(adapter - 3))
                    {
                        possibilities[adapter] += possibilities[adapter - 3];
                    }
                    if (adapters.Contains(adapter - 2))
                    {
                        possibilities[adapter] += possibilities[adapter - 2];
                    }
                    if (adapters.Contains(adapter - 1))
                    {
                        possibilities[adapter] += possibilities[adapter - 1];
                    }
                }
            }

            return possibilities.Last().ToString(); ;
        }
    }
}