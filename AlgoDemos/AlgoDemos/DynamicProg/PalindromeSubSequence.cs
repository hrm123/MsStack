using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoDemos.DynamicProg
{
    /// <summary>
    /// Given a string find the longest palindrome sub sequence (includes non-contiguous characters that form palindrome)
    /// Note - dynamic progamming with O(n^2) runtime and O(n^2) space is optimal for subsequence but not for substring. For longest palindrome in substring, 
    /// just a simple iteration over characters and expansion at each character 
    /// is little better since it is O(n^2) runtime and O(1) space. Manacher's algorithm is even better for the other problem (substring based) 
    /// since  it is O(n) runtime and O(1) space
    /// </summary>
    public class PalindromeSubSequence
    {
        // consider building palindrome in 1 length strings and from them palindrome in 2 length strings .. upto final result we get pallindrom in (o,n-1) string
        public string LongestPalindrome(string s)
        {
            int n = s.Length;
            int[,] dp = new int[n, n];
            int i = 0;
            int j = 0;
            //Console.WriteLine($"iterating");
            for (i = n - 1; i >= 0; i--)
            {
                for (j = 0; j < n; j++)
                {
                    //  Console.WriteLine($"{i},{j}");
                    if (j == i)
                    {
                        dp[i, j] = 1;
                        continue;
                    }
                    if (j < i)
                    { //TODO - might be avoid such iterations?
                        dp[i, j] = 0;
                        continue;
                    }

                    if (s[i] == s[j])
                    {
                        dp[i, j] = 2 + ((i == n - 1 || j == 0) ? 0 : dp[i + 1, j - 1]);
                    }
                    else
                    {
                        dp[i, j] = Math.Max(
                            ((i == n - 1) ? 0 : dp[i + 1, j]),
                            ((j == 0) ? 0 : dp[i, j - 1])
                        );
                    }
                }
            }
            int solutionLength = dp[0, n - 1];

            /*
            Console.WriteLine($"writing dp");
            for(i=0;i<n;i++){
                for(j=0;j<n;j++){
                    Console.Write($"{dp[i,j]} ");
                }
                Console.WriteLine();

            }
            */
                    

            // Console.WriteLine($"writing solution");
            i = 0;
            j = n - 1;
            int k = 0;
            char[] response = new char[solutionLength];
            WriteSolution(dp, ref response, i, j, ref k, s);
            
            return new String(response);
        }

        void WriteSolution(int[,] dp, ref char[] solution, int i, int j, ref int k, string s)
        {
            // Console.WriteLine($"processing i={i},j={j},k={k}");
            if (i == j)
            {
                //one character - end of recursion
                // Console.WriteLine($"adding ={i},{j},{s[j]} k={k}");
                solution[k++] = s[j];
                return;
            }
            int l = dp[i, j];
            if (s[i] == s[j])
            {
                char sameCharAtEnds = s[i];
                if (l == 2 + dp[i + 1, j - 1])
                {
                    
                    // take the end character str[i] or str[j]
                    // Console.WriteLine($"adding ={i},{j},{s[i]} k={k}");
                    solution[k++] = sameCharAtEnds;
                    i = i + 1;
                    j = j - 1;
                    if (j >= i)
                    {
                        WriteSolution(dp, ref solution, i, j, ref k, s);
                    }
                    // Console.WriteLine($"adding ={i},{j},{s[i]} k={k}");
                    solution[k++] = sameCharAtEnds;
                } 
                else if (i==j+1) // 2 character string
                {
                    // Console.WriteLine($"adding ={i},{j},{s[i]} k={k} twice");
                    solution[k++] = sameCharAtEnds;
                    solution[k++] = sameCharAtEnds;
                }
                else
                {
                    throw new ApplicationException("Unexpected state");
                }
            }
            else
            {
                if (dp[i, j] == dp[i + 1, j])
                {
                    i = i + 1;
                    WriteSolution(dp, ref solution, i, j, ref k, s);
                }
                else if (dp[i, j] == dp[i, j - 1])
                {
                    j = j - 1;
                    WriteSolution(dp, ref solution, i, j, ref k, s);
                }
                else if (dp[i, j] == dp[i+1, j - 1])
                {
                    j = j - 1;
                    i = i + 1; 
                    WriteSolution(dp, ref solution, i, j, ref k, s);
                }
            }
            return;
        }

        public void Demo()
        {
            PalindromeSubSequence palindrome = new PalindromeSubSequence();
            string testCase = "babad";
            Console.WriteLine($"solution for {testCase} = {palindrome.LongestPalindrome(testCase)}");

            testCase = "cbbd";
            Console.WriteLine($"solution for {testCase} = {palindrome.LongestPalindrome(testCase)}");

            testCase = "abacyxcaba";
            Console.WriteLine($"solution for {testCase} = {palindrome.LongestPalindrome(testCase)}");

            testCase = "ab123cd456dc789ba";
            Console.WriteLine($"solution for {testCase} = {palindrome.LongestPalindrome(testCase)}");
        }
    }
}
