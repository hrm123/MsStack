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
            FindAllPalindromes("mdw");

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

            fLettersTree.PrintDFSPathsToConsole(rootNode, "");
            Console.ReadKey();
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
                CreateTreeOfLetters(new string(word.AsEnumerable().Where(c => c != childObj.Data).ToArray()), childObj);
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
