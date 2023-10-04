using AlgoDemos.ExpressionTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    /// <summary>
    /// 886. Possible Bipartition
    /// </summary>
    public class PossibleBipartitionSln
    {
        public static void TestCase()
        {
            int[][] arr = new int[][] { new int[] { 4, 7 }, new int[] { 4, 8 }, new int[] { 5, 6 }, new int[] { 1, 6 }, new int[] { 3, 7 }, new int[] { 2, 5 }, new int[] { 5, 8 }, new int[] { 1, 2 }, new int[] { 4, 9 }, new int[] { 6, 10 }, new int[] { 8, 10 }, new int[] { 3, 6 }, new int[] { 2, 10 }, new int[] { 9, 10 }, new int[] { 3, 9 }, new int[] { 2, 3 }, new int[] { 1, 9 }, new int[] { 4, 6 }, new int[] { 5, 7 }, new int[] { 3, 8 }, new int[] { 1, 8 }, new int[] { 1, 7 }, new int[] { 2, 4 } };
            int n = 10;

            var pbs = new PossibleBipartitionSln();
            Console.WriteLine($"Answer = {pbs.PossibleBipartition(n, arr)}");
        }

        public static void TestCase2()
        {
            int[][] arr = new int[][] { new int[] { 1,2 }, new int[] { 1,3 }, new int[] { 2,3 }};
            int n = 3;

            var pbs = new PossibleBipartitionSln();
            Console.WriteLine($"Answer = {pbs.PossibleBipartition(n, arr)}");
        }

        public static void TestCase1()
        {
            int[][] arr = new int[][] { new int[] { 1, 2 }, new int[] { 1, 3 }, new int[] { 2, 4 } };
            int n = 4;

            var pbs = new PossibleBipartitionSln();
            Console.WriteLine($"Answer = {pbs.PossibleBipartition(n, arr)}");
        }



        public bool PossibleBipartition(int N, int[][] dislikes)
        {
            var graph = new List<int>[N + 1];
            for (var i = 1; i <= N; i++)
            {
                graph[i] = new List<int>();
            }
            foreach (var dislike in dislikes)
            {
                graph[dislike[0]].Add(dislike[1]);
                graph[dislike[1]].Add(dislike[0]);
            }
            Dictionary<int, bool> visited = new Dictionary<int, bool>();
            for (var i = 1; i <= N; i++)
            {
                if (!visited.ContainsKey(i))
                {
                    if (IsCycle(graph, i, visited, -1))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool IsCycle(List<int>[] graph, int node, Dictionary<int, bool> visited, int parent  )
        {
            if(visited.ContainsKey(node) )
            {
                return true;
            }

            visited[node] = true;
            
            foreach (var neighbor in graph[node])
            {
                if (!visited.ContainsKey(neighbor))
                {
                    if (neighbor == parent)
                    {
                        continue;
                    }
                    if (IsCycle(graph, neighbor, visited, node))
                    {
                        if (visited.Count % 2 != 0)
                        {
                            return true;
                        }
                    }
                }
                else if (neighbor != parent)
                {
                    if (visited.Count % 2 != 0)
                    {
                        return true;
                    }
                }
                    
            }


            return false;
        }
    }
}
