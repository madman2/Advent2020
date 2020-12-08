using Advent2020.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent2020
{
    class Day07: ISolver
    {
        public string FirstStarInputFile { get; } = "input.txt";
        public string SecondStarInputFile { get; } = "input.txt";

        private Dictionary<string, Bag> BagList;
        private const string ShinyGoldBagColor = "shiny gold";

        public string SolveFirstStar(StreamReader reader)
        {
            var listOfDefinitions = StreamParsers.GetStreamAsStringList(reader);
            BuildBagList(listOfDefinitions);

            var parentBags = new HashSet<string>();
            GetParentBags(ShinyGoldBagColor, parentBags);

            // Includes shiny gold bag, subtract one for answer
            return (parentBags.Count() - 1).ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var listOfDefinitions = StreamParsers.GetStreamAsStringList(reader);
            BuildBagList(listOfDefinitions);

            // Includes shiny gold bag, subtract one for final answer
            int bagCount = CountInnerBags(ShinyGoldBagColor);

            return (bagCount - 1).ToString();
        }

        private void GetParentBags(string startBag, ISet<string> parentBags)
        {
            parentBags.Add(startBag);
            foreach (var parentBag in BagList[startBag].ParentColors)
            {
                GetParentBags(parentBag, parentBags);
            }
        }

        private int CountInnerBags(string startBag)
        {
            if (BagList[startBag].InnerBags.Count == 0)
                return 1;

            int numInnerBags = 1;
            foreach (var innerBag in BagList[startBag].InnerBags)
            {
                numInnerBags += (innerBag.Item1 * CountInnerBags(innerBag.Item2));
            }

            return numInnerBags;
        }

        private void BuildBagList(List<string> listOfDefinitions)
        {
            BagList = new Dictionary<string, Bag>();
            foreach (var definition in listOfDefinitions)
            {
                var bagAndInnerBags = definition.Split("bags contain").Select(x => x.Trim()).ToList();
                var currentBagColor = bagAndInnerBags[0];
                var innerBags = StringParsers.SplitDelimitedStringIntoStringList(bagAndInnerBags[1], ",".ToCharArray());

                var currentBag = GetOrCreateBag(currentBagColor);
                if (bagAndInnerBags[1] != "no other bags.")
                {
                    foreach (var innerBagDescription in innerBags)
                    {
                        var innerBagTuple = CreateBagTuple(innerBagDescription);
                        currentBag.InnerBags.Add(innerBagTuple);
                        var innerBag = GetOrCreateBag(innerBagTuple.Item2);
                        innerBag.ParentColors.Add(currentBagColor);
                    }
                }
            }
        }

        private Bag GetOrCreateBag(string color)
        {
            if (BagList.ContainsKey(color))
                return BagList[color];

            var newBag = new Bag(color);
            BagList[color] = newBag;
            return newBag;
        }

        private ValueTuple<int, string> CreateBagTuple(string bagRule)
        {
            var ruleToReturn = new ValueTuple<int, string>();
            var splitRule = bagRule.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            ruleToReturn.Item1 = int.Parse(splitRule[0]);
            var color = "";
            for (int i = 1; i < splitRule.Length - 1; ++i)
            {
                color += " " + splitRule[i];
            }
            ruleToReturn.Item2 = color.Trim();
            return ruleToReturn;
        }

        private class Bag
        {
            public Bag(string color)
            {
                Color = color;
                ParentColors = new HashSet<string>();
                InnerBags = new List<ValueTuple<int, string>>();
            }

            public string Color { get; }
            public ISet<string> ParentColors { get; set; }
            public List<ValueTuple<int, string>> InnerBags { get; set; }
        }
    }
}