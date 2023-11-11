using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp.strings
{

    
    public class StateNode
    {
        static int MAXC = 26; // max number of characters in alphabet
        public int state { get; }
        public Dictionary<int, StateNode> transitions = new Dictionary<int, StateNode>();
        public StateNode failure;
        public List<string> words = new List<string>();
        public StateNode(int state)
        {
            this.state = state;
            for (int i = 0; i < MAXC; i++)
            {
                transitions[i] = (state == 0)? this: null;
            }
            if(state == 0) { failure = this; }
        }
    }

    /// <summary>
    /// Ex puzzles - Is given string s in the original n string? / SET  (of strings) OPERATIONS - Add(or)Delete(or)Search string s from SET S
    /// </summary>
    public class AhoCorasickTrie
    {
        StateNode _root = new StateNode(0);
        static int MAXC = 26; // max number of characters in alphabet
        char[] alphs = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        public static void Testcase()
        {
            /*
            String[] arr = { "apple", "pie", "aunty" }; // { "he", "she", "hers", "his" };
            String text = "heateapplepieatauntyshouse"; // "ahishers";
            int k = arr.Length;
            */

            String[] arr = { "acc", "atc", "cat", "gcg" };
            String text = "gcatcg";


            AhoCorasickTrie act = new AhoCorasickTrie();
            var answer = act.SearchWords(arr, text);
        }

        public List<Tuple<int, int>> SearchWords(String[] arr, String text)
        {
            List<Tuple<int, int>> wordsMatching = new List<Tuple<int, int>>();
            CreateTrie(arr);

            DebugStateTrie();

            //search for matches
            var currentState = _root;
            for(int i=0;i< text.Length;i++)
            {
                char ch = text[i];
                currentState = FindNextState(currentState, ch);
                foreach(var word in currentState.words)
                {
                    wordsMatching.Add(new Tuple<int, int>(i - word.Length+1, i));
                }

            }
            return wordsMatching;
        }

        private void DebugStateTrie()
        {
            Queue<StateNode> q = new Queue<StateNode>();
            q.Enqueue( _root );
            StringBuilder sb = new StringBuilder();
            while (q.Count() > 0)
            {
                var current = q.Dequeue();
                sb.Append($"{current.state}-");
                for (int ch = 0; ch < MAXC; ch++)
                {
                    var next = current.transitions[ch];
                    if (next!=null && next.state != 0) // has valid transition from current node
                    {
                        q.Enqueue(next);
                        sb.Append($"{alphs[ch]}-{next.state}-{next.failure.state},");
                    }
                }
                sb.AppendLine();
            }
            Console.Write( sb.ToString() );
        }

        private StateNode FindNextState(StateNode currentState, char ch)
        {
            var answer = currentState;
            int c = ch - 'a';
            while (answer.transitions[c] == null) // if no goto function exists go to failure function .. goes on till we reach root state
            {
                answer = answer.failure; 
            }
            return answer.transitions[c]; // could be root node or another node

        }
        private void CreateTrie(string[] arr)
        {
            int states = 1;
            foreach (string word in arr)
            {
                StateNode current = _root;
                
                for (int i = 0; i < word.Length; i++)
                {
                    int ch = word[i]  - 'a'; // gives 0-25 index of a-z.. so 'c' will be 2
                    if (current.transitions[ch] == null || current.transitions[ch].state == 0)
                    {
                        current.transitions[ch] = new StateNode(states++);
                    }
                    current = current.transitions[ch];

                }
                current.words.Add(word);
            }

            BuildFailureLinks();
        }

        private void BuildFailureLinks()
        {
            // for root state failure links already set in constructor - all go to root state itself if there is not transform for specific character
            //do BFS search to set failure links of nodes level by level
            Queue<StateNode> q = new Queue<StateNode>();
            for (int ch = 0; ch < MAXC; ch++)
            {
                if (_root.transitions[ch].state != 0) // has valid transition from root node
                {
                    q.Enqueue(_root.transitions[ch]);
                    _root.transitions[ch].failure = _root; // first level nodes failure go to root node
                }
            }

            while (q.Count() > 0)
            {
                StateNode currentNode = q.Dequeue();
                for (int ch = 0; ch < MAXC; ch++)
                {
                    if (currentNode.transitions[ch] != null && currentNode.transitions[ch].state != 0)
                    {  // has valid transition from current node- need to get failure function for the child node
                        var failureNode = currentNode.failure;
                        while(failureNode.transitions[ch] == null)
                        {
                            failureNode = failureNode.failure;
                        }
                        currentNode.transitions[ch].failure = failureNode.transitions[ch]; // failurenode could be root node or one of other nodes in climb which have valid transition state
                        q.Enqueue(currentNode.transitions[ch]);
                    }
                }
            }
        }
    }

}
