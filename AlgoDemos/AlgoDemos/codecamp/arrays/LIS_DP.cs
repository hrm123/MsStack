using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp.arrays
{
    /// <summary>
    /// 300. Longest Increasing Subsequence. 118 ms - beats 62% c# users. 40 MB - beats 80% c# users
    /// </summary>
    public class LIS_DP
    {
        int[] _dp;


        public int LengthOfLIS(int[] nums)
        {
            _dp = new int[nums.Length];
            _dp[0] = 1;
            for (int i = 1; i < nums.Length; i++)
            {
                _dp[i] = 1; //initialize to minimum (considering this is only element)
                for (int y = 0; y < i; y++)
                {
                    if (nums[y] <= nums[i])
                    {
                        _dp[i] = Math.Max(1 + _dp[y], _dp[i]);
                    }
                }

            }
            int answer = _dp[0];
            for (int i = 1; i < nums.Length; i++)
            {
                answer = Math.Max(answer, _dp[i]);
            }
            return answer;
        }

    }
}
