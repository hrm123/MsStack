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
    }
}