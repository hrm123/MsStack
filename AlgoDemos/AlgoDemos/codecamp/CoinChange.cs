using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    public class CoinChangeSoln
    {

        Dictionary<string, int> solutionCache = new Dictionary<string, int>();
        int[] _coins;
        int _soln = 100000000;

        public int CoinChange(int[] coins, int amount)
        {

            if (amount == 0)
            {
                return 0;
            }

            int[] dp = new int[amount + 1];

            Array.Fill(dp, Int32.MaxValue);
            dp[0] = 0;

            for (int i = 1; i <= amount; i++)
            {
                foreach (int coin in coins)
                {
                    if (i == coin)
                    {
                        dp[i] = 1;
                    }
                    else if (i > coin)
                    {
                        if (dp[i - coin] == Int32.MaxValue)
                        {
                            continue;
                        }
                        dp[i] = Math.Min(dp[i - coin] + 1, dp[i]);
                    }
                }
            }

            if (dp[amount] == Int32.MaxValue)
            {
                return -1;
            }

            return dp[amount];
        }

        private void RecursiveSoln(int end, int total, int numCoins, int recursionDepth)
        {

            if (end < 0)
            {
                return;
            }
            string cachekey = end + "," + total;

            int response = -1;
            if (solutionCache.ContainsKey(cachekey))
            {
                response = solutionCache[cachekey];
                if (_soln > response) { _soln = response; }
                return;
            }
            // Console.WriteLine($"end={end} amt={_coins[end]} total={total} numCoins={numCoins} recursionDepth={recursionDepth}");


            if (total == 0)
            {
                response = numCoins;
                if (!solutionCache.ContainsKey(cachekey))
                {
                    solutionCache.Add(cachekey, response);
                }
                else
                {
                    Console.WriteLine(
                    $"cache already contains cachekey {cachekey} with value {solutionCache[cachekey]} instead of new value {response}");
                }
                if (_soln > response) { _soln = response; }
            }

            if (end == 0)
            {
                //last element of _coins array.. if it reduces total to zero then return numCoins else  return -1
                if (total % _coins[end] == 0)
                {
                    response = numCoins + total / _coins[end];
                    if (!solutionCache.ContainsKey(cachekey))
                    {
                        solutionCache.Add(cachekey, response);
                    }
                    else
                    {
                        Console.WriteLine(
                         $"cache already contains cachekey {cachekey} with value {solutionCache[cachekey]} instead of new value {response}");
                    }
                    if (_soln > response) { _soln = response; }
                }
                return;

            }
            if (total < _coins[end])
            {
                //no change in numCoins since current coin cannot help in reducing total
                //Console.WriteLine($"no change in numCoins");
                RecursiveSoln(end - 1, total, numCoins, recursionDepth + 1);

            }
            else
            {

                RecursiveSoln(end, total - _coins[end], numCoins + 1, recursionDepth + 1);
                RecursiveSoln(end - 1, total, numCoins, recursionDepth + 1);
            }
        }
    }
}