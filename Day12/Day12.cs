using Advent2020.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Advent2020
{
    class Day12 : ISolver
    {
        public string FirstStarInputFile { get; } = "input.txt";
        public string SecondStarInputFile { get; } = "input.txt";

        private enum Version
        {
            Silver,
            Gold
        }

        private enum Direction
        {
            North = 0,
            East,
            South,
            West
        }

        private const int NumDirections = 4;

        public string SolveFirstStar(StreamReader reader)
        {
            var directions = StreamParsers.GetStreamAsStringList(reader);
            var ship = new Ship(0, 0, Direction.East);
            foreach (var direction in directions)
            {
                var action = direction[0];
                var param = int.Parse(direction.Substring(1));

                switch (action)
                {
                    case 'L':
                        ship.Rotate(param, false);
                        break;
                    case 'R':
                        ship.Rotate(param, true);
                        break;
                    case 'N':
                        ship.Move(Direction.North, param);
                        break;
                    case 'E':
                        ship.Move(Direction.East, param);
                        break;
                    case 'S':
                        ship.Move(Direction.South, param);
                        break;
                    case 'W':
                        ship.Move(Direction.West, param);
                        break;
                    case 'F':
                        ship.MoveForward(param);
                        break;
                }
            }

            return ship.GetManhattanDistance().ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var directions = StreamParsers.GetStreamAsStringList(reader);
            var ship = new Ship(0, 0);
            var waypoint = new Waypoint(10, 1);

            foreach (var direction in directions)
            {
                var action = direction[0];
                var param = int.Parse(direction.Substring(1));

                switch (action)
                {
                    case 'L':
                        waypoint.Rotate(param, false);
                        break;
                    case 'R':
                        waypoint.Rotate(param, true);
                        break;
                    case 'N':
                        waypoint.Move(Direction.North, param);
                        break;
                    case 'E':
                        waypoint.Move(Direction.East, param);
                        break;
                    case 'S':
                        waypoint.Move(Direction.South, param);
                        break;
                    case 'W':
                        waypoint.Move(Direction.West, param);
                        break;
                    case 'F':
                        ship.MoveTowardWaypoint(waypoint, param);
                        break;
                }
            }

            return ship.GetManhattanDistance().ToString();
        }

        private class Ship : NauticalPoint
        {
            private Direction currentDirection;

            public Ship(int initialX, int initialY, Direction intialDirection = Direction.North) : base(initialX, initialY)
            {
                this.currentDirection = intialDirection;
            }

            public void MoveForward(int distance)
            {
                Move(currentDirection, distance);
            }

            public void MoveTowardWaypoint(Waypoint waypoint, int multiplier)
            {
                X += waypoint.X * multiplier;
                Y += waypoint.Y * multiplier;
            }

            public override void Rotate(int degrees, bool clockwise)
            {
                var dir = clockwise ? 1 : -1;
                var turns = (degrees % 360) / 90;
                currentDirection = (Direction)(((int)currentDirection + NumDirections + dir * turns) % NumDirections);
            }
        }

        private class Waypoint : NauticalPoint
        {
            public Waypoint(int initialX, int initialY) : base(initialX, initialY)
            {
            }

            public override void Rotate(int degrees, bool clockwise)
            {
                var dir = clockwise ? 1 : -1;
                var turns = (degrees % 360) / 90;
                for (int i = 0; i < turns; i++)
                {
                    var tempX = Y * dir;
                    var tempY = -1 * X * dir;
                    X = tempX;
                    Y = tempY;
                }
            }
        }

        private abstract class NauticalPoint
        {
            public int X { get; set; }
            public int Y { get; set; }

            public NauticalPoint(int initialX, int initialY)
            {
                X = initialX;
                Y = initialY;
            }

            public void Move(Direction direction, int distance)
            {
                switch (direction)
                {
                    case Direction.North:
                        Y += distance;
                        break;
                    case Direction.West:
                        X -= distance;
                        break;
                    case Direction.South:
                        Y -= distance;
                        break;
                    case Direction.East:
                        X += distance;
                        break;
                }
            }

            public int GetManhattanDistance()
            {
                return Math.Abs(X) + Math.Abs(Y);
            }

            public abstract void Rotate(int degrees, bool clockwise);
        }
    }
}