using System.IO;

namespace Advent2020
{
    interface ISolver
    {
        public string SolveFirstStar(StreamReader reader);
        public string SolveSecondStar(StreamReader reader);
    }
}
