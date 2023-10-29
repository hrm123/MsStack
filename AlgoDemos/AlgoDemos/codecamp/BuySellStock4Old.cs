using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    /// <summary>
    /// 188.
    /// failing for prices =     [1,2,4,2,5,7,2,4,9,0] k = 2. answer = 12 instead of 13
    /// </summary>
    internal class BuySellStock4Old
    {
        public static void TestCase()
        {
            BuySellStock4 bss4 = new BuySellStock4();
            int ans = bss4.MaxProfit(2, new int[] { 1, 2, 4, 2, 5, 7, 2, 4, 9, 0 });
            Console.WriteLine();
            Console.WriteLine($"{ans} should be 13");
        }

        public class Solution
        {

            public int MaxProfit(int k, int[] prices)
            {

                List<int> profits = new List<int>();
                int buy = prices[0], sell, profit;
                int n = prices.Length;
                Console.WriteLine($"n={n}");
                if (n == 2) { return prices[1] > prices[0] ? prices[1] - prices[0] : 0; }

                // get all buy- sells with profit.. and push profit into array
                for (int i = 1; i < n; i++)
                {
                    Console.WriteLine($"{i}");
                    if (i == n - 1)
                    {
                        sell = prices[i];
                        profit = sell - buy;
                        profits.Add(profit);
                        continue;
                    }

                    if (prices[i] > prices[i - 1] && prices[i + 1] <= prices[i])
                    { // started decreasing
                        Console.WriteLine("started decreasing");
                        sell = prices[i];
                        profit = sell - buy;
                        profits.Add(profit);
                        buy = prices[i];
                        Console.WriteLine("started decreasing end");
                        continue;
                    }
                    if (prices[i] < prices[i - 1] && prices[i + 1] >= prices[i])
                    { //started increasing
                        Console.WriteLine("started increasing");
                        sell = prices[i];
                        profit = sell - buy;
                        profits.Add(profit);
                        buy = prices[i];
                        continue;
                    }
                }

                //get k subarrays that will give max profit using DP
                Console.WriteLine($"profits={string.Join(",", profits.Select(n => n.ToString()).ToArray())}");
                int[,] dp = new int[profits.Count() + 1, k + 1];
                for (int i = 0; i <= profits.Count(); i++)
                {
                    dp[i, 0] = 0;
                }
                for (int y = 0; y <= k; y++)
                {
                    dp[0, y] = 0;
                }

                dp[0, 1] = 0;
                int profitTillPrev = 0;
                for (int i = 1; i <= profits.Count(); i++)
                { //k==1
                  // dp[i, 1] = (dp[i - 1, 1]<0) ? profits[i - 1]: dp[i - 1, 1] + profits[i - 1];

                    if (profitTillPrev + profits[i - 1] < 0)
                    {
                        dp[i, 1] = profitTillPrev;
                        profitTillPrev = 0;
                    }
                    else
                    {
                        dp[i, 1] = profitTillPrev + profits[i - 1];
                        profitTillPrev = dp[i, 1];
                    }


                    /*
                    dp[i, 1] = System.Math.Max(profitTillPrev, profitTillPrev + profits[i - 1]);
                    if (dp[i, 1] != profitTillPrev + profits[i - 1])
                    {
                        //current element has been skipped - so not contiguous array - mosly current elemetn is negative
                        profitTillPrev = 0;
                    }
                    else
                    {
                        profitTillPrev = dp[i, 1];
                    }
                    */
                }

                Console.WriteLine($"dp[i,1]");
                for (int i = 0; i <= profits.Count; i++)
                {
                    Console.Write($"{dp[i, 1]},");
                }
                for (int x = 1; x <= profits.Count(); x++)
                {
                    for (int y = 2; y <= k; y++)
                    {
                        int profit1 = 0;
                        int profit2 = 0;
                        for (int i = 0; i <= x - 1; i++)
                        {
                            profit1 = System.Math.Max(profit1, dp[i, y - 1]);
                        }
                        for (int i = 0; i <= x - 1; i++)
                        {
                            profit2 = System.Math.Max(profit2, dp[i, y]);
                        }
                        dp[x, y] = System.Math.Max(profits[x - 1] + profit1, profit2);
                        Console.WriteLine($"dp[{x},{y}]={dp[x, y]}");
                    }
                }
                if (k == 1)
                { // have to calc max separately only for this row
                    int max = 0;
                    for (int i = 1; i <= profits.Count(); i++)
                    {
                        max = System.Math.Max(dp[i, 1], max);
                    }
                    return max;
                }
                else
                {
                    return dp[profits.Count(), k];
                }

            }
        }
    }
}
