using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp.strings
{
    public class KMP
    {

        /// <summary>
        /// LPS{i} can be calculated based on LPS(i-1). If LPS(i-1) = l, then if str[l] == str[i] then LPS(i) = l+1. 
        /// If (l==0 && str[l}!=str[i] )then LPS[i]=0. if (l!=0 && str[l}!=str[i]) recursively calculate len=LPS(len-1) then check tr[i]==str[len} or not while len>0
        /// LPS(0) = 0; if str[0]==str[1] LPS(1)=1
        /// </summary>
        public int[] BuildLPS(string str)
        {
            int n = str.Length, len = 0, i =1;
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

        /// <summary>
        /// LPS(i) where i is character in text will tell which index of pattern string to choose next.
        /// if we are comparing ith character of text and we found that either we have reached end of pattern string (or) we have mismatch at i,
        /// then  i is always incremented by 1 but j (current index of char on pattern will be LPS(i)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="pat"></param>
        public void solve(string text, string pat)
        {
            int n = text.Length, m = pat.Length, i=0,j=0;
            int[] lps = BuildLPS(pat);
            while (i < n)
            {
                if (text[i] == pat[j]) { i++;j++; }
                if (j == m )
                {
                    Console.WriteLine($"Pattern found at {i-j}");
                    j = lps[j - 1];
                }
                else
                {
                     if(i<n && pat[j] != text[i])
                    {
                        if(j==0) { i++;  }
                        else
                        {
                            j = lps[j - 1];
                        }
                    }
                }
            }


        }
    }
}
