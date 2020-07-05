using AlgoDemos.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AlgoDemos.puzzles
{
    public class AllPalindromes
    {
        Graph<char> fLettersTree;

        public void Demo()
        {
            string word = "mdaam";
            string[] allPalindromes = FindAllPalindromes(word);
            int minimmumSwaps = FindMinimumSwapsReqd(word, allPalindromes);
            Console.WriteLine("minimmumSwaps - " + minimmumSwaps);
            Console.ReadKey();
        }

        private int FindNumberOfSwaps(string source, string palindrome)
        {
            char[] palindromeCharArray = palindrome.ToCharArray();
            List<char> sourceChars = source.ToCharArray().ToList();
            int swapCount = 0;

            for (int i=0;i< palindromeCharArray.Length; i++)
            {
                if (sourceChars[i] == palindromeCharArray[i]) continue;
                int pointer = sourceChars.IndexOf(palindromeCharArray[i]);
                swapCount += pointer - i;
                sourceChars[pointer] = sourceChars[i];
                sourceChars[i] = '\0'; //make already matched chars from source as null
            }
            return swapCount;
        }


        private void DataStructures()
        {
            Stack<string> reverseWord = new Stack<string>();
            Queue<string> cellphoneOrders = new Queue<string>();
            LinkedList<string> sentence = new LinkedList<string>();
            Dictionary<string, string> hashWordMeanings = new Dictionary<string, string>();
            // hashtable uses - fast data lookup, indexing the database, caches, unique data representation
            Array integersArray = Array.CreateInstance(typeof(Int32), 5);
            integersArray.SetValue(33,0);
            integersArray.SetValue(23, 1);
            integersArray.SetValue(13, 2);
            integersArray.SetValue(83, 3);
            integersArray.SetValue(63, 4);
            Array.BinarySearch(integersArray, 63); // o(n) search
            Array.Sort(integersArray); // sorts the array
            Array.BinarySearch(integersArray, 63); // o(log n) search
            


        }
        public int FindMinimumSwapsReqd(string source, string[] allPalindromes)
        {
            int minimumSwaps = -1;

            foreach (string palindrome in allPalindromes.Distinct())
            {
                int numSwaps = FindNumberOfSwaps(source, palindrome);
                if(minimumSwaps == -1 && numSwaps > -1)
                {
                    minimumSwaps = numSwaps;
                }
                minimumSwaps = numSwaps < minimumSwaps ? numSwaps : minimumSwaps;
            }
            return minimumSwaps;

        }

        public string[] FindAllPalindromes(string source)
        {
            string[] allPalindromes = null;

            // create a tree of all possible combination of letters
            // root is empty; root has N childs which are each letter of the source
            //each child will have childs that are letter of source not sued by their parents
            fLettersTree = new Graph<char>();
            GraphNode<char> rootNode = new GraphNode<char>('\0', 1);
            fLettersTree.AddRootNode(rootNode);
            CreateTreeOfLetters(source, rootNode);

            // fLettersTree.PrintDFSPathsToConsole(rootNode, "");

            allPalindromes = fLettersTree.GetPalindromes();
            foreach(string palindrome in allPalindromes)
            {
                Console.WriteLine(palindrome);
            }
            
            Console.ReadKey();

            //now that we have the tree of all possible words of same length prune the tree for palindromes


            return allPalindromes;
        }


        private void CreateTreeOfLetters(string word, GraphNode<char> parent)
        {
            if(word.Length == 0)
            {
                return; //end of recursion
            }

            /*
            char[] childrenChars = (parent.Level == 1) ? word.ToCharArray() :  word.Substring(1).ToCharArray();
            // parent.Data = word.Substring(0, 1)[0];
            if(childrenChars.Length == 0)
            {
                return;
            }
            */

            List<GraphNode<char>> childObjs =
                word.AsEnumerable().Select(c1 => new GraphNode<char>(c1, parent.Level+1)).ToList();

            childObjs = fLettersTree.AddChildNodes(parent, childObjs); //actual child objects of tree are returned

            foreach(var childObj in childObjs)
            {
                int itr = 0; // remove only first occurence of childObj data
                CreateTreeOfLetters(new string(word.AsEnumerable().Where(c => !(c == childObj.Data && itr++ == 0)  ).ToArray()), childObj);
                itr = 0;
            }

            /*
            childObjs.ForEach(child =>
            {
                CreateTreeOfLetters(word.Substring(1), child);
            });
            */

            
        }

    }
}
