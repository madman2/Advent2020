using Advent2020.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent2020
{
    class Day15 : ISolver
    {
        public string FirstStarInputFile { get; } = "input.txt";
        public string SecondStarInputFile { get; } = "input.txt";

        private const int FirstStarN = 2020;
        private const int SecondStarN = 30000000;

        public string SolveFirstStar(StreamReader reader)
        {
            var numbers = StringParsers.SplitDelimitedStringIntoIntList(reader.ReadLine(), ",".ToCharArray());
            return GetNthNumberSpoken(numbers, FirstStarN).ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var numbers = StringParsers.SplitDelimitedStringIntoIntList(reader.ReadLine(), ",".ToCharArray());
            return GetNthNumberSpoken(numbers, SecondStarN).ToString();
        }

        private int GetNthNumberSpoken(List<int> initialNumbers, int n)
        {
            // Contains a dictionary mapping a number to a queue that contains
            // the last two rounds it has been spoken in, or just the last
            // round it was spoken if it has only been spoken once
            var numbersDict = new Dictionary<int, Queue<int>>();

            // Initialize Dictionary with initial numbers and their respective rounds
            for (int i = 0; i < initialNumbers.Count; i++)
            {
                numbersDict.Add(initialNumbers[i], new Queue<int>());
                numbersDict[initialNumbers[i]].Enqueue(i);
            }

            var lastNumber = initialNumbers.Last<int>();
            for (int i = initialNumbers.Count; i < n; i++)
            {
                var nextNumber = 0;

                // We know that lastNumber is already in the numbersDict, with either
                // one or two elements in its queue
                // If it has been spoken more than once, the difference between
                // the elements in the queue becomes the nextNumber
                if (numbersDict[lastNumber].Count() > 1)
                {
                    var previousTurn = numbersDict[lastNumber].Dequeue();
                    nextNumber = numbersDict[lastNumber].Peek() - previousTurn;
                }

                // If the new number has never been spoken, add it to the numbersDict
                if (!numbersDict.ContainsKey(nextNumber))
                {
                    numbersDict[nextNumber] = new Queue<int>();
                }
                // Add this new number to the numbersDict with the current round
                numbersDict[nextNumber].Enqueue(i);
                lastNumber = nextNumber;
            }

            return lastNumber;
        }
    }
}