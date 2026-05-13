using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.PrimeTeleportation
{
    public class Solution
    {
        // get max number - nMax- O(n)
        // get all prime numbers in the prime array - O(n)*O(sqrt(N)) - sieve of eratpsthenes ?
        // for each number in the prime array, populate primeNumber factors location to primesDict- O(n)*O(numPrimes) - dictionary <int primenumber, (HashSet<int> locations)>
        // for each number in primesArray - get the minimum (mn_l) and max (mx_l) location using primesDict
        // calculate min(mn_l + n-mx_l) and return

        private HashSet<int> sieve_of_eratosthenes(int n)
        {
            bool[] is_prime = new bool[n+1];
            Array.Fill(is_prime, true);
            is_prime[0] = is_prime[1] = false;
            for (int x = 2; x < Math.Sqrt(n) + 1; x++)
            {
                if (is_prime[x])
                {
                    for (int y = x * x; y <= n; y += x)
                    {
                        is_prime[y] = false;
                    }
                }
            }
            HashSet<int> coll = new HashSet<int>();
            for (int x = 2; x <= n; x++)
            {
                if (is_prime[x])
                {
                    coll.Add(x);
                }
            }
            return coll;
        }


        public int MinJumps(int[] nums)
        {
            int nMax = nums.Max();
            HashSet<int> primes = sieve_of_eratosthenes(nMax);
            HashSet<int> currentPrimes = new HashSet<int>();
            for (int x = 0; x < nums.Length; x++)
            {
                if (primes.Contains(nums[x]))
                {
                    currentPrimes.Add(nums[x]);
                }
            }
            //create locations of prime factors
            var primeLocations = new Dictionary<int, SortedSet<int>>();
            foreach (var prime in currentPrimes)
            {
                for (int x = 0; x < nums.Length; x++)
                {
                    if (nums[x] % prime == 0)
                    {
                        if (!primeLocations.ContainsKey(prime))
                        {
                            primeLocations[prime] = new SortedSet<int>();
                        }
                        primeLocations[prime].Add(x);
                    }
                }
            }

            //get primefactor with maximum range such that low index is before high index
            int primeWithMaxRange = -1;
            int maxRange = -1;
            int maxRangeStart = -1;
            int maxRangeEnd = -1;
            foreach (var (key, value) in primeLocations)
            {
                int minIndex = -1, maxIndex = -1;
                if (value.Count == 1)
                {
                    continue; // just one prime factor location, so no range to consider
                }
                foreach (var y in value)
                {
                    if (nums[y] == key && minIndex == -1)
                    {
                        minIndex = y;
                    }
                    else
                    {
                        if (y > maxIndex)
                        {
                            maxIndex = y;
                        }
                    }
                }
                if (maxIndex > minIndex && maxIndex - minIndex > maxRange)
                {
                    primeWithMaxRange = key;
                    maxRangeStart = minIndex;
                    maxRangeEnd = maxIndex;
                    maxRange = maxRangeEnd - maxRangeStart;
                }
            }

            if (maxRangeStart == -1 || maxRangeEnd == -1) // no 'prime range' found in the array
            {
                return nums.Length - 1;
            }

            return maxRangeStart + (nums.Length - maxRangeEnd);


        }

        public void Demo()
        {
            PrimeTeleportation.Solution primeTeleportation = new();
            int[] nums = new int[] { 1, 2, 4, 6 };
            int answer = -1;
            // answer = primeTeleportation.MinJumps(nums);
            // Console.WriteLine($"Answer: {answer} should be 2");
            nums = [4, 6, 5, 8];
            
            // answer = primeTeleportation.MinJumps(nums);
            //Console.WriteLine($"Answer: {answer} should be 3");

            nums = [2, 3, 4, 7, 9];
            // answer = primeTeleportation.MinJumps(nums);
            // Console.WriteLine($"Answer: {answer} should be 2");

            nums = [7, 5, 7];
            // answer = primeTeleportation.MinJumps(nums);
            // Console.WriteLine($"Answer: {answer} should be 1");

            nums = [10, 3, 8, 10];
            // answer = primeTeleportation.MinJumps(nums);
            // Console.WriteLine($"Answer: {answer} should be 3");

            nums = [5, 2, 20, 1, 15];
            // answer = primeTeleportation.MinJumps(nums);
            // Console.WriteLine($"Answer: {answer} should be 1");

            nums = [17, 114, 68, 110, 9, 100, 7, 19, 111, 65, 28];
            answer = primeTeleportation.MinJumps(nums);
            Console.WriteLine($"Answer: {answer} should be 6");
        }
    }
}
