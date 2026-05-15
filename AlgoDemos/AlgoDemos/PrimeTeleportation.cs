using AlgoDemos.ExpressionTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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


            /*
            var primeNumberLocationsInArray = new Dictionary<int,List<int>>();
            foreach (var prime in currentPrimes)
            {
                for (int x = 0; x < nums.Length; x++)
                {
                    if (nums[x] == prime)
                    {
                        if (!primeNumberLocationsInArray.ContainsKey(prime))
                        {
                            primeNumberLocationsInArray[prime] = new List<int>();
                        }
                        primeNumberLocationsInArray[prime].Add(x);
                    }
                }
            }
            */

            var primeNumbersTeleportLocations = new Dictionary<int, List<int>>();
            foreach (var prime in currentPrimes)
            {
                for (int x = 0; x < nums.Length; x++)
                {
                    if (nums[x] != prime && nums[x] % prime == 0)
                    {
                        if (!primeNumbersTeleportLocations.ContainsKey(prime))
                        {
                            primeNumbersTeleportLocations[prime] = new List<int>();
                        }
                        primeNumbersTeleportLocations[prime].Add(x);
                    }
                }
            }


            //create adjacency list
            var adjacencyList = new Dictionary<int, List<int>>();
            for (int y = 0; y < nums.Length; y++)
            {
                adjacencyList[y] = new List<int>();
                if (y < nums.Length - 1)
                {
                    adjacencyList[y].Add(y + 1);
                }
                if (y > 0)
                {
                    adjacencyList[y].Add(y - 1);
                }

                if (currentPrimes.Contains(nums[y]) && primeNumbersTeleportLocations.ContainsKey(nums[y]))
                {
                    foreach(var teleportLocation in primeNumbersTeleportLocations[nums[y]])
                    {
                        adjacencyList[y].Add(teleportLocation);
                    }
                }
            }

            return BFS(adjacencyList, nums);


        }

        private int BFS(Dictionary<int, List<int>> adjList, int[] nums)
        {

            var bfs = new Queue<(int node, string path)>();
            var visited = new HashSet<int>();

            bfs.Enqueue((0, ""));
            visited.Add(0);
            while(bfs.Count > 0) {
                var (curNode, path) = bfs.Dequeue();
                if(curNode == nums.Length - 1)
                {
                    return path.Count(c => c == ',');
                }
                foreach (var neighbor in adjList[curNode])
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        bfs.Enqueue((neighbor, path + "," + neighbor));
                    }
                }
            }
            return -1;
        }

        public void Demo()
        {
            PrimeTeleportation.Solution primeTeleportation = new();
            int[] nums = new int[] { 1, 2, 4, 6 };
            int answer = -1;
            //answer = primeTeleportation.MinJumps(nums);
            // Console.WriteLine($"Answer: {answer} should be 2");

            nums = [4, 6, 5, 8];
            answer = primeTeleportation.MinJumps(nums);
            Console.WriteLine($"Answer: {answer} should be 3");

            
            nums = [2, 3, 4, 7, 9];
            answer = primeTeleportation.MinJumps(nums);
            Console.WriteLine($"Answer: {answer} should be 2");

            nums = [7, 5, 7];
            answer = primeTeleportation.MinJumps(nums);
            Console.WriteLine($"Answer: {answer} should be 1");

            nums = [10, 3, 8, 10];
            answer = primeTeleportation.MinJumps(nums);
            Console.WriteLine($"Answer: {answer} should be 3");

            nums = [5, 2, 20, 1, 15];
            answer = primeTeleportation.MinJumps(nums);
            Console.WriteLine($"Answer: {answer} should be 1");

            nums = [17, 114, 68, 110, 9, 100, 7, 19, 111, 65, 28];
            answer = primeTeleportation.MinJumps(nums);
            Console.WriteLine($"Answer: {answer} should be 6");
            
        }
    }
}
