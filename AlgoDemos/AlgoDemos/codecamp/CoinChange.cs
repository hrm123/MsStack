using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    public class CoinChangeSoln
    {
        public int CoinChange(int[] coins, int amount)
        {
            Dictionary<int, int> CoinQty = new Dictionary<int, int>();

            int l = coins.Length;

            for (int i = 0; i < l; i++)
            {
                CoinQty[coins[i]] = 0;
            }

            Array.Sort(coins);
            Array.Reverse(coins);

            int total = amount;

            // Console.WriteLine($"l={coins.Length}");
            for (int i = 0; i < l; i++)
            {
                int denom = coins[i];
                Console.WriteLine($"denom={denom} total={total}");
                if (total == 0)
                {
                    break;
                }

                if (total >= denom)
                {
                    int tmp = total % denom;

                    CoinQty[denom] = total / denom;
                    total = tmp;
                    Console.WriteLine($"denom={denom} total={total} qty={CoinQty[denom]}");
                }
            }

            if (total != 0)
            {
                return -1;
            }

            int totalCoins = 0;
            foreach (var (key, value) in CoinQty)
            {
                totalCoins += value;
            }
            return totalCoins;



        }
    }
}
