using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    /// <summary>
    /// 188.
    /// failing for prices =     [1,2,4,2,5,7,2,4,9,0] k = 2. answer = 12 instead of 13
    /// </summary>
    internal class BuySellStock4
    {
        public static void TestCase_works()
        {
            BuySellStock4 bss4 = new BuySellStock4();
            int ans = bss4.MaxProfit(2, new int[] { 1, 2, 4, 2, 5, 7, 2, 4, 9, 0 });
            // Console.WriteLine();
            // Console.WriteLine($"{ans} should be 13");
        }
        public static void TestCase_works_2()
        {
            BuySellStock4 bss4 = new BuySellStock4();
            int ans = bss4.MaxProfit(2, new int[] { 2, 4, 1 });
            // Console.WriteLine();
            // Console.WriteLine($"{ans} should be 2");
        }

        public static void TestCase()
        {
            BuySellStock4 bss4 = new BuySellStock4();
            int ans = bss4.MaxProfit(4, new int[] { 1, 2, 4, 2, 5, 7, 2, 4, 9, 0 });
            // Console.WriteLine();
            // Console.WriteLine($"{ans} should be 15");
        }

        public int MaxProfit(int k, int[] prices)
        {
            if (prices.Length < 2)
            {
                return 0;
            }
            List<int> profits = new List<int>();
            int buy = prices[0], sell, profit;
            int n = prices.Length;
            // Console.WriteLine($"n={n}");
            if (n == 2) { return prices[1] > prices[0] ? prices[1] - prices[0] : 0; }

            // get all buy- sells with profit.. and push profit into array
            for (int i = 1; i < n; i++)
            {
                // Console.WriteLine($"{i}");
                if (i == n - 1)
                {
                    sell = prices[i];
                    profit = sell - buy;
                    profits.Add(profit);
                    continue;
                }

                if (prices[i] > prices[i - 1] && prices[i + 1] <= prices[i])
                { // started decreasing
                    // Console.WriteLine("started decreasing");
                    sell = prices[i];
                    profit = sell - buy;
                    profits.Add(profit);
                    buy = prices[i];
                    // Console.WriteLine("started decreasing end");
                    continue;
                }
                if (prices[i] < prices[i - 1] && prices[i + 1] >= prices[i])
                { //started increasing
                    // Console.WriteLine("started increasing");
                    sell = prices[i];
                    profit = sell - buy;
                    profits.Add(profit);
                    buy = prices[i];
                    continue;
                }
            }

            //get k subarrays that will give max profit using DP
            // Console.WriteLine($"profits={string.Join(",", profits.Select(n => n.ToString()).ToArray())}");
            int[] profitsArray = profits.ToArray();
            return MaxProfitsRecursive(profitsArray, profitsArray.Length, k);

            /*
            int pn = profits.Count();
            int[,] dp = new int[pn +1, k + 1]; // dp[i,j] = profit when j transaction are left and i profits transactions are left
            

            //k=1 - only one transaction is left - choose max possible single profit 
            for (int i=1;i<=pn; i++)
            {
                dp[i, 1] = max(profitsArray, i-1);
            }

            //n=1 - only one profit transaction is left - choose it if it is positive
            for (int i = 1; i <= n; i++)
            {
                dp[i, 1] = max(profitsArray, i - 1);
            }
            */




        }

        int MaxProfitsRecursive(int[] profitsArray, int n, int k)
        {
            if (n == 1)
            { // only one profit transaction left
                return System.Math.Max(profitsArray[0], 0);
            }
            if (k == 1)
            { //only one transaction allowed
                return FindMaxSubArraySum(profitsArray, 0, n-1);
            }
            
            int res = Int32.MinValue;
            for(int i=1;i<n;i++)
            {
                int oneTransSum = FindMaxSubArraySum(profitsArray, i, n - 1);
                int otherTransactionsSum = MaxProfitsRecursive(profitsArray, i, k - 1);
                res = System.Math.Max(res, otherTransactionsSum + oneTransSum);
            }
            return res;
        }

        int sum(int[] a, int from, int to)
        {
            int total = 0;
            for (int i = from; i < to; i++)
            {
                total += a[i];
            }
            return total;
        }

        int max(int[] a,int n)
        {
            int maxVal = 0;
            for(int i = 0; i < n; i++)
            {
                maxVal = System.Math.Max(a[i], maxVal);
            }
            return maxVal;
        }


        private int FindMaxSubArraySum(int[] profitsArray,int start, int end)
        {
            if(start == end)
            {
                return Math.Max(profitsArray[end], 0);
            }

            int[] profits = profitsArray.Skip(start).Take(end-start+1).ToArray();
            int n = profits.Length;
            int s = 0;
            int[] maxSumEnding = new int[n];
            int[] maxSumStartIndex = new int[n];
            string[] dbg = new string[n];
            dbg[0] = $"{s}-{profits[s]}";
            maxSumEnding[0] = profits[s];
            maxSumStartIndex[0] = s;

            for (int i = 1; i < n; i++)
            {
                if (maxSumEnding[i - 1] > 0)
                {
                    dbg[i] = $"{s}-{profits[i] + maxSumEnding[i - 1]}";
                    maxSumEnding[i] = profits[i] + maxSumEnding[i - 1];
                    maxSumStartIndex[i] = s;
                }
                else
                {
                    s = i;
                    dbg[i] = $"{s}-{profits[i]}";
                    maxSumEnding[i] = profits[i];
                    maxSumStartIndex[i] = s;
                }
            }
            // Console.WriteLine("Max Sum Values");
            for (int i = 0; i < n; i++)
            {
                // Console.Write($"{dbg[i]},");
            }

            int max = Int32.MinValue;
            for (int i = 0; i < n; i++)
            {
                max = System.Math.Max(max, maxSumEnding[i]);
            }
            int res =  Math.Max(max, 0);
            // Console.WriteLine($"res={res}");
            return res;
        }
    }
}
