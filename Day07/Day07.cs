using Advent2020.Utils;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Reflection.Metadata.Ecma335;

namespace Advent2020
{
    class Day07: ISolver
    {
        public string FirstStarInputFile { get; } = "input.txt";
        public string SecondStarInputFile { get; } = "input.txt";

        private Dictionary<string, List<ValueTuple<int, string>>> BagGraph;
        private const string ShinyGoldBagColor = "shiny gold";

        public string SolveFirstStar(StreamReader reader)
        {
            var listOfDefinitions = StreamParsers.GetStreamAsStringList(reader);
            BuildBagGraph(listOfDefinitions);

            int bagCount = 0;
            foreach (var bagColor in BagGraph.Keys)
            {
                if (ContainsBag(bagColor, ShinyGoldBagColor))
                    bagCount++;
            }

            // Includes shiny gold bag, subtract one for answer
            return (bagCount - 1).ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var listOfDefinitions = StreamParsers.GetStreamAsStringList(reader);
            BuildBagGraph(listOfDefinitions);

            // Includes shiny gold bag, subtract one for final answer
            int bagCount = CountBags(ShinyGoldBagColor);

            return (bagCount - 1).ToString();
        }

        private bool ContainsBag(string startBag, string endBag)
        {
            if (startBag == endBag)
                return true;

            foreach (var subBag in BagGraph[startBag])
            {
                if (ContainsBag(subBag.Item2, endBag))
                    return true;
            }

            return false;
        }

        private int CountBags(string startBag)
        {
            if (BagGraph[startBag].Count == 0)
                return 1;

            int sumOfSubBags = 1;
            foreach (var subBag in BagGraph[startBag])
            {
                sumOfSubBags += (subBag.Item1 * CountBags(subBag.Item2));
            }

            return sumOfSubBags;
        }

        private void BuildBagGraph(List<string> listOfDefinitions)
        {
            BagGraph = new Dictionary<string, List<ValueTuple<int, string>>>();
            foreach (var definition in listOfDefinitions)
            {
                var bagAndSubBags = definition.Split("contain");
                var subBags = StringParsers.SplitDelimitedStringIntoStringList(bagAndSubBags[1], new char[] { ',' });

                var listOfRules = new List<ValueTuple<int, string>>();
                if (bagAndSubBags[1].Trim() != "no other bags.")
                {
                    foreach (var subBag in subBags)
                    {
                        listOfRules.Add(CreateBagNode(subBag));
                    }
                }

                BagGraph.Add(GetBagColor(bagAndSubBags[0]), listOfRules);
            }
        }

        private ValueTuple<int, string> CreateBagNode(string bagRule)
        {
            var ruleToReturn = new ValueTuple<int, string>();
            var splitRule = bagRule.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            ruleToReturn.Item1 = int.Parse(splitRule[0]);
            var type = "";
            for (int i = 1; i < splitRule.Length - 1; ++i)
            {
                type += " " + splitRule[i].Trim();
            }
            ruleToReturn.Item2 = type.Trim();
            return ruleToReturn;
        }

        private string GetBagColor(string bag)
        {
            var bagWords = bag.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            var stringToReturn = "";
            for (int i = 0; i < bagWords.Length - 1; ++i)
            {
                stringToReturn += " " + bagWords[i].Trim();
            }

            return stringToReturn.Trim();
        }
    }
}