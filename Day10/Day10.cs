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

            // Add port, which has effective joltage 0
            adapters.Add(0);
            adapters.Sort();

            // Build up an array of possibilities, where possibilities[i] represents
            // the number of possible ways to configure a valid adapter chain
            // ending in i
            long[] possibilities = new long[adapters.Max() + 1];

            // Initialize possibilities[0] to 1, because there is only one way
            // to configure the port
            possibilities[0] = 1;
            possibilities[1] = adapters.Contains(1) ? 1 : 0;
            possibilities[2] = possibilities[1] + (adapters.Contains(2) ? 1 : 0);

            for (int i = 3; i < adapters.Count(); i++)
            {
                var adapter = adapters[i];
                possibilities[adapter] += possibilities[adapter - 3];
                possibilities[adapter] += possibilities[adapter - 2];
                possibilities[adapter] += possibilities[adapter - 1];
            }

            return possibilities.Last().ToString();
        }
    }
}