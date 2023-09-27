using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    /// <summary>
    /// 72. EditDistance
    /// </summary>
    public class EditDistanceRecursive
    {

        int[,] _cache;
        List<string> _changes;

        public int MinDistance(string word1, string word2)
        {
            int s = word1.Length;
            int t = word2.Length;
            _cache = new int[s, t];
            for (int i = 0; i < s; i++)
                for (int j = 0; j < t; j++)
                    _cache[i, j] = -1;
            _changes = new List<string>();
            int edits = Calculate(word1, word2);
            return edits;
        }

        int Calculate(string src, string tar)
        {

            int sl = src.Length;
            int tl = tar.Length;

            if (sl == 0)
            {
                return tl;
            }
            if (tl == 0)
            {
                return sl;
            }

            if (_cache[sl - 1, tl - 1] != -1)
            {
                return _cache[sl - 1, tl - 1];
            }

            int editDistance = 0;

            // if last char of both string are same then just call sub problem from second last char
            if (src[sl - 1] == tar[tl - 1])
            {
                editDistance = Calculate(src.Substring(0, sl - 1), tar.Substring(0, tl - 1));
                _cache[sl - 1, tl - 1] = editDistance;
                _changes.Add($"remove one char of both strings at end - {src} & {tar}");
                return editDistance;
            }

            int addDistance = Calculate(src.Substring(0, sl), tar.Substring(0, tl - 1)); // add one char (same as last character of traget string) to src string 
            int replaceDistance = Calculate(src.Substring(0, sl - 1), tar.Substring(0, tl - 1)); // replace last char of src string  with same as last character of target string
            int removeDistance = Calculate(src.Substring(0, sl - 1), tar.Substring(0, tl));// delete last char of src string 



            editDistance = Math.Min(Math.Min(addDistance, replaceDistance), Math.Min(replaceDistance, removeDistance));

            //debug
            if (editDistance == addDistance) { _changes.Add($"Add at end - "); }

            editDistance += 1;
            _cache[sl - 1, tl - 1] = editDistance;
            // Console.WriteLine($"Edit Distance of {src} and {tar} is {editDistance}");
            return editDistance;

        }
    }
}
