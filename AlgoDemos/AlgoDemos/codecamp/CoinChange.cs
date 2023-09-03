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
        public int CoinChange(int[] coins, int amount)
        {
            Dictionary<int, int> CoinQty = new Dictionary<int, int>();

            int l = coins.Length;
            Array.Sort(coins);
            // Array.Reverse(coins);
            _coins = coins;

            for (int i = 0; i < l; i++)
            {
                CoinQty[coins[i]] = 0;
            }
            int recursionDepth = 0;
            int numCoins = RecursiveSoln(l - 1, amount, 0, recursionDepth);
            return numCoins;
        }

        private int RecursiveSoln(int end, int total, int numCoins, int recursionDepth)
        {
            Console.WriteLine($"end={end} amt={_coins[end]} total={total} numCoins={numCoins} recursionDepth={recursionDepth}");
            /*
            if(solutionCache.ContainsKey(end+","+total)){
                return solutionCache[end+","+total];
            }
            */
            int response = -1;
            if (end == 0)
            {
                //last element of _coins array.. if it reduces total to zero then return numCoins else  return -1
                if (total % _coins[end] == 0)
                {
                    response = numCoins + total / _coins[end];
                }
                else { response = -1; }
                solutionCache.Add(end + "," + total, response);
                return response;
            }
            if (total < _coins[end])
            {
                //no change in numCoins since current coin cannot help in reducing total
                //Console.WriteLine($"no change in numCoins");
                int excludesCurrentCoin = RecursiveSoln(end - 1, total, numCoins, recursionDepth + 1);

                response = excludesCurrentCoin;
                solutionCache.Add(end + "," + total, response);
                return response;
            }

            int resultIncludesCurrentCoin = RecursiveSoln(end - 1, total % _coins[end], numCoins + total / _coins[end], recursionDepth + 1);
            int resultExcludesCurrentCoin = RecursiveSoln(end - 1, total, numCoins, recursionDepth + 1);
            if (resultIncludesCurrentCoin == -1 && resultExcludesCurrentCoin == -1)
            {
                solutionCache.Add(end + "," + total, -1);
                return -1; // this route not possible to get solution
            }
            else if (resultIncludesCurrentCoin == -1)
            {
                // other result wins
                solutionCache.Add(end + "," + total, resultExcludesCurrentCoin);
                return resultExcludesCurrentCoin;
            }
            else if (resultExcludesCurrentCoin == -1)
            {
                // other result wins
                solutionCache.Add(end + "," + total, resultIncludesCurrentCoin);
                return resultIncludesCurrentCoin;
            }
            else
            {
                // minimum result wins
                response = resultIncludesCurrentCoin <= resultExcludesCurrentCoin ? resultIncludesCurrentCoin : resultExcludesCurrentCoin;
                solutionCache.Add(end + "," + total, response);
                return response;
            }
        }
    }
}