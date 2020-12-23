using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent2020
{
    class Day23 : ISolver
    {
        public string FirstStarInputFile { get; } = "input.txt";
        public string SecondStarInputFile { get; } = "input.txt";

        private const int PartOneIterations = 100;
        private const int PartTwoIterations = 10000000;
        private const int PartTwoMaxCupValue = 1000000;

        public string SolveFirstStar(StreamReader reader)
        {
            var cupInput = reader.ReadLine();

            var finalCuplist = GetFinalCupList(cupInput, PartOneIterations);

            string result = "";
            var oneNode = finalCuplist.Find(1);
            var iteratorNode = oneNode.Next ?? finalCuplist.First;

            while (iteratorNode.Value != 1)
            {
                result += iteratorNode.Value;
                iteratorNode = iteratorNode.Next ?? finalCuplist.First;
            }

            return result;
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var cupInput = reader.ReadLine();

            var finalCuplist = GetFinalCupList(cupInput, PartTwoIterations, PartTwoMaxCupValue);

            var oneNode = finalCuplist.Find(1);
            var oneNodeNext = oneNode.Next ?? finalCuplist.First;
            var oneNodeNextNext = oneNodeNext.Next ?? finalCuplist.First;

            long product = (long)oneNodeNext.Value * oneNodeNextNext.Value;

            return product.ToString();
        }

        private LinkedList<int> GetFinalCupList(string cupInput, int iterations, int maxCupValue = -1)
        {
            var cupList = new LinkedList<int>();
            var cupLookup = new Dictionary<int, LinkedListNode<int>>();

            foreach (var n in cupInput)
            {
                cupList.AddLast(int.Parse(n.ToString()));
            }

            if (maxCupValue > cupList.Max())
            {
                for (int i = cupList.Max() + 1; i <= maxCupValue; i++)
                {
                    cupList.AddLast(i);
                }
            }
            else
            {
                maxCupValue = cupList.Max();
            }

            var minCupValue = cupList.Min();

            var iteratorNode = cupList.First;
            while (iteratorNode != null)
            {
                cupLookup.Add(iteratorNode.Value, iteratorNode);
                iteratorNode = iteratorNode.Next;
            }

            int moves = 0;
            var currentCupNode = cupList.First;
            while (moves < iterations)
            {
                moves++;

                LinkedListNode<int> destinationCupNode = null;

                var targetDestinationCupValue = currentCupNode.Value - 1;
                while (destinationCupNode == null)
                {
                    if (targetDestinationCupValue < minCupValue)
                    {
                        targetDestinationCupValue = maxCupValue;
                    }
                    destinationCupNode = cupLookup[targetDestinationCupValue];

                    iteratorNode = currentCupNode;
                    for (int i = 0; i < 3; i++)
                    {
                        iteratorNode = iteratorNode.Next ?? cupList.First;
                        if (iteratorNode.Value == targetDestinationCupValue)
                        {
                            destinationCupNode = null;
                            if (targetDestinationCupValue == minCupValue)
                            {
                                targetDestinationCupValue = maxCupValue;
                            }
                            else
                            {
                                targetDestinationCupValue--;
                            }
                            break;
                        }
                    }
                }

                var removedCups = new List<LinkedListNode<int>>();
                iteratorNode = currentCupNode;
                for (int i = 0; i < 3; i++)
                {
                    iteratorNode = iteratorNode.Next ?? cupList.First;
                    removedCups.Add(iteratorNode);
                }

                foreach (var node in removedCups)
                {
                    cupList.Remove(node);
                }

                iteratorNode = destinationCupNode;
                for (int i = 0; i < 3; i++)
                {
                    cupList.AddAfter(iteratorNode, removedCups[i]);
                    iteratorNode = iteratorNode.Next;
                }

                currentCupNode = currentCupNode.Next ?? cupList.First;
            }

            return cupList;
        }
    }
}