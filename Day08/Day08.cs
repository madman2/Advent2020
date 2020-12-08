﻿using Advent2020.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Advent2020
{
    class Day08: ISolver
    {
        public string FirstStarInputFile { get; } = "input.txt";
        public string SecondStarInputFile { get; } = "input.txt";

        public string SolveFirstStar(StreamReader reader)
        {
            var instructionTextList = StreamParsers.GetStreamAsStringList(reader);
            var instructions = BuildInstructionList(instructionTextList);

            try
            {
                RunProgram(instructions, -1);
            }
            catch (InfiniteLoopException e)
            {
                return e.AccumulatorValue.ToString();
            }

            throw new Exception("Unable to detect loop");
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var instructionTextList = StreamParsers.GetStreamAsStringList(reader);
            var instructions = BuildInstructionList(instructionTextList);

            var jmpIndices = new List<int>();
            for (int i = 0; i < instructions.Count(); ++i)
            {
                if (instructions[i].Type == "jmp")
                    jmpIndices.Add(i);
            }

            int acc = -1;
            int jmpToSkipIndex = 0;
            while (acc == -1 && jmpToSkipIndex < jmpIndices.Count())
            {
                try
                {
                    acc = RunProgram(instructions, jmpIndices[jmpToSkipIndex]);
                }
                catch
                { 
                    // Expect to find many loops here, ignore
                }

                jmpToSkipIndex++;
            }

            return acc.ToString();
        }

        private List<Instruction> BuildInstructionList(List<string> instructionTextList)
        {
            var instructionsToReturn = new List<Instruction>();
            foreach (var instruction in instructionTextList)
            {
                var split = instruction.Split(new char[0]);
                var instructionName = split[0];
                var instructionArg = split[1];
                var instructionArgSign = instructionArg[0];
                var instructionArgVal = int.Parse(instructionArg.Substring(1));
                var newInstruction = new Instruction() { Type = instructionName, Value = (instructionArgSign == '-') ? -1 * instructionArgVal : instructionArgVal };
                instructionsToReturn.Add(newInstruction);
            }

            return instructionsToReturn;
        }

        private int RunProgram(List<Instruction> instructions, int jmpToIgnore)
        {
            int acc = 0;
            var InstructionsRun = new HashSet<int>();
            for (int i = 0; i < instructions.Count(); ++i)
            {
                if (InstructionsRun.Contains(i))
                    throw new InfiniteLoopException($"Instruction {i} is being executed again", acc);

                InstructionsRun.Add(i);

                var instructionToRun = instructions[i];
                switch (instructionToRun.Type)
                {
                    case "nop":
                        continue;
                    case "acc":
                        acc += instructionToRun.Value;
                        break;
                    case "jmp":
                        if (i != jmpToIgnore)
                            i += instructionToRun.Value - 1;
                        break;
                    default:
                        throw new ArgumentException();
                }
            }

            return acc;
        }

        private class Instruction
        {
            public string Type { get; set; }
            public int Value { get; set; }
        }

        private class InfiniteLoopException : Exception
        {
            public InfiniteLoopException(string message, int accumulatorValue)
                : base(message)
            {
                AccumulatorValue = accumulatorValue;
            }

            public int AccumulatorValue { get; }
        }
    }
}