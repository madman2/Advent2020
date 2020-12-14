using Advent2020.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent2020
{
    class Day13 : ISolver
    {
        public string FirstStarInputFile { get; } = "input.txt";
        public string SecondStarInputFile { get; } = "input.txt";

        public string SolveFirstStar(StreamReader reader)
        {
            var estimate = int.Parse(reader.ReadLine());
            var nextLine = reader.ReadLine();
            var buses = StringParsers.SplitDelimitedStringIntoStringList(nextLine, ",".ToCharArray());
            long shortestWait = Int64.MaxValue;
            int closestBus = 0;
            foreach (var bus in buses)
            {
                if (bus == "x")
                    continue;
                var busInterval = int.Parse(bus);
                // Effectively computes -estimate % busInterval, picks the smallest positive unique solution
                var waitTime = (-estimate % busInterval + busInterval) % busInterval;
                if (waitTime < shortestWait)
                {
                    shortestWait = waitTime;
                    closestBus = busInterval;
                }
            }

            return (shortestWait * closestBus).ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            // Ignore first line of file
            reader.ReadLine();

            var nextLine = reader.ReadLine();
            var buses = StringParsers.SplitDelimitedStringIntoStringList(nextLine, ",".ToCharArray());
            
            var busIntervals = new List<long>();
            var busDepartures = new List<int>();
            for (int i = 0; i < buses.Count(); i++)
            {
                var busInterval = buses[i];
                if (busInterval != "x")
                {
                    busIntervals.Add(uint.Parse(busInterval));
                    busDepartures.Add(i);
                }
            }

            // We have a list of bus intervals that looks like this:
            // busIntervals = [ n0, n1, n2, ... ]
            // We also have a list of required departure times for each bus (relative to the departure of bus n0):
            // busDepartures = [ 0, 1, 5, ... ]
            // This can be interpreted as n1 leaves at time t=0, n2 leaves at time t=1, n3 leaves at time t=5...

            // We can generalize this to say:
            // busIntervals[i] leaves busDepartures[i] minutes after bus n0, for 0 <= i < busIntervals.Count();

            // For a given n0 departure time t, we can compute the wait time from t to the departure of bus[i] as follows:
            // actualDeparture[0] = 0
            // actualDeparture[i] = -t mod busIntervals[i]

            // We are looking for an n0 departure time t such that -t = busDepartures[i] (mod busIntervals[i])
            // This problem is a system of congruence equations of the form x = a[i] (mod n[i]) for 0 <= i <= busIntervals.Count()

            var solution = SolveSystemOfCongruenceEquations(busIntervals, busDepartures);

            // The solution we get back is the single unique solution that exists in the interval (-N, 0)
            // where N is defined below. The way we defined the problem, the solution to this system is equivalent to -t,
            // so to find that actual value of t we need to multiply by -1
            return (-solution).ToString();
        }

        // Finds x s.t. x = a[i] (mod n[i]) for all 0 <= i < n.Count()
        // Assumption: n.Values is pairwise relatively prime
        // Solution below effectively derives the Chinese Remainder Theorem and uses that to solve the problem
        // https://www.dave4math.com/mathematics/chinese-remainder-theorem
        private long SolveSystemOfCongruenceEquations(List<long> n, List<int> a)
        {
            // Let k = n.Count()
            // Let N = n[0] * n[1] * ... * n[k - 1]
            // Let N[i] = N / n[i] for all 0 <= i < k
            var k = n.Count();
            var N = new long[k];
            long prod = 1;
            foreach (var ni in n)
            {
                prod *= ni;
            }

            for (int i = 0; i < k; i++)
            {
                N[i] = prod / n[i];
            }

            // We know gcd(N[i], n[i]) = 1 for all 0 <= i < k,
            // because if there exists some integer d that divides N[i] and n[i],
            // d would need to be able to divide both n[i] and n[j], where we know
            // n[j] is a factor of N[i]. Due to all pairs in n being relatively prime,
            // we know d == 1, so gcd(N[i], n[i]) = 1

            // From Bezout's Identity:
            // If gcd(m, n) > 0, then gcd(m, n) = a*m + b*n for some integers a, b
            // We can use this to claim that for all (N[i], n[i]), there exist
            // integers a[i], b[i] s.t. a[i] * N[i] + b[i] * n[i] = 1 for all 0 <= i < k
            // https://www.dave4math.com/mathematics/greatest-common-divisors/

            // We can use this to show that there must exist some x[i] s.t. N[i] * x[i] = 1 (mod n[i])
            // We can claim that x[i] * N[i] and 1 are congruent mod n[i] if we can show that (x[i] * N[i] - 1) is divisible by n[i]
            // If we replace ai above with x[i], then we have (x[i] * N[i]) - 1 = -b[i] * n[i].
            // Because -b[i] * n[i] is divisible by n[i], then we have shown that the x[i] defined above does exist for all 0 <= i < k

            // (1) Now we have x[i] * N[i] = 1 (mod n[i]) for all 0 <= i < k
            // (2) We can also make the claim x[i] * N[i] = 0 (mod n[j]) for all j != i
            // This claim holds up because basically we know from the definition of N[i] that N[i] is divisible by all n[j] s.t. j != i

            // Remember that the system we are trying to solve is:
            // x = i (mod n[i]) for all 0 <= i < k
            // Let's consider x = (x[0] * N[0] * a[0]) + (x[1] * N[1] * a[1]) + ... + (x[k - 1] * N[k - 1] * a[k - 1])
            // From the two claims (1) and (2) above, we can see that x = a[i] (mod n[i]), for all 0 <= i < k
            // This is because when we take (x[0] * N[0] * a[0]) + (x[1] * N[1] * a[1]) + ... + (x[k - 1] * N[k - 1] * a[k - 1]) (mod n[i]),
            // the term containing x[i] * N[i] is congruent to a[i] (mod n[i]) and the rest of the terms are congruent to 0 (mod n[i]) and disappear

            // This solution x = a[i] (mod n[i]) for all 0 <= i < k is exactly what we are trying to solve!
            // Now we simply need to solve for x

            // To do this, we first need to find x[] s.t. x[i] * N[i] = 1 (mod n[i])
            // This requires us to compute the modular multiplicative inverse of N[i] mod n[i]
            var x = new long[k];
            for (int i = 0; i < k; i++)
            {
                x[i] = ModularMultiplicativeInverse(N[i], n[i]);
            }

            // Now, we just need to compute x = (x[0] * N[0] * a[i]) + (x[1] * N[1] * a[1]) + ... + (x[k - 1] * N[k - 1] * a[k - 1])
            long X = 0;
            for (int i = 0; i < k; i++)
            {
                long temp = x[i] * N[i] * a[i];
                X += temp;
            }

            // There are many possible unique solutions for this problem. For our case, we want
            // the unique solution between (-N, 0)
            while (X < -prod)
            {
                X += prod;
            }

            while (X > 0)
            {
                X -= prod;
            }

            return X;
        }

        // To solve x * N = 1 (mod n) for x, we need to compute the modular multiplicative inverse N (mod n)
        // If N and n are coprime, we can use the Extended Euclidian algorithm to compute this
        // https://www.dave4math.com/mathematics/euclidean-algorithm/

        // The Extended Euclidian algorithm allows us to use gcd() computation to solve our equation
        // We know gcd(N, n) can always be represented as N * x + n * b for some integers x, b
        // Because N and n are coprime, we can write N * x + n * b = 1
        // Taking both sides mod n leaves: N * x = 1 (mod n)

        // If we compute the gcd(N, n) by finding x and b, then in the process we will compute
        // the x coefficient, which is our modular multiplicative inverse
        private long ModularMultiplicativeInverse(long N, long n)
        {
            long x = 1;
            long b = 0;
            long n0 = n;

            if (n == 1)
            {
                return 0;
            }

            while (N > 1)
            {
                long quotient = N / n;
                long temp = n;

                // n is now the remainder
                n = N % n;
                N = temp;
                temp = b;

                // Update x and b
                b = x - quotient * b;
                x = temp;
            }

            if (x < 0)
            {
                x += n0;
            }

            return x;
        }
    }
}