using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{


    public class EditDistance
    {
        int[,] _cache;
        char[,] _changes;

        public int MinDistance(string word1, string word2)
        {
            int s = word1.Length;
            int t = word2.Length;
            _cache = new int[s, t];
            _changes = new char[s, t];
            int edits = Calculate(word1, word2);

            PrintSolution(word1, word2);

            return edits;
        }


        void PrintSolution(string word1, string word2)
        {
            int i = 4, j = 2;
            Console.WriteLine($"Following modifications (from the end) done on source string '{word1}' to match with target string '{word2}'");
            while (i>=0 && j>=0)
            {
                switch(_changes[i,j])
                {
                    case 'N': // no change in last character
                        Console.WriteLine($"Same Character removed from both (action at {i},{j})");
                        i--;
                        j--;
                        break;
                    case 'R': //remove
                        Console.WriteLine($"Remove Character  (action at {i},{j})");
                        i--;
                        break;
                    case 'S': //substitute
                        Console.WriteLine($"substitute Character (action at {i},{j})");
                        i--;
                        j--;
                        break;
                    case 'A': //add
                        Console.WriteLine($"Add Character  (action at {i},{j})");
                        j--;
                        break;
                }

            }
        }

        int Calculate(string src, string tar)
        {

            int sl = src.Length;
            int tl = tar.Length;
            int editDistance = 0;

            //end condition
            if (sl == 0)
            {
                return tl;
            }
            else if (tl == 0)
            {
                return sl;
            }
            // if last char of both string are same then just call sub problem from second last char
            else if (src[sl - 1] == tar[tl - 1])
            {
                editDistance = Calculate(src.Substring(0, sl - 1), tar.Substring(0, tl-1));
                _cache[sl - 1, tl -1] = editDistance;
                _changes[sl - 1, tl - 1] = 'N'; // no change
                return editDistance;
            }

            int addDistance = Calculate(src.Substring(0, sl ), tar.Substring(0, tl - 1));
            int replaceDistance = Calculate(src.Substring(0, sl - 1), tar.Substring(0, tl - 1));
            int removeDistance = Calculate(src.Substring(0, sl - 1), tar.Substring(0, tl));



             editDistance = Math.Min(Math.Min(addDistance, replaceDistance), Math.Min(replaceDistance, removeDistance));

            //debug
            if(editDistance == addDistance) { _changes[sl - 1, tl - 1] = 'A';  } //add
            if (editDistance == replaceDistance) { _changes[sl - 1, tl - 1] = 'S'; } // substitute
            if (editDistance == removeDistance) { _changes[sl - 1, tl - 1] = 'R'; } // remove

            editDistance += 1;
            _cache[sl-1, tl-1] = editDistance;
            Console.WriteLine($"Solution at {sl-1} , {tl-1}");
            return editDistance;

        }
    }
}
