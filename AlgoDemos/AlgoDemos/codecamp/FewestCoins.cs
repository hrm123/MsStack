using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    /// <summary>
    /// 322. Coin Change - 114 ms - beats 28% of C# users. 42MB - beats 40% of c# users
    /// </summary>
    public class FewestCoins
    {
        int[,] _dp; // = new int[amount + 1, n + 1];
        int[] _coins;
        public static void TestCase()
        {
            int[] coins = new int[] { 186, 419, 83, 408 };
            int amount = 6249;
            FewestCoins fc = new FewestCoins();
            int fewest = fc.CoinChange(coins, amount);
            Console.WriteLine($"{fewest} should be 20");
        }

        public int CoinChange(int[] coins, int amount)
        {

            if (amount == 0)
            {
                return 0;
            }
            int n = coins.Length;
            _coins = new int[n + 1];
            _coins[0] = 0;
            for (int j = 0; j < n; j++)
            {
                _coins[j+1] = coins[j];
            }
            Array.Sort(_coins);
            _dp = new int[amount + 1, n + 1];

            for( int i = 0; i <= amount;i++)
                for (int j = 0; j <= n; j++)
                {
                    _dp[i, j] = -1;
                }
            for (int i = 0; i <= amount; i++)
            {
                 _dp[i, 0] = Int32.MaxValue; // with no coins we cannot make any amount
            }
            for (int j = 0; j <= n; j++)
            {
                _dp[0, j] = 0; // zero amt means zero coins needed
            }

            for (int amt = 1; amt <= amount; amt++)
            {
                for (int j = 1; j <= n; j++)
                {
                    
                    int coin = _coins[j];
                    if (amt == coin)
                    {
                        _dp[amt, j] = 1; // only one coin of same value of amount is required 
                    }
                    else if (amt > coin)
                    {

                        if (_dp[amt - coin, j] == Int32.MaxValue)
                        {
                            _dp[amt, j] = _dp[amt, j - 1];
                            continue;
                        }
                        else
                        {
                            _dp[amt, j] = Math.Min(_dp[amt - coin, j] + 1, _dp[amt, j - 1]);
                        }
                    }
                    else if (amt < coin)
                    {
                        // since current coin cannot be used use previous coin value
                        _dp[amt, j] = _dp[amt,j - 1];
                    }
                }
            }

            return (_dp[amount, _coins.Length-1] == Int32.MaxValue) ? -1 : _dp[amount, _coins.Length-1];
        }
    }
}
