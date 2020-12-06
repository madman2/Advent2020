using Advent2020.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Advent2020
{
    class Day04: ISolver
    {
        public string FirstStarInputFile { get; } = "input.txt";
        public string SecondStarInputFile { get; } = "input.txt";

        private readonly ISet<string> ValidEyeColors = new HashSet<string>() { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

        public string SolveFirstStar(StreamReader reader)
        {
            string line;
            int count = 0;
            while ((line = reader.ReadLine()) != null)
            {
                string passport = "";
                while (!string.IsNullOrWhiteSpace(line))
                {
                    passport += " " + line;
                    line = reader.ReadLine();
                }

                if (ValidatePassport(passport, 0))
                    count++;
            }

            return count.ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            string line;
            int count = 0;
            while ((line = reader.ReadLine()) != null)
            {
                string passport = "";
                while (!string.IsNullOrWhiteSpace(line))
                {
                    passport += " " + line;
                    line = reader.ReadLine();
                }

                if (ValidatePassport(passport, 1))
                    count++;
            }

            return count.ToString();
        }

        private bool ValidatePassport(string passport, int version)
        {
            var requiredFields = new HashSet<string>() { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
            var fields = StringParsers.SplitDelimitedStringIntoStringList(passport, new char[0]);

            foreach (var field in fields)
            {
                var fieldAndValue = field.Split(':');
                var fieldName = fieldAndValue[0];
                var value = fieldAndValue[1];

                if (ValidateField(fieldName, value, version))
                {
                    if (requiredFields.Contains(fieldName))
                        requiredFields.Remove(fieldName);
                }
            }

            return requiredFields.Count() == 0;
        }

        private bool ValidateField(string fieldName, string fieldValue, int version)
        {
            if (version == 0)
                return true;

            try
            {
                switch (fieldName)
                {
                    case "byr":
                        var birthYear = int.Parse(fieldValue);
                        return birthYear >= 1920 && birthYear <= 2002;
                    case "iyr":
                        var issueYear = int.Parse(fieldValue);
                        return issueYear >= 2010 && issueYear <= 2020;
                    case "eyr":
                        var expirationYear = int.Parse(fieldValue);
                        return expirationYear >= 2020 && expirationYear <= 2030;
                    case "hgt":
                        var units = fieldValue.Substring(fieldValue.Length - 2);
                        if (units == "cm")
                        {
                            var height = int.Parse(fieldValue.Substring(0, 3));
                            return height >= 150 && height <= 193;
                        }
                        else if (units == "in")
                        {
                            var height = int.Parse(fieldValue.Substring(0, 2));
                            return height >= 59 && height <= 76;
                        }
                        else
                        {
                            return false;
                        }
                    case "hcl":
                        if (fieldValue.ElementAt(0) != '#')
                            return false;
                        var rgb = Convert.ToUInt32(fieldValue.Substring(1), 16);
                        return true;
                    case "ecl":
                        return ValidEyeColors.Contains(fieldValue);
                    case "pid":
                        bool isNumber = true;
                        foreach (char c in fieldValue)
                        {
                            isNumber &= Char.IsNumber(c);
                        }
                        return isNumber && fieldValue.Length == 9;
                    default:
                        return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
