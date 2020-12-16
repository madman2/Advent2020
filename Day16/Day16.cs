using Advent2020.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent2020
{
    class Day16 : ISolver
    {
        public string FirstStarInputFile { get; } = "input.txt";
        public string SecondStarInputFile { get; } = "input.txt";

        public string SolveFirstStar(StreamReader reader)
        {
            var rulesMap = BuildRulesMap(reader);
            reader.ReadLine();

            var yourTicket = StringParsers.SplitDelimitedStringIntoIntList(reader.ReadLine(), ",".ToCharArray());
            reader.ReadLine();
            reader.ReadLine();

            var line = reader.ReadLine();
            long errorRate = 0;
            while (!string.IsNullOrWhiteSpace(line))
            {
                var nearbyTicket = StringParsers.SplitDelimitedStringIntoIntList(line, ",".ToCharArray());
                errorRate += GetTicketErrorRate(nearbyTicket, rulesMap);
                line = reader.ReadLine();
            }

            return errorRate.ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var rulesMap = BuildRulesMap(reader);
            reader.ReadLine();

            var yourTicket = StringParsers.SplitDelimitedStringIntoIntList(reader.ReadLine(), ",".ToCharArray());
            reader.ReadLine();
            reader.ReadLine();

            var line = reader.ReadLine();
            var nearbyTickets = new List<List<int>>();
            while (!string.IsNullOrWhiteSpace(line))
            {
                var nearbyTicket = StringParsers.SplitDelimitedStringIntoIntList(line, ",".ToCharArray());
                if (GetTicketErrorRate(nearbyTicket, rulesMap) == 0)
                {
                    nearbyTickets.Add(nearbyTicket);
                }
                line = reader.ReadLine();
            }

            var possibleIndices = GetPossibleIndices(nearbyTickets, rulesMap);
            var indexMap = MatchFieldsToIndices(possibleIndices);

            long productToReturn = 1;
            foreach (var field in indexMap.Keys)
            {
                if (field.StartsWith("departure"))
                {
                    var indexOfDepartureField = indexMap[field];
                    productToReturn *= yourTicket[indexOfDepartureField];
                }
            }

            return productToReturn.ToString();
        }

        private Dictionary<string, ISet<int>> BuildRulesMap(StreamReader reader)
        {
            var rulesMap = new Dictionary<string, ISet<int>>();
            var line = reader.ReadLine();
            while (!string.IsNullOrWhiteSpace(line))
            {
                var split = line.Split(": ");
                var key = split[0];
                var rules = split[1].Split(" or ");
                var validNumbers = new HashSet<int>();
                foreach (var rule in rules)
                {
                    var limits = rule.Split('-');
                    var lower = int.Parse(limits[0]);
                    var upper = int.Parse(limits[1]);
                    for (int i = lower; i <= upper; i++)
                    {
                        if (!validNumbers.Contains(i))
                        {
                            validNumbers.Add(i);
                        }
                    }
                }
                rulesMap.Add(key, validNumbers);
                line = reader.ReadLine();
            }

            return rulesMap;
        }

        private long GetTicketErrorRate(List<int> ticket, Dictionary<string, ISet<int>> rulesMap)
        {
            long errorRate = 0;
            foreach (var number in ticket)
            {
                bool validNumber = false;
                foreach (var rule in rulesMap.Keys)
                {
                    if (rulesMap[rule].Contains(number))
                    {
                        validNumber = true;
                        break;
                    }
                }
                if (!validNumber)
                {
                    errorRate += number;
                }
            }

            return errorRate;
        }

        private Dictionary<string, ISet<int>> GetPossibleIndices(List<List<int>> nearbyTickets, Dictionary<string, ISet<int>> rulesMap)
        {
            var possibleIndices = new Dictionary<string, ISet<int>>();
            foreach (var field in rulesMap.Keys)
            {
                possibleIndices[field] = new HashSet<int>();
                var indexCounts = new int[rulesMap.Count()];
                foreach (var ticket in nearbyTickets)
                {
                    for (int i = 0; i < rulesMap.Count(); i++)
                    {
                        var number = ticket[i];
                        if (rulesMap[field].Contains(number))
                        {
                            indexCounts[i]++;
                        }
                    }
                    for (int i = 0; i < rulesMap.Count(); i++)
                    {
                        if (indexCounts[i] == nearbyTickets.Count)
                        {
                            possibleIndices[field].Add(i);
                        }
                    }
                }
            }

            return possibleIndices;
        }

        private Dictionary<string, int> MatchFieldsToIndices(Dictionary<string, ISet<int>> possibleIndices)
        {
            var indexMap = new Dictionary<string, int>();
            var totalFields = possibleIndices.Count();

            while (indexMap.Count() < totalFields)
            {
                var chosenIndex = -1;
                foreach (var field in possibleIndices.Keys)
                {
                    if (possibleIndices[field].Count == 1)
                    {
                        chosenIndex = possibleIndices[field].First();
                        indexMap.Add(field, chosenIndex);
                    }
                }

                if (chosenIndex < 0)
                {
                    throw new Exception("Unable to match fields to indices");
                }

                foreach (var field in possibleIndices.Keys)
                {
                    possibleIndices[field].Remove(chosenIndex);
                }
            }

            return indexMap;
        }
    }
}