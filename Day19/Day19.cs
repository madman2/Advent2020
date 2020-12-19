using Advent2020.Utils;
using System.Collections.Generic;
using System.IO;

namespace Advent2020
{
    class Day19 : ISolver
    {
        public string FirstStarInputFile { get; } = "input.txt";
        public string SecondStarInputFile { get; } = "input.txt";

        Dictionary<int, List<List<int>>> Rules;
        Dictionary<int, char> BaseRules;

        public string SolveFirstStar(StreamReader reader)
        {
            var lines = StreamParsers.GetStreamAsStringList(reader);
            var index = BuildRules(lines);
            
            long count = 0;
            for (int i = index; i < lines.Count; i++)
            {
                var word = lines[i];
                int numMatches;
                var lastIndex = CheckRuleRepeat(word, 0, 0, out numMatches);
                if (numMatches == 1 && lastIndex == word.Length)
                {
                    count++;
                }
            }

            return count.ToString();
        }

        /*
         * For part 2, the input is modified as follows:
         * 0: 8 11 (this is the same)
         * 8: 42 | 42 8
         * 11: 42 31 | 42 11 31
         * 
         * When expanding out rule 0, we see the following pattern:
         * 0: 42 [42 [42.. ] ] [42 [42 [42.. ..31] 31] 31]
         * 
         * So instead of modifying our grammar solver to handle these infinite cases, we can
         * simply check to see if the string matches:
         * 
         * 0: 42 42 42... 31 31...
         * where the number of rule 42 matches is greater than the number of rule 31 matches
         * and the number of rule 31 matches must be at least 1
         */
        public string SolveSecondStar(StreamReader reader)
        {
            var lines = StreamParsers.GetStreamAsStringList(reader);
            var index = BuildRules(lines);

            long count = 0;
            for (int i = index; i < lines.Count; i++)
            {
                var word = lines[i];
                int num42Matches, num31Matches;

                var last42Index = CheckRuleRepeat(word, 42, 0, out num42Matches);
                if (num42Matches < 2)
                {
                    // Start of string needs to match rule 42 at least twice in order to be valid
                    continue;
                }
                var last31Index = CheckRuleRepeat(word, 31, last42Index, out num31Matches);

                // If the rest of the string exactly matched some number of rule 31s and
                // there were more rule 42 matches than rule 31, it is valid according to the
                // new rules definitions
                if (last31Index == word.Length && num31Matches > 0 && num42Matches > num31Matches)
                {
                    count++;
                }
            }

            return count.ToString();
        }

        private int BuildRules(List<string> rules)
        {
            Rules = new Dictionary<int, List<List<int>>>();
            BaseRules = new Dictionary<int, char>();

            for (int i = 0; i < rules.Count; i++)
            {
                var rule = rules[i];
                if (string.IsNullOrEmpty(rule))
                {
                    return i + 1;
                }
                var split = rule.Split(": ");
                var ruleNum = int.Parse(split[0]);
                if (split[1].StartsWith('\"'))
                {
                    BaseRules.Add(ruleNum, split[1][1]);
                    continue;
                }
                var ruleOptions = split[1].Split(" | ");
                var ruleLists = new List<List<int>>();
                foreach (var option in ruleOptions)
                {
                    var ruleList = new List<int>();
                    foreach (var n in option.Split(null))
                    {
                        ruleList.Add(int.Parse(n));
                    }
                    ruleLists.Add(ruleList);
                }
                Rules.Add(ruleNum, ruleLists);
            }

            throw new InvalidDataException("No empty line found");
        }

        private int CheckRule(string word, int rule, int index, out bool result)
        {
            // If the index is past the end of the string, return
            if (index >= word.Length)
            {
                result = false;
                return -1;
            }

            // If we found a base rule, check this index to see if it matches the character
            // defined for the rule
            if (BaseRules.ContainsKey(rule))
            {
                result = word[index] == BaseRules[rule];
                return index;
            }

            // Go through each possible "pattern" that is OR'd together in the rule
            // definition and see if any of them match the substring starting with
            // the supplied index
            var patterns = Rules[rule];
            foreach (var pattern in patterns)
            {
                int nextIndex = index;
                bool patternValid = true;
                for (int i = 0; i < pattern.Count; i++)
                {
                    bool subRuleValid;
                    nextIndex = CheckRule(word, pattern[i], nextIndex, out subRuleValid);
                    if (!subRuleValid)
                    {
                        patternValid = false;
                        break;
                    }
                    nextIndex++;
                }
                // This pattern defined in the rule is valid, return
                if (patternValid)
                {
                    result = true;
                    // nextIndex already got incremented so we need to decrement it
                    // to inform the caller about the last index we checked
                    return nextIndex - 1;
                }
            }

            // This particular rule does not match the substring starting with
            // the supplied index, return
            result = false;
            return -1;
        }

        // Check how many consecutive times a rule can be applied to a word
        // from a given index
        private int CheckRuleRepeat(string word, int rule, int index, out int numRepeats)
        {
            var lastIndex = index;
            var count = 0;
            bool valid;
            do
            {
                var tempIndex = CheckRule(word, rule, lastIndex, out valid);
                if (valid)
                {
                    count++;
                    lastIndex = tempIndex + 1;
                }

            } while (valid);

            numRepeats = count;
            return lastIndex;
        }
    }
}