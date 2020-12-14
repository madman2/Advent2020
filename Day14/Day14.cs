using Advent2020.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace Advent2020
{
    class Day14 : ISolver
    {
        public string FirstStarInputFile { get; } = "input.txt";
        public string SecondStarInputFile { get; } = "input.txt";

        private const string MemWriteRegexString = @"mem\[(\d+)\] = (\d+)";

        public string SolveFirstStar(StreamReader reader)
        {
            var lines = StreamParsers.GetStreamAsStringList(reader);
            var memory = new Dictionary<long, long>();

            string mask = "";
            foreach (var line in lines)
            {
                if (line.Substring(0, 4) == "mask")
                {
                    mask = line.Split(" = ")[1];
                }
                else
                {
                    var match = Regex.Match(line, MemWriteRegexString);
                    var address = int.Parse(match.Groups[1].Value);
                    long value = Int64.Parse(match.Groups[2].Value);
                    var valueAsBinaryString = LongIntToBinaryString(value, 36);
                    var modifiedValue = "";
                    for (int i = 0; i < mask.Length; i++)
                    {
                        if (mask[i] == '1')
                        {
                            modifiedValue += '1';
                        }
                        else if (mask[i] == '0')
                        {
                            modifiedValue += '0';
                        }
                        else
                        {
                            modifiedValue += valueAsBinaryString[i];
                        }
                    }
                    long modifiedValueAsInt = Convert.ToInt64(modifiedValue, 2);
                    memory[address] = modifiedValueAsInt;
                }
            }

            return memory.Values.Sum().ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var lines = StreamParsers.GetStreamAsStringList(reader);
            var memory = new Dictionary<long, long>();
            List<List<int>> maskList = null;

            foreach (var line in lines)
            {
                if (line.Substring(0, 4) == "mask")
                {
                    var mask = line.Split(" = ")[1];
                    maskList = UpdateMaskList(mask);
                    //PrintMaskLists(maskList);
                }
                else
                {
                    var match = Regex.Match(line, MemWriteRegexString);
                    long address = Int64.Parse(match.Groups[1].Value);
                    long value = Int64.Parse(match.Groups[2].Value);
                    var addressAsBinaryString = LongIntToBinaryString(address, 36);
                    foreach (var mask in maskList)
                    {
                        var modifiedAddress = "";
                        for (int i = 0; i < 36; i++)
                        {
                            if (mask[i] == 1)
                            {
                                modifiedAddress += '1';
                            }
                            else if (mask[i] == 0)
                            {
                                modifiedAddress += '0';
                            }
                            else
                            {
                                // This is the case where the mask bit == -1, preserve original bit
                                modifiedAddress += addressAsBinaryString[i];
                            }
                        }
                        long actualAddress = Convert.ToInt64(modifiedAddress, 2);
                        memory[actualAddress] = value;
                    }
                }
            }

            return memory.Values.Sum().ToString();
        }

        private string LongIntToBinaryString(long number, int bits)
        {
            var result = Convert.ToString(number, 2);
            var padding = "";
            for (int i = 0; i < bits - result.Length; i++)
            {
                padding += "0";
            }
            return padding + result;
        }

        // Returns list of all permutations of masks given a mask containing floating bits
        // 0 => Overwrite with a 0
        // 1 => Overwrite with a 1
        // -1 => Keep original bit value
        private List<List<int>> UpdateMaskList(string mask)
        {
            var newMaskList = new List<List<int>>();
            foreach (char c in mask)
            {
                if (c == 'X')
                {
                    if (newMaskList.Count() == 0)
                    {
                        var zeroMask = new List<int>() { 0 };
                        var oneMask = new List<int>() { 1 };
                        newMaskList.Add(zeroMask);
                        newMaskList.Add(oneMask);
                    }
                    else
                    {
                        var newMasks = new List<List<int>>();
                        foreach (var m in newMaskList)
                        {
                            var newMask = new List<int>(m);
                            m.Add(0);
                            newMask.Add(1);
                            newMasks.Add(newMask);
                        }
                        foreach (var m in newMasks)
                        {
                            newMaskList.Add(m);
                        }
                    }
                }
                else
                {
                    if (newMaskList.Count() == 0)
                    {
                        var firstMask = new List<int>() { c == '1' ? 1 : -1 };
                        newMaskList.Add(firstMask);
                    }
                    else
                    {
                        foreach (var m in newMaskList)
                        {
                            m.Add(c == '1' ? 1 : -1);
                        }
                    }
                }
            }

            return newMaskList;
        }

        private void PrintMaskLists(List<List<int>> maskLists)
        {
            foreach (var mask in maskLists)
            {
                var maskString = "";
                foreach (int c in mask)
                {
                    maskString += c;
                }
                Console.WriteLine(maskString);
            }
        }
    }
}