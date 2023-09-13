using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    /// <summary>
    /// 1032. Stream of Characters - Class checks if given stream of characters ends with any of words given 
    /// First attempt - failed on basic tests (typo)
    /// Second Attempt - timed out after 15/18 testcases(because of typo)
    /// Third attempt - 2493 ms (beats 8% of C# users). 166 MB - beats 22% of C# users
    /// </summary>
    /// 
    public class StreamChecker
    {

        Dictionary<string, HashSet<int>> _matcher = new Dictionary<string, HashSet<int>>();
        string[] _words;
        int _minWordLength = 100000, _maxWordLength = 0;
        public string Reverse(string str)
        {
            char[] chars = str.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }

        public StreamChecker(string[] words)
        {
            _words = words;
            // create matcher = Hash<char+indexinword,set of ints> which gives index of all words that have particular character at particular position
            // have to reverse words since it is easier to match
            int indexOfword = 0;


            foreach (string wordReverse in words)
            {
                string wordCurrent = Reverse(wordReverse);
                _minWordLength = (wordCurrent.Length < _minWordLength) ? wordCurrent.Length : _minWordLength;
                _maxWordLength = (wordCurrent.Length > _maxWordLength) ? wordCurrent.Length : _maxWordLength;
                for (int i = 0; i < wordCurrent.Length; i++)
                {
                    HashSet<int> tmp = null;
                    string key = wordCurrent[i] + "" + i;
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

            Console.WriteLine($"hash created {_matcher.Count()} , minWordLength={_minWordLength}, maxWordLength={_maxWordLength}");
            /*
            foreach(var (key,value) in _matcher){
                Console.WriteLine($"key={key} - value={String.Join<int>(",", value)}");
            }
            */

        }


        List<char> streamedChars = new List<char>();  // store characters that are getting streamed

        public bool Query(char letter)
        {
            // Console.WriteLine($"-------------{letter}---------------------");
            HashSet<int> prevSet = null;
            int j = 0;
            streamedChars.Add(letter);
            if (streamedChars.Count() < _minWordLength) return false;
            HashSet<int> currentSet = null;
            HashSet<int> tmp = null;
            //  Console.WriteLine($"count={streamedChars.Count()}");
            for (int i = streamedChars.Count() - 1; i >= 0; i--)
            {
                /*
                if(j>_minWordLength){
                    Console.WriteLine("more than min word length");
                }
                */
                if (j > _maxWordLength)
                {
                    Console.WriteLine("more than max word length");
                }
                // the first streamed character should match last character of words
                // second  streamed character should match 2nd lst character of words etc
                string key = streamedChars[i] + "" + j;
                currentSet = null;
                if (_matcher.ContainsKey(key))
                {
                    currentSet = _matcher[key];
                    //Console.WriteLine($" key={key},  current set={ String.Join<int>(",", currentSet)}");   
                }
                else
                {
                    return false;
                }


                // if currentSet is empty then return false
                if (currentSet == null || currentSet.Count == 0)
                { // if one iteration does not return any words then intersetion with previoud set will be empty
                    return false;
                }


                //check if current set has any word  of length i.. if yes that is solution
                tmp = new HashSet<int>(currentSet);
                if (prevSet != null)
                {
                    tmp.IntersectWith(prevSet);
                    if (tmp == null || tmp.Count == 0)
                    {
                        return false;
                    }
                    // Console.WriteLine($" intersect set={ String.Join<int>(",", tmp)}"); 
                    //check if current set has any word  of length i.. if yes that is solution
                    if (ExistsWordOfGivenLength(tmp, j + 1))
                    {
                        // Console.WriteLine($"ExistsWordOfGivenLength {j+1} second");
                        return true;
                    }
                }
                else
                {
                    if (ExistsWordOfGivenLength(currentSet, j + 1))
                    {
                        // Console.WriteLine($"ExistsWordOfGivenLength {j+1}");
                        return true;
                    }
                }


                prevSet = tmp;
                j++;
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
    /**
     * Your StreamChecker object will be instantiated and called as such:
     * StreamChecker obj = new StreamChecker(words);
     * bool param_1 = obj.Query(letter);
     */
    /**
     * Your StreamChecker object will be instantiated and called as such:
     * StreamChecker obj = new StreamChecker(words);
     * bool param_1 = obj.Query(letter);
     */

    /**
     * Your StreamChecker object will be instantiated and called as such:
     * StreamChecker obj = new StreamChecker(words);
     * bool param_1 = obj.Query(letter);
     */
}
