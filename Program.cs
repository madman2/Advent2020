using System;
using System.Diagnostics;
using System.IO;

namespace Advent2020
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create solver
            ISolver solver = new Day21();

            try
            {
                // Read first input file
                var firstStarFilePath = Path.Combine(solver.GetType().Name, solver.FirstStarInputFile);
                var firstStarFileReader = File.OpenText(firstStarFilePath);

                var stopWatch = Stopwatch.StartNew();
                var firstStarResult = solver.SolveFirstStar(firstStarFileReader);
                stopWatch.Stop();
                Console.WriteLine($"First star result: {firstStarResult}, {stopWatch.ElapsedMilliseconds}ms");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception thrown while solving first star: {e.Message}");
            }

            try
            {
                // Read second input file
                var secondStarFilePath = Path.Combine(solver.GetType().Name, solver.SecondStarInputFile);
                var secondStarFileReader = File.OpenText(secondStarFilePath);

                var stopWatch = Stopwatch.StartNew();
                var secondStarResult = solver.SolveSecondStar(secondStarFileReader);
                stopWatch.Stop();
                Console.WriteLine($"Second star result: {secondStarResult}, {stopWatch.ElapsedMilliseconds}ms");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception thrown while solving second star: {e.Message}");
            }
        }
    }
}
