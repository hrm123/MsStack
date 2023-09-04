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
            
            int l = coins.Length;
            Array.Sort(coins);
            _coins = coins;

            int recursionDepth = 0;
            RecursiveSoln(l - 1, amount, 0, recursionDepth);
            return (_soln == 100000000) ? -1 : _soln;
        }

        private void RecursiveSoln(int end, int total, int numCoins, int recursionDepth)
        {

            if (end < 0)
            {
                return;
            }
            string cachekey = end + "," + total + "," + numCoins;

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