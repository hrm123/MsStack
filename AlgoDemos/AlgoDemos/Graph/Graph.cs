using AlgoDemos.ExpressionTree;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgoDemos.Graph
{
    public class Graph<T>
    {
        public GraphNode<T> RootNode { get; set; }

        public bool AddRootNode(GraphNode<T> root)
        {
            if (RootNode != null) return false;
            RootNode = root;
            return true;
        }

        /*
        public bool AddChildNode(GraphNode<T> parent, GraphNode<T> child)
        {
            if(parent == null || child == null)
            {
                return false;
            }
            parent.ChildNodes.Add(child);
            return true;
        }
        */
        
        private bool IsPalindrome(string word)
        {
            char[] wordChars = word.ToCharArray();
            int wordLength = word.Length -1;
            for (int i=0; i < wordLength; i++)
            {
                if (wordChars[i] != wordChars[wordLength - i]) return false;
            }
            return true;
        }

        BlockingCollection<string> palindromes = new BlockingCollection<string>();

        public string[] GetPalindromes()
        {
            GetPalindromes(RootNode, "");
            return palindromes.ToArray();
        }

        private void GetPalindromes(GraphNode<T> node, string pathTillNow)
        {
            // DFS search and in each dfs path from root to leaf node validate if that can be a palindrome
            // at the end of DFS path validate if that string is palindrome
            if (node.ChildNodes == null || node.ChildNodes.Count == 0)
            {
                if (IsPalindrome(pathTillNow))
                {
                    palindromes.Add(pathTillNow);
                }
                return;
            }
            foreach (var child in node.ChildNodes)
            {
                // Console.WriteLine(child.nodeIdentifier);
                GetPalindromes(child, pathTillNow + child.Data);
            }
        }

        public void PrintDFSPathsToConsole(GraphNode<T> node, string pathTillNow)
        {
            if(node.ChildNodes == null || node.ChildNodes.Count == 0)
            {
                Console.WriteLine(pathTillNow); // end of one DFS path
                return;
            }
            foreach(var child in node.ChildNodes)
            {
                // Console.WriteLine(child.nodeIdentifier);
                PrintDFSPathsToConsole(child, pathTillNow + child.nodeIdentifier + "-");
            }

        }

        public void PrintLevelOrderToConsole() //BFS
        {
            Queue<GraphNode<T>> queue = new Queue<GraphNode<T>>();
            queue.Enqueue(RootNode);
            int prevLevel = 1;
            while (queue.Count > 0)
            {
                GraphNode<T> tempNode = queue.Dequeue();
                int currentLevel = tempNode.Level;
                if (currentLevel != prevLevel)
                {
                    Console.WriteLine();
                }
                Console.Write(tempNode.nodeIdentifier  + " ");
                if (tempNode.ChildNodes != null)
                {
                    tempNode.ChildNodes.ForEach(c => queue.Enqueue(c));
                }
                prevLevel = currentLevel;
            }
            // PrintNode(1, RootNode as CharNode);
        }

        private List<GraphNode<T>> CopyChilds(List<GraphNode<T>>  childs)
        {
            return childs.Select(c => new GraphNode<T>(c.Data, c.Level)).ToList();
        }
        public List<GraphNode<T>> AddChildNodes(GraphNode<T> parent, List<GraphNode<T>> childs)
        {
            if (parent == null || childs == null)
            {
                return null;
            }

            var children = CopyChilds(childs);
            parent.ChildNodes = children;
            /*
            if (parent.ChildNodes != null)
            {
                parent.ChildNodes.AddRange(children);
            }
            else
            {
                parent.ChildNodes = children;
            }
            */
            return children;
        }
    }
}
