﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent2020
{
    public class Day02 : ISolver
    {
        private const int TargetSum = 2020;

        public string SolveFirstStar(StreamReader reader)
        {
            int count = 0;

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var ruleAndPassword = line.Split(':');
                var rule = ruleAndPassword[0].Trim();
                var password = ruleAndPassword[1].Trim();
                if (validatePassword(rule, password, 1))
                    count++;
            }

            return count.ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            int count = 0;

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var ruleAndPassword = line.Split(':');
                var rule = ruleAndPassword[0].Trim();
                var password = ruleAndPassword[1].Trim();
                if (validatePassword(rule, password, 2))
                    count++;
            }

            return count.ToString();
        }

        private bool validatePassword(string rule, string password, int version)
        {
            var limitsAndLetter = rule.Split(null);
            var limits = limitsAndLetter[0];
            var letter = limitsAndLetter[1][0];

            var minAndMax = limits.Split('-');
            var min = int.Parse(minAndMax[0]);
            var max = int.Parse(minAndMax[1]);

            if (version == 1)
            {
                return validatePasswordPart1(min, max, letter, password);
            }

            return validatePasswordPart2(min, max, letter, password);
        }

        private bool validatePasswordPart1(int min, int max, char letter, string password)
        {
            var occurrences = password.Where(c => (c == letter)).Count();
            return occurrences >= min && occurrences <= max;
        }

        private bool validatePasswordPart2(int index1, int index2, char letter, string password)
        {
            return password[index1 - 1] == letter ^ password[index2 - 1] == letter;
        }


    }
}