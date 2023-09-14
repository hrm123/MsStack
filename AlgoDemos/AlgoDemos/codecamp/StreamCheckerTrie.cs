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

    // 590 ms - beats 42% of users with C# / 149 MB - beats 42% of C# users


    public class StreamCheckerTrie
    {

        string[] _words;
        int[,] _trie;
        int[] _wordsAtState;
        int MAX_ALPHABETS = 26;
        int _totalStates = 0;

        public string Reverse(string str)
        {
            char[] chars = str.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }


        public StreamCheckerTrie(string[] words)
        {
            _words = words;
            int totalStates = 0;
            foreach (var word in words)
            {
                totalStates += word.Length;
            }
            totalStates += 1; // accounts for root state / 0 state (which is 0 row which has 26 columns (all alphabets))
            _totalStates = totalStates;
            _trie = new int[totalStates, MAX_ALPHABETS]; // each character is one state
            _wordsAtState = new int[totalStates];
            Array.Fill(_wordsAtState, 0);
            for (int i = 0; i < totalStates; i++)
            {
                for (int j = 0; j < MAX_ALPHABETS; j++)
                {
                    _trie[i, j] = -1;
                }
            }

            int state = 1;
            int currentWordIndex = 0;
            foreach (string wordReverse in words)
            {
                string wordCurrent = Reverse(wordReverse);
                int currentState = 0;
                foreach (char c in wordCurrent)
                {
                    if (_trie[currentState, c - 'a'] == -1)
                    {
                        _trie[currentState, c - 'a'] = state++; // create node
                        // Console.WriteLine($" {currentState}-{_trie[currentState,c-'a']}={c}");
                    }
                    currentState = _trie[currentState, c - 'a'];
                }
                _wordsAtState[currentState] |= 1 << currentWordIndex;
                /*
                for(int i=0;i< _words.Length;i++){
                    if((_wordsAtState[currentState] & (1 << i)) > 0){
                        Console.WriteLine($"{currentState} stored {_words[i]}");
                    }
                }
                */

                currentWordIndex++;
            }
        }


        List<char> streamedChars = new List<char>();  // store characters that are getting streamed

        public bool Query(char letter)
        {
            // Console.WriteLine($"-------------{letter}---------------------");

            streamedChars.Add(letter);

            int currentState = 0;
            for (int i = streamedChars.Count() - 1; i >= 0; i--)
            {
                int ch = streamedChars[i] - 'a';
                int nextState = _trie[currentState, ch];
                if (nextState == -1)
                {
                    return false;
                }

                if ((_wordsAtState[nextState] ^ 0) != 0)
                {
                    return true;
                }

                /*
                for(int y=0;y< _words.Length;y++){
                    if((_wordsAtState[nextState] & (1 << y)) > 0){
                        Console.WriteLine($"{nextState} stored {_words[y]}");
                        return true;
                    }
                }
                */

                currentState = nextState;
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
