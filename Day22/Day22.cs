using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent2020
{
    class Day22 : ISolver
    {
        public string FirstStarInputFile { get; } = "input.txt";
        public string SecondStarInputFile { get; } = "input.txt";

        private readonly string PlayerNumberRegex = @"Player (\d+):";

        public string SolveFirstStar(StreamReader reader)
        {
            var playerHands = new Dictionary<int, Queue<int>>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var playerNumber = int.Parse(Regex.Match(line, PlayerNumberRegex).Groups[1].Value);
                playerHands.Add(playerNumber, new Queue<int>());
                while (!string.IsNullOrWhiteSpace(line = reader.ReadLine()))
                {
                    playerHands[playerNumber].Enqueue(int.Parse(line));
                }
            }

            while (!playerHands.Values.Any(cards => cards.Count == 0))
            {
                var thisHand = new Dictionary<int, int>();
                foreach (var player in playerHands.Keys)
                {
                    thisHand[player] = playerHands[player].Dequeue();
                }
                var winner = thisHand.FirstOrDefault(x => x.Value == thisHand.Values.Max()).Key;
                var loser = thisHand.Keys.FirstOrDefault(x => x != winner);

                playerHands[winner].Enqueue(thisHand[winner]);
                playerHands[winner].Enqueue(thisHand[loser]);
            }

            var winningHand = playerHands.Values.FirstOrDefault(x => x.Count > 0);
            return ComputeHandScore(winningHand).ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var playerHands = new Dictionary<int, List<int>>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var playerNumber = int.Parse(Regex.Match(line, PlayerNumberRegex).Groups[1].Value);
                playerHands.Add(playerNumber, new List<int>());
                while (!string.IsNullOrWhiteSpace(line = reader.ReadLine()))
                {
                    playerHands[playerNumber].Add(int.Parse(line));
                }
            }

            var winner = -1;
            while (!playerHands.Values.Any(cards => cards.Count == 0))
            {
                winner = RecursiveCombat(playerHands[1], playerHands[2]);
            }
            var winningCards = playerHands[winner];

            return ComputeHandScore(winningCards).ToString();
        }

        private int RecursiveCombat(List<int> p1, List<int> p2)
        {
            var previousHands = new HashSet<(int p1, int p2)>();
            while (p1.Count > 0 && p2.Count > 0)
            {
                var p1Hand = ComputeHandScore(p1);
                var p2Hand = ComputeHandScore(p2);

                if (previousHands.Contains((p1Hand, p2Hand)))
                {
                    return 1;
                }

                previousHands.Add((p1Hand, p2Hand));

                var p1Card = p1.First();
                p1.RemoveAt(0);
                var p2Card = p2.First();
                p2.RemoveAt(0);

                int winner;
                if (p1Card > p1.Count || p2Card > p2.Count)
                {
                    winner = p1Card > p2Card ? 1 : 2;
                }
                else
                {
                    winner = RecursiveCombat(p1.Take(p1Card).ToList(), p2.Take(p2Card).ToList());
                }

                var winningHand = (winner == 1) ? p1 : p2;
                winningHand.Add((winner == 1) ? p1Card : p2Card);
                winningHand.Add((winner == 1) ? p2Card : p1Card);
            }

            return p1.Count > 0 ? 1 : 2;
        }

        private int ComputeHandScore(IEnumerable<int> hand)
        {
            int multiplier = hand.Count();
            int score = 0;
            foreach (var card in hand)
            {
                score += multiplier * card;
                multiplier--;
            }

            return score;
        }
    }
}