using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp.strings
{
    /// <summary>
    /// 1143. Longest Common Subsequence
    /// 74 ms - beats 52% of c# users. 40 MB - beats 76% of C# users
    /// </summary>
    public class LCS_DP
    {
        int _m = 0, _n = 0;
        int[,] _dp;


        public static void TestCase()
        {
            string text1 = "abcde";
            string text2 = "ace";
            LCS_DP lcsDp = new LCS_DP();
            var answer = lcsDp.LongestCommonSubsequence(text1, text2);
            Console.WriteLine($"LCS = {answer} should be 3");
        }

        public static void TestCase1()
        {
            string text1 = "abc";
            string text2 = "abc";
            LCS_DP lcsDp = new LCS_DP();
            var answer = lcsDp.LongestCommonSubsequence(text1, text2);
            Console.WriteLine($"LCS = {answer} should be 3");
        }

        public static void TestCase2()
        {
            string text1 = "abc";
            string text2 = "def";
            LCS_DP lcsDp = new LCS_DP();
            var answer = lcsDp.LongestCommonSubsequence(text1, text2);
            Console.WriteLine($"LCS = {answer} should be 0");
        }

        public int LongestCommonSubsequence(string text1, string text2)
        {
            _m = text1.Length;
            _n = text2.Length;
            _dp = new int[_m + 1, _n + 1];

            for (int i = 0; i < _n + 1; i++)
            {
                _dp[0, i] = 0;
            }


            for (int i = 0; i < _m + 1; i++)
            {
                _dp[i, 0] = 0;
            }

            for (int i = 1; i < _m + 1; i++)
            {
                for (int j = 1; j < _n + 1; j++)
                {
                    int v1 = text1[i-1] == text2[j-1] ? 1 + _dp[i - 1, j - 1] : 0;
                    int v2 = text1[i - 1] != text2[j-1] ? _dp[i - 1, j - 1] : 0;
                    int v3 = _dp[i - 1, j];
                    int v4 = _dp[i, j - 1];
                    _dp[i, j] = Math.Max(v1, Math.Max(v2, Math.Max(v3, v4)));
                }
            }
            return _dp[_m, _n];

        }
    }
}
