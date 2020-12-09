using Advent2020.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent2020
{
    class Day09: ISolver
    {
        public string FirstStarInputFile { get; } = "input.txt";
        public string SecondStarInputFile { get; } = "input.txt";

        private const int WindowSize = 25;
        private long FirstStarResult;

        public string SolveFirstStar(StreamReader reader)
        {
            var numbers = StreamParsers.GetStreamAsLongIntList(reader);
            var recentNumbers = new LinkedList<long>();
            for (int i = 0; i < WindowSize; i++)
            {
                recentNumbers.AddLast(numbers[i]);
            }

            for (int i = WindowSize; i < numbers.Count(); i++)
            {
                if (!isNumberValid(recentNumbers, numbers[i]))
                {
                    FirstStarResult = numbers[i];
                    return numbers[i].ToString();
                }

                recentNumbers.RemoveFirst();
                recentNumbers.AddLast(numbers[i]);
            }

            throw new Exception("Could not find invalid number");
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var numbers = StreamParsers.GetStreamAsLongIntList(reader);
            var subList = findSubListEqualToN(numbers, FirstStarResult);

            if (subList == null)
            {
                throw new Exception($"Unable to find sub-list equal to {FirstStarResult}");
            }

            return (subList.Min() + subList.Max()).ToString();
        }

        private bool isNumberValid(LinkedList<long> possibilities, long n)
        {
            var numberSet = possibilities.ToHashSet<long>();
            foreach (int num in numberSet)
            {
                if (numberSet.Contains(n - num))
                {
                    return true;
                }
            }
            return false;
        }

        private List<long> findSubListEqualToN(List<long> numbers, long n)
        {
            long currentSum = 0;
            int low = 0;
            int high = 0;

            for (; low < numbers.Count(); ++low)
            {
                while (currentSum < n)
                {
                    currentSum += numbers[high];
                    high++;
                }

                if (currentSum == n)
                {
                    return numbers.GetRange(low, high - low + 1);
                }

                currentSum -= numbers[low];
            }

            return null;
        }
    }
}