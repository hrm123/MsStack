using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp.strings
{
    https://www.geeksforgeeks.org/aho-corasick-algorithm-pattern-searching/
    public class AhoKorasick
    {
        static int MAXS = 600; // should be sum of the length of all keywords
        static int MAXC = 26; // max number of characters in alphabet
        static int[] outBits = new int[MAXS]; // Bit i in this mask is 1 if the word with index i appears when machine enters this state
        static int[] f = new int[MAXS]; // failure function
        /*
         * The suffix/failure links of the root vertex and all its immediate children point to the root vertex. For any vertex  
            v  deeper in the tree, we can calculate the suffix link as follows: if  
            p  is the ancestor of  v  with  c  being the letter labeling the edge from  
            p  to  v , go to  p , then follow its suffix link, and perform the transition with the letter  
            c  from there.
         */
        static int[,] g = new int[MAXS, MAXC]; // goto function (or trie) 


        public static void Testcase()
        {
            /*
            String[] arr = { "apple", "pie", "aunty" }; // { "he", "she", "hers", "his" };
            String text = "heateapplepieatauntyshouse"; // "ahishers";
            int k = arr.Length;
            */

            String[] arr = { "acc", "atc", "cat","gcg" }; 
            String text = "gcatcg"; 
            int k = arr.Length;

            Array.Fill(outBits, 0);
            for (int i = 0; i < MAXS; i++)
                for (int j = 0; j < MAXC; j++)
                    g[i, j] = -1;

            AhoKorasick.searchWords(arr, k, text);
        }  

    /// <summary>
    /// Builds string matching machine
    /// </summary>
    /// <param name="arr">array of words. index of each word in the array is important - outBits[state] &  (1 << i) >0 if we just found word[i] in the text</i></param>
    /// <param name="k"></param>
    /// <returns> number of states that the built machine has. States are numbered 0 up to the return value - 1, inclusive</returns>
    static int buildMachine(string[] arr, int k)
        {
            

            int states = 1; // only the 0 state

            //consider arr = ['apple','pie']

            //fill g[], which is same as building trie for arr[]
            for(int i = 0; i < k; ++i)
            {
                string word = arr[i];
                int currentState = 0;

                for (int j = 0; j < word.Length; j++)
                {
                    int ch = word[j] - 'a'; // gives 0-25 index of a-z.. so 'c' will be 2
                    
                    //allocate a new node (create a new state) if a node for ch does'nt exist
                    if (g[currentState,ch] == -1)
                    {
                        g[currentState, ch] = states++;
                    }
                    currentState = g[currentState, ch];
                } // adds the alphabets of current word starting from node '0'- i.e., set g[0, int_of_current_alphabet]to a value greater than zero which points to new state in the combines alphabets of all words

                // add index of  current word in output function (along with any other words index that may exist alrady there)
                outBits[currentState] |= (1 << i); // 1 << i is current word index set in binary format of 3 bit integer (so one of 32 bits will be value 1 others will be 0)
            }

            // For all characters which don't have
            // an edge from root (or state 0) in Trie (basically any characters not present in all of given words),
            // add a goto edge to state 0 (failed state) itself
            for (int ch = 0; ch < MAXC; ++ch)
                if (g[0, ch] == -1)
                    g[0, ch] = 0;

            Array.Fill(f, 0);

            //failure function is computed breadth first order on Trie
            Queue<int> q = new Queue<int>();

            //iterate over every possible input
            for(int ch = 0; ch < MAXC; ch++){
                // all nodes at depth 1 (root's immediate children) have failure function value as 0
                if (g[0,ch] != 0) // if current alphabet is at root node
                {
                    f[g[0, ch]] = 0;
                    q.Enqueue(g[0, ch]);
                }
                //all of level 1 children also point to 
            }

            while(q.Count != 0) // now q will have states that correspond to level 1 nodes
            {
                int state = q.Peek();
                q.Dequeue();
                // For the removed state, find failure
                // function for all those characters
                // for which goto function is
                // not defined.
                for (int ch = 0; ch < MAXC; ++ch)
                {

                    // If goto function is defined for
                    // character 'ch' and 'state'
                    if (g[state, ch] != -1)
                    {

                        // Find failure state of removed state
                        // note - there is only one failure state for given state (even though this state
                        // may have few characters transformed to another state and few characters resulting in failed state
                        // initially  failed state of first level nodes point to root node (represented by state = 0)
                        int failure = f[state]; // get failure state of parent of current [state, 'c'h'} which is state'' itself

                        // Find the deepest node labeled by
                        // proper suffix of String from root to
                        // current state.
                        while (g[failure, ch] == -1) // keep going upward on parents till root is reached or another in-between parent has the state the transforms to  'ch' in which case the resulting inbetween parents ch child node becomes failure link of current g[state,ch]  node
                            failure = f[failure];

                        failure = g[failure, ch];
                        f[g[state, ch]] = failure;

                        // Merge output values
                        outBits[g[state, ch]] |= outBits[failure];

                        // Insert the next level node
                        // (of Trie) in Queue
                        q.Enqueue(g[state, ch]);
                    }
                }
            }
            return states;


        }


        // Returns the next state the machine will transition to
        // using goto and failure functions. currentState - The
        // current state of the machine. Must be between
        //                0 and the number of states - 1,
        //                inclusive.
        // nextInput - The next character that enters into the
        // machine.
        static int findNextState(int currentState,
                                 char nextInput)
        {
            int answer = currentState;
            int ch = nextInput - 'a';

            // If goto is not defined, use
            // failure function
            while (g[answer, ch] == -1)
                answer = f[answer];

            return g[answer, ch];
        }

        // This function finds all occurrences of
        // all array words in text.
        static void searchWords(String[] arr, int k,
                                String text)
        {

            // Preprocess patterns.
            // Build machine with goto, failure
            // and output functions
            buildMachine(arr, k);

            // Initialize current state
            int currentState = 0;

            // Traverse the text through the
            // built machine to find all
            // occurrences of words in []arr
            for (int i = 0; i < text.Length; ++i)
            {
                currentState = findNextState(currentState,
                                             text[i]);

                // If match not found, move to next state
                if (outBits[currentState] == 0)
                    continue;

                // Match found, print all matching
                // words of []arr
                // using output function.
                for (int j = 0; j < k; ++j)
                {
                    if ((outBits[currentState] & (1 << j)) > 0)
                    {
                        Console.Write("Word " + arr[j] +
                                      " appears from " +
                                      (i - arr[j].Length + 1) +
                                      " to " + i + "\n");
                    }
                }
            }
        }

    }
}
