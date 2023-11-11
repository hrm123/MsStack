using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp.strings
{
    /// <summary>
    /// 214. Shortest Palindrome.  74ms (beats 83% users of c#) / 42MB (beats 46% users of c#)
    /// </summary>
    public class ShortestPalindrome
    {
        public static void TestCase()
        {
            ShortestPalindrome sp = new ShortestPalindrome();
            sp.CalcShortestPalindrome("aacecaaa");

        }
        public string CalcShortestPalindrome(string s)
        {
            string sRev = new string(s.ToCharArray().Reverse().ToArray());
            string s1 = s + "#" + sRev;
            //LPS to find longest palindromic prefix
            int[] lps = BuildLPS(s1);
            Console.WriteLine($"len = {lps[s1.Length - 1]}");
            int n = s.Length, longestpalindrome = lps[s1.Length - 1], i = 0;
            string prefix = new String(s.Substring(longestpalindrome).ToCharArray().Reverse().ToArray());
            return prefix + s;

        }

        private int[] BuildLPS(string str)
        {
            int n = str.Length, len = 0, i = 1;
            int[] lps = new int[n]; ;
            lps[0] = 0;
            while (i < n)
            {
                if (str[i] == str[len])
                {
                    len++;
                    lps[i] = len;
                    i++;
                }
                else
                {
                    if (len == 0)
                    {
                        lps[i] = 0;
                        i++;
                    }
                    else
                    {
                        len = lps[len - 1];
                    }
                }
            }
            return lps;
        }
    }
}
