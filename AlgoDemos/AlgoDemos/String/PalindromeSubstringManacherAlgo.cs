using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.String
{
    /// <summary>
    /// For each character expand at that character based on odd length palindrome and even length 
    /// palindrome and keep track of max palindrome seen so far. O(n^2) runtime and O(1) space
    /// </summary>
    public class PalindromeSubstringManacherAlgo
    {
        string _s = "";
        int _n = 0;

        private (int start, int end, int length) GetOddLengthPalindrome(int x,
        int mx, int mx_s, int mx_e)
        {
            int c = 0, c_s = 0, c_e = 0;
            if (_s[x] == _s[x - 1])
            {
                int left = x - 1, right = x;

                while (left >= 0 && right < _n && _s[left] == _s[right])
                {
                    c += 2;
                    c_s = left;
                    c_e = right;
                    left--;
                    right++;
                }
            }

            //see if we need to update the max palindrom

            return c > mx ? (c_s, c_e, c) : (mx_s, mx_e, mx);
        }

        private (int start, int end, int length) GetEvenLengthPalindrome(int x,
        int mx, int mx_s, int mx_e)
        {


            int c = 0, c_s = 0, c_e = 0;
            if (x >= _n - 1)
            {
                return (mx_s, mx_e, mx);
            }


            if (_s[x - 1] == _s[x + 1])
            {
                int left = x - 1, right = x + 1;
                c++; // for the middle character
                while (left >= 0 && right < _n && _s[left] == _s[right])
                {
                    c += 2;
                    c_s = left;
                    c_e = right;
                    left--;
                    right++;
                }
            }

            //see if we need to update the max palindrome
            return c > mx ? (c_s, c_e, c) : (mx_s, mx_e, mx);
        }

        public string LongestPalindrome(string s)
        {

            int mx = 0, mx_s = 0, mx_e = 0;
            _s = s;
            _n = _s.Length;

            if (_n == 1)
            {
                return s;
            }
            if (_n == 0)
            {
                return "";
            }

            for (int x = 1; x < _n; x++)
            {
                (mx_s, mx_e, mx) = GetOddLengthPalindrome(x, mx, mx_s, mx_e);
                (mx_s, mx_e, mx) = GetEvenLengthPalindrome(x, mx, mx_s, mx_e);
            }
            if (mx != 0)
            {
                return s.Substring(mx_s, mx);
            }
            else
            {
                return s.Substring(0, 1); ; // each character of string itself is palindrome
            }
        }
        public static void Demo()
        {
            PalindromeSubstring ps = new PalindromeSubstring();
            Console.WriteLine($"answer={ps.LongestPalindrome("ababa")} should be of length 5");
            Console.WriteLine($"answer={ps.LongestPalindrome("abccbd")} should be of length 4");
            Console.WriteLine($"answer={ps.LongestPalindrome("abcde")} should be of length 1");
            Console.WriteLine($"answer={ps.LongestPalindrome("a")} should be of length 1");
            Console.WriteLine($"answer={ps.LongestPalindrome("cccc")} should be of length 4");
            Console.WriteLine($"answer={ps.LongestPalindrome("")} should be of length 0");
        }
    }
}
