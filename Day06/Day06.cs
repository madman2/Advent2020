using System.Collections.Generic;
using System.IO;

namespace Advent2020
{
    class Day06: ISolver
    {
        public string FirstStarInputFile { get; } = "input.txt";
        public string SecondStarInputFile { get; } = "input.txt";

        public string SolveFirstStar(StreamReader reader)
        {
            string line;
            int totalAnswers = 0;
            while ((line = reader.ReadLine()) != null)
            {
                var groupAnswers = new HashSet<char>();
                while (!string.IsNullOrWhiteSpace(line))
                {
                    foreach (char c in line)
                    {
                        groupAnswers.Add(c);
                    }
                    line = reader.ReadLine();
                }

                totalAnswers += groupAnswers.Count;
            }

            return totalAnswers.ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            string line;
            int totalCommonAnswers = 0;
            while ((line = reader.ReadLine()) != null)
            {
                int groupSize = 0;
                var groupAnswersTally = new Dictionary<char, int>();
                while (!string.IsNullOrWhiteSpace(line))
                {
                    groupSize++;
                    foreach (char c in line)
                    {
                        if (groupAnswersTally.ContainsKey(c))
                        {
                            groupAnswersTally[c]++;
                        }
                        else
                        {
                            groupAnswersTally[c] = 1;
                        }
                    }
                    line = reader.ReadLine();
                }

                int numCommon = 0;
                foreach (var value in groupAnswersTally.Values)
                {
                    if (value == groupSize)
                        numCommon++;
                }

                totalCommonAnswers += numCommon;
            }

            return totalCommonAnswers.ToString();
        }
    }
}