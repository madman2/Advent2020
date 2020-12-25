using Advent2020.Utils;
using System.IO;

namespace Advent2020
{
    class Day25 : ISolver
    {
        public string FirstStarInputFile { get; } = "input.txt";
        public string SecondStarInputFile { get; } = "input.txt";

        private const int TransformDivisor = 20201227;
        private const int DefaultSubject = 7;

        public string SolveFirstStar(StreamReader reader)
        {
            var lines = StreamParsers.GetStreamAsLongIntList(reader);
            var cardPublicKey = lines[0];
            var doorPublicKey = lines[1];

            var doorLoops = ReverseTransform(DefaultSubject, doorPublicKey);

            return TransformLoop(cardPublicKey, doorLoops).ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            return "Free star!";
        }

        private long Transform(long result, long subject)
        {
            return (result * subject) % TransformDivisor;
        }

        private long TransformLoop(long subject, long loops)
        {
            long result = 1;
            for (int i = 0; i < loops; i++)
            {
                result = Transform(result, subject);
            }

            return result;
        }

        private long ReverseTransform(long subject, long finalResult)
        {
            long result = 1;
            long loops = 0;
            while (result != finalResult)
            {
                result = Transform(result, subject);
                loops++;
            }

            return loops;
        }
    }
}