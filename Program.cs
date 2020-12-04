using System;
using System.IO;

namespace Advent2020
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // Create solver
            ISolver solver = new Day04();

            try
            {
                // Read first input file
                var firstStarFilePath = Path.Combine(solver.GetType().Name, solver.FirstStarInputFile);
                var firstStarFileReader = File.OpenText(firstStarFilePath);

                var firstStarResult = solver.SolveFirstStar(firstStarFileReader);
                Console.WriteLine($"First star result: {firstStarResult}");
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

                var secondStarResult = solver.SolveSecondStar(secondStarFileReader);
                Console.WriteLine($"Second star result: {secondStarResult}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception thrown while solving second star: {e.Message}");
            }
        }
    }
}
