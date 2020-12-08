using Advent2020.Utils;
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

        private List<int> SeenJmpIndices;

        public string SolveFirstStar(StreamReader reader)
        {
            var instructionTextList = StreamParsers.GetStreamAsStringList(reader);
            var instructions = BuildInstructionList(instructionTextList);

            try
            {
                RunProgram(instructions);
            }
            catch (InfiniteLoopException e)
            {
                return e.AccumulatorValue.ToString();
            }

            throw new Exception("No infinite loop detected");
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var instructionTextList = StreamParsers.GetStreamAsStringList(reader);
            var instructions = BuildInstructionList(instructionTextList);

            try
            {
                // Build up "seen" jmp indices
                RunProgram(instructions, true);
            }
            catch
            {
                // Expect infinite loop
            }

            int acc = -1;
            int jmpToSkipIndex = 0;
            while (acc == -1 && jmpToSkipIndex < SeenJmpIndices.Count())
            {
                try
                {
                    instructions[SeenJmpIndices[jmpToSkipIndex]].Type = OpCode.NOP;
                    acc = RunProgram(instructions);
                }
                catch
                {
                    // Expect to find many loops here, ignore
                }
                finally
                {
                    instructions[SeenJmpIndices[jmpToSkipIndex]].Type = OpCode.JMP;
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
                OpCode opCode;
                switch (split[0])
                {
                    case "nop":
                        opCode = OpCode.NOP;
                        break;
                    case "acc":
                        opCode = OpCode.ACC;
                        break;
                    case "jmp":
                        opCode = OpCode.JMP;
                        break;
                    default:
                        throw new ArgumentException($"Instruction contains an invalid OpCode: {split[0]}");
                }
                var instructionArgVal = int.Parse(split[1]);
                var newInstruction = new Instruction() { Type = opCode, Value = instructionArgVal };
                instructionsToReturn.Add(newInstruction);
            }

            return instructionsToReturn;
        }

        private int RunProgram(List<Instruction> instructions, bool trackJmpIndices = false)
        {
            // If jmpToIgnore < 0, track "seen" jmp indices
            if (trackJmpIndices)
                SeenJmpIndices = new List<int>();

            int acc = 0;
            int ip = 0;
            var InstructionsRun = new HashSet<int>();
            while (ip < instructions.Count())
            {
                if (InstructionsRun.Contains(ip))
                    throw new InfiniteLoopException($"Instruction {ip} is being executed again", acc);

                InstructionsRun.Add(ip);

                var instructionToRun = instructions[ip];
                switch (instructionToRun.Type)
                {
                    case OpCode.NOP:
                        ip++;
                        break;
                    case OpCode.ACC:
                        acc += instructionToRun.Value;
                        ip++;
                        break;
                    case OpCode.JMP:
                        if (trackJmpIndices)
                            SeenJmpIndices.Add(ip);
                        ip += instructionToRun.Value;
                        break;
                    default:
                        throw new InvalidEnumArgumentException($"Unable to handle OpCode: {instructionToRun.Type}");
                };
            }

            return acc;
        }

        private enum OpCode
        {
            NOP,
            ACC,
            JMP
        }

        private class Instruction
        {
            public OpCode Type { get; set; }
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