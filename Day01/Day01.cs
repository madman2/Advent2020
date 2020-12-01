using System;
using System.Collections.Generic;
using System.IO;

namespace Advent2020
{
    public class Day01 : ISolver
    {
        private const int TargetSum = 2020;

        public string SolveFirstStar(StreamReader reader)
        {
            var numberSet = new HashSet<int>();

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var n = int.Parse(line);
                var difference = TargetSum - n;
                if (numberSet.Contains(difference))
                {
                    return (n * difference).ToString();
                }
                numberSet.Add(n);
            }

            throw new Exception($"Unable to find a pair of numbers that add to {TargetSum}");
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var numberList = new List<int>();
            var numberDict = new Dictionary<int, int>();

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var n = int.Parse(line);
                numberList.Add(n);

                // Count duplicate entries by incrementing the value in the dict
                if (numberDict.ContainsKey(n))
                {
                    numberDict[n] = numberDict[n] + 1;
                }
                else
                {
                    numberDict[n] = 1;
                }
            }

            // Iterate through each pair of expense report values
            for (int i = 0; i < numberList.Count; ++i)
            {
                for (int j = 0; j < numberList.Count; ++j)
                {
                    // Can't use the same expense report twice
                    if (i == j)
                        continue;

                    // Compute the missing number that we are looking for
                    int difference = TargetSum - (numberList[i] + numberList[j]);
                    if (numberDict.ContainsKey(difference))
                    {
                        // If the missing number is a duplicate, make sure that there is a duplicate number available
                        if (numberList[i] == difference || numberList[j] == difference)
                        {
                            if (numberDict[difference] == 1)
                            {
                                // Do not have an additional copy of that number we can use, continue
                                continue;
                            }
                        }
                        return (numberList[i] * numberList[j] * difference).ToString();
                    }
                }
            }

            throw new Exception($"Unable to find a set of three numbers that add to {TargetSum}");
        }
    }
}
