using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    /// <summary>
    /// 72. Edit Distance - 57ms - beats 90% of c# users. 43 MB - beats 92% of c# users.
    /// </summary>
    public class EditDistanceDP
    {
        int[,] _dp; // stores edit distance when word1 has i chars & word2 has j chars
        string _src, _tar;
        public static void TestCase()
        {
            EditDistanceDP dp = new EditDistanceDP();
            int answer = dp.MinDistance("horse", "ros");
            // answer = dp.MinDistance("intention", "execution");

        }

        public int MinDistance(string word1, string word2)
        {
            int l1 = word1.Length + 1; // src
            int l2 = word2.Length + 1; // target
            _src = " " + word1;
            _tar = " " + word2;
            _dp = new int[l1, l2];
            for (int i = 0; i < l1; i++)
            {
                _dp[i, 0] = i;
            }
            for (int i = 0; i < l2; i++)
            {
                _dp[0, i] = i;
            }
            for (int i = 1; i < l1; i++)
            {
                for (int j = 1; j < l2; j++)
                {
                    if (_src[i] == _tar[j])
                    {
                        _dp[i, j] =  _dp[i - 1, j - 1];
                        continue;
                    }
                    int addDistance = 1 + _dp[i, j - 1];
                    int replaceDistance = 1 + _dp[i - 1, j - 1];
                    int deleteDistance = 1 + _dp[i - 1, j];
                    _dp[i, j] = Math.Min(Math.Min(addDistance, replaceDistance), Math.Min(replaceDistance, deleteDistance));
                }
            }
            return _dp[l1-1, l2-1];
        }
    }
}
