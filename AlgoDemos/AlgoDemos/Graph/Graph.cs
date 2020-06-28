using AlgoDemos.ExpressionTree;
using System;
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
