using Advent2020.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Advent2020
{
    class Day21 : ISolver
    {
        public string FirstStarInputFile { get; } = "input.txt";
        public string SecondStarInputFile { get; } = "input.txt";

        Dictionary<string, ISet<string>> PossibleIngredients = new Dictionary<string, ISet<string>>();
        Dictionary<string, int> IngredientCounts = new Dictionary<string, int>();

        public string SolveFirstStar(StreamReader reader)
        {
            var lines = StreamParsers.GetStreamAsStringList(reader);
            foreach (var line in lines)
            {
                var split = line.Split(" (contains ");
                var ingredients = split[0].Split(null);
                var allergens = split[1].Substring(0, split[1].Length - 1).Split(", ");

                foreach (var allergen in allergens)
                {
                    if (!PossibleIngredients.ContainsKey(allergen))
                    {
                        PossibleIngredients.Add(allergen, new HashSet<string>());
                        foreach (var ingredient in ingredients)
                        {
                            PossibleIngredients[allergen].Add(ingredient);
                        }
                    }
                    else
                    {
                        PossibleIngredients[allergen].IntersectWith(ingredients);
                    }
                }

                foreach (var ingredient in ingredients)
                {
                    if (IngredientCounts.ContainsKey(ingredient))
                    {
                        IngredientCounts[ingredient]++;
                    }
                    else
                    {
                        IngredientCounts[ingredient] = 1;
                    }
                }
            }

            var allIngredients = new HashSet<string>(IngredientCounts.Keys);
            foreach (var allergen in PossibleIngredients.Keys)
            {
                foreach (var ingredient in PossibleIngredients[allergen])
                {
                    allIngredients.Remove(ingredient);
                }
            }

            int count = 0;
            foreach (var ingredient in allIngredients)
            {
                count += IngredientCounts[ingredient];
            }

            return count.ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var lines = StreamParsers.GetStreamAsStringList(reader);
            foreach (var line in lines)
            {
                var split = line.Split(" (contains ");
                var ingredients = split[0].Split(null);
                var allergens = split[1].Substring(0, split[1].Length - 1).Split(", ");

                foreach (var allergen in allergens)
                {
                    if (!PossibleIngredients.ContainsKey(allergen))
                    {
                        PossibleIngredients.Add(allergen, new HashSet<string>());
                        foreach (var ingredient in ingredients)
                        {
                            PossibleIngredients[allergen].Add(ingredient);
                        }
                    }
                    else
                    {
                        PossibleIngredients[allergen].IntersectWith(ingredients);
                    }
                }
            }

            var allergenToIngredient = new Dictionary<string, string>();
            while (allergenToIngredient.Count() < PossibleIngredients.Count())
            {
                foreach (var allergen in PossibleIngredients.Keys)
                {
                    if (PossibleIngredients[allergen].Count == 1)
                    {
                        var ingredient = PossibleIngredients[allergen].First();
                        allergenToIngredient.Add(allergen, ingredient);
                        foreach (var ingredients in PossibleIngredients.Values)
                        {
                            ingredients.Remove(ingredient);
                        }
                    }
                }
            }

            var a = allergenToIngredient.Keys.ToList<string>();
            a.Sort();
            var result = "";
            foreach (var allergen in a)
            {
                result += allergenToIngredient[allergen] + ",";
            }

            return result.Substring(0, result.Length - 1);
        }
    }
}