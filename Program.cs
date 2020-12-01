using System;
using System.IO;

namespace Advent2020
{
    class Program
    {
        private const string DayToSolve = "Day01";
        private const string FirstStarInputName = "input.txt";
        private const string SecondStarInputName = "input.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // Create solver
            ISolver solver = new Day01();

            try
            {
                // Read first input file
                var firstStarFilePath = Path.Combine(DayToSolve, FirstStarInputName);
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
                var secondStarFilePath = Path.Combine(DayToSolve, SecondStarInputName);
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
