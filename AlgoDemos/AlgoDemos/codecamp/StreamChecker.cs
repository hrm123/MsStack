using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    /// <summary>
    /// Class checks if given stream of characters ends with any of words given 
    /// </summary>
    public class StreamChecker
    {

        Dictionary<string, HashSet<int>> _matcher = new Dictionary<string, HashSet<int>>();
        string[] _words;

        public StreamChecker(string[] words)
        {
            _words = words;
            // create matcher = Hash<char+indexinword,set of ints> which gives index of all words that have particular character at particular position
            // have to reverse words since it is easier to match
            int indexOfword = 0;
            foreach (string word in words)
            {
                for (int i = 0; i < word.Length; i++)
                {
                    HashSet<int> tmp = null;
                    string key = word[i] + "" + i;
                    if (_matcher.ContainsKey(key))
                    {
                        tmp = _matcher[key];
                    }
                    else
                    {
                        tmp = new HashSet<int>();
                        _matcher[key] = tmp;
                    }
                    tmp.Add(indexOfword);
                }
                indexOfword++;
            }

        }


        List<char> streamedChars = new List<char>();  // store characters that are getting streamed

        public bool Query(char letter)
        {
            HashSet<int> prevSet = null;
            int j = 0;
            for (int i = streamedChars.Count() - 1; i >= 0; i--)
            {
                // the first streamed character should match last character of words
                // second  streamed character should match 2nd lst character of words etc
                HashSet<int> currentSet = _matcher[streamedChars[i] + "" + j];
                if (prevSet != null)
                {
                    currentSet.Intersect(prevSet);
                }

                // if currentSet is empty then return false
                if (currentSet.Count == 0)
                { // if one iteration does not return any words then intersetion with previoud set will be empty
                    return false;
                }

                //check if current set has any word  of length i.. if yes that is solution
                if (ExistsWordOfGivenLength(currentSet, i))
                {
                    return true;
                }
            }
            return false;
        }

        private bool ExistsWordOfGivenLength(HashSet<int> words, int size)
        {
            foreach (int i in words)
            {
                if (_words[i].Length == size)
                {
                    return true;
                }
            }
            return false;
        }
    }

    /**
     * Your StreamChecker object will be instantiated and called as such:
     * StreamChecker obj = new StreamChecker(words);
     * bool param_1 = obj.Query(letter);
     */
}
