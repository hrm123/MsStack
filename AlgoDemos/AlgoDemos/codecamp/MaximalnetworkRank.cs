using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    /*
     * #1615
     * 166ms (beats 16% of C# users). 92 MB (beats 6% of C# users)
     */
    internal class MaximalnetworkRank
    {
        Dictionary<int, string> sourceDest = new Dictionary<int, string>();
        Dictionary<int, int> nodeDegree = new Dictionary<int, int>();
        int _n;
        int[][] _roads;

        public int MaximalNetworkRank(int n, int[][] roads)
        {
            _roads = roads;
            _n = n;
            for (int i = 0; i < n; i++)
            {
                sourceDest[i] = "";
            }

            int[] edge;
            for (int i = 0; i < roads.Length; i++)
            {
                edge = roads[i];
                //assuming roads is unique
                sourceDest[edge[0]] = sourceDest[edge[0]] + "," + edge[1];
                sourceDest[edge[1]] = sourceDest[edge[1]] + "," + edge[0];
            }

            foreach (var (key, value) in sourceDest)
            {
                sourceDest[key] = value + ",";
            }
            DisconnectedTrees();


            //calculate degree of each node
            foreach (var (key, value) in sourceDest)
            {
                nodeDegree[key] = value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Length;
                // Console.WriteLine($"source={key}, degree={nodeDegree[key]}");
            }

            /*  
            //iterate over sourceDest and calcualte ma network rank using nodeDegree
            int maxValue = 0;

            foreach(var (key,value) in sourceDest){
                int[] destns = value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select<string, int>(int.Parse).ToArray();
                foreach(int dest in destns){
                    int currentDegree = nodeDegree[key] + nodeDegree[dest] -1;
                    if(currentDegree > maxValue){
                        maxValue = currentDegree;
                    }
                }
            }
            */

            // get max and second max from list of degrees
            int first = 0, second = 0;
            int firstNode = 0, secondNode = 0;

            foreach (var (key, value) in nodeDegree)
            {
                if (value >= first)
                {
                    second = first;
                    secondNode = firstNode;
                    first = value;
                    firstNode = key;
                }
                if (value >= second && value < first)
                {
                    second = value;
                    secondNode = key;
                }
            }
            // Console.WriteLine($"firstNode={firstNode}, secondNode={secondNode}");
            if (first != second)
            {
                return first + second - (ExistsUnconnectedNodeToFirstNode(firstNode, secondNode) ? 0 : (NodesAreConnected(firstNode, secondNode) ? (ExistsNonNeighbour(firstNode, secondNode) ? 0 : 1) : 0));
            }
            else
            {
                int val1 = first + second - (ExistsUnconnectedNodeToFirstNode(firstNode, secondNode) ? 0 : (NodesAreConnected(firstNode, secondNode) ? (ExistsNonNeighbour(firstNode, secondNode) ? 0 : 1) : 0));
                int val2 = first + second - (ExistsUnconnectedNodeToFirstNode(secondNode, firstNode) ? 0 : (NodesAreConnected(secondNode, firstNode) ? (ExistsNonNeighbour(secondNode, firstNode) ? 0 : 1) : 0));
                return Math.Max(val1, val2);
            }
        }


        private bool ExistsNonNeighbour(int firstNode, int secondNode)
        {
            int secondDegree = nodeDegree[secondNode];
            bool exists = false;
            foreach (var (key, value) in nodeDegree)
            {
                if (value != secondDegree) { continue; }
                if (key == firstNode || key == secondNode)
                {
                    //same node
                    continue;
                }
                if (sourceDest[key].IndexOf($",{firstNode}") == -1)
                {
                    exists = true;
                    break;
                }
            }
            // Console.WriteLine($"ExistsNonNeighbour = {exists}");
            return exists;
        }

        private bool ExistsUnconnectedNodeToFirstNode(int firstNode, int secondNode)
        {
            // should be in different tree (have to change code)

            int secondDegree = nodeDegree[secondNode];
            bool exists = false;


            foreach (var (key, value) in nodeDegree)
            {
                if (value != secondDegree) { continue; }

                if (firstNode == key)
                {
                    //same node
                    continue;
                }

                int firstNodeTree = 0;
                int secondNodeTree = 0;

                foreach (var (key1, value1) in trees)
                {
                    if (value1.Contains(firstNode)) { firstNodeTree = key1; }
                    if (value1.Contains(secondNode)) { secondNodeTree = key1; }
                }
                if (firstNodeTree != secondNodeTree)
                {
                    // Console.WriteLine($"firstNode={firstNode}, secondNode={key} not connected");
                    exists = true;
                    break;
                }

            }
            return exists;
        }

        int root(int a)
        {
            // If current vertex is
            // the topmost vertex
            if (a == parent[a])
            {
                return a;
            }

            // Otherwise, set topmost vertex of
            // its parent as its topmost vertex
            return parent[a] = root(parent[a]);
        }

        void connect(int a, int b)
        {
            // Connect edges
            a = root(a);
            b = root(b);

            if (a != b)
            {
                parent[b] = a;
            }
        }

        int[] parent;
        Dictionary<int, HashSet<int>> trees;
        void DisconnectedTrees()
        {
            parent = new int[_n];
            trees = new Dictionary<int, HashSet<int>>();
            for (int i = 0; i < _n; i++)
            {
                parent[i] = i;
            }

            // Traverse all edges
            for (int i = 0; i < _roads.Count(); i++)
            {
                connect(_roads[i][0], _roads[i][1]);
            }

            HashSet<int> s = new HashSet<int>();

            // Traverse all vertices
            for (int i = 0; i < _n; i++)
            {

                //create dictionary entry if not already exists
                if (!trees.ContainsKey(root(i)))
                {
                    trees[root(i)] = new HashSet<int>();
                }

                trees[root(i)].Add(i);
            }

            /*
            foreach(var (key,value) in trees)
            {
                Console.WriteLine($"root={key} set={string.Join(",", value)}");
            }
            */

            return;
        }

        private bool NodesAreConnected(int firstNode, int secondNode)
        {
            // if(firstNode == secondNode) return false;
            // Console.WriteLine($"firstNode={firstNode}, secondNode={secondNode}");
            foreach (var (key, value) in sourceDest)
            {
                if (key == firstNode && value.IndexOf($",{secondNode},") != -1)
                {
                    return true;
                }
            }
            return false;
        }
    }