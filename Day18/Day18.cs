using Advent2020.Utils;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Advent2020
{
    class Day18 : ISolver
    {
        public string FirstStarInputFile { get; } = "input.txt";
        public string SecondStarInputFile { get; } = "input.txt";

        private readonly string ParenthesesRegex = @"\([ |\d|\*|\+]+\)";

        public string SolveFirstStar(StreamReader reader)
        {
            var expressions = StreamParsers.GetStreamAsStringList(reader);
            long sum = 0;
            foreach (var expression in expressions)
            {
                sum += EvaluateExpression(expression);
            }
            return sum.ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var expressions = StreamParsers.GetStreamAsStringList(reader);
            long sum = 0;
            foreach (var expression in expressions)
            {
                sum += EvaluateExpression(expression, '+');
            }
            return sum.ToString();
        }

        private long EvaluateExpression(string expression, char prioritizedOperation)
        {
            while (expression.IndexOf('(') >= 0)
            {
                var matches = Regex.Matches(expression, ParenthesesRegex);
                foreach (var match in matches.Cast<Match>().Reverse())
                {
                    var simplified = EvaluateSubExpression(match.Value.Substring(1, match.Length - 2), '+');
                    expression = expression.Replace(match.Value, simplified.ToString());
                }
            }

            return EvaluateSubExpression(expression, '+');
        }

        private long EvaluateExpression(string expression)
        {
            while (expression.IndexOf('(') >= 0)
            {
                var matches = Regex.Matches(expression, ParenthesesRegex);
                foreach (var match in matches.Cast<Match>().Reverse())
                {
                    var simplified = EvaluateSubExpression(match.Value.Substring(1, match.Length - 2));
                    expression = expression.Replace(match.Value, simplified.ToString());
                }
            }

            return EvaluateSubExpression(expression);
        }

        private long EvaluateSubExpression(string expression)
        {
            var expressionSplit = expression.Split(null);
            long result = -1;
            for (int i = 0; i < expressionSplit.Length; i += 2)
            {
                result = long.Parse(expressionSplit[i]);
                while (i < expressionSplit.Length - 2)
                {
                    var operation = expressionSplit[i + 1][0];
                    var nextOperand = long.Parse(expressionSplit[i + 2]);
                    if (operation == '+')
                    {
                        result += nextOperand;
                    }
                    else
                    {
                        result *= nextOperand;
                    }
                    i += 2;
                }
            }

            if (result < 0)
            {
                throw new Exception("Expression resulted in a negative number");
            }

            return result;
        }

        private long EvaluateSubExpression(string expression, char prioritizedOperation)
        {
            var expressionSplit = expression.Split(null);
            long result;
            List<long> firstPass = new List<long>();
            for (int i = 0; i < expressionSplit.Length; i += 2)
            {
                result = long.Parse(expressionSplit[i]);
                while (i < expressionSplit.Length - 2 && expressionSplit[i + 1][0] == prioritizedOperation)
                {
                    var operation = expressionSplit[i + 1][0];
                    var nextOperand = long.Parse(expressionSplit[i + 2]);
                    result = PerformOperation(result, nextOperand, operation);
                    i += 2;
                }
                firstPass.Add(result);
            }

            result = prioritizedOperation == '+' ? 1 : 0;
            foreach (var operand in firstPass)
            {
                result = PerformOperation(result, operand, prioritizedOperation == '+' ? '*' : '+');
            }

            if (result < 0)
            {
                throw new Exception("Expression resulted in a negative number");
            }

            return result;
        }

        private long PerformOperation(long op1, long op2, char operation)
        {
            switch (operation)
            {
                case '+':
                    return op1 + op2;
                case '*':
                    return op1 * op2;
            }

            throw new InvalidOperationException($"Invalid operation: {operation}");
        }
    }
}