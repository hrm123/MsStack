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
        public static void TestCase1()
        {
            int[][] arr = new int[][] { new int[] { 4, 7 }, new int[] { 4, 8 }, new int[] { 5, 6 }, new int[] { 1, 6 }, new int[] { 3, 7 }, new int[] { 2, 5 }, new int[] { 5, 8 }, new int[] { 1, 2 }, new int[] { 4, 9 }, new int[] { 6, 10 }, new int[] { 8, 10 }, new int[] { 3, 6 }, new int[] { 2, 10 }, new int[] { 9, 10 }, new int[] { 3, 9 }, new int[] { 2, 3 }, new int[] { 1, 9 }, new int[] { 4, 6 }, new int[] { 5, 7 }, new int[] { 3, 8 }, new int[] { 1, 8 }, new int[] { 1, 7 }, new int[] { 2, 4 } };
            int n = 10;

            var pbs = new PossibleBipartitionSln();
            Console.WriteLine($"Answer = {pbs.PossibleBipartition(n, arr)}");
        }

        public static void TestCase()
        {
            int[][] arr = new int[][] { new int[] { 1,2 }, new int[] { 1,3 }, new int[] { 2,3 }};
            int n = 3;

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
            }
            Dictionary<int, bool> visited = new Dictionary<int, bool>();
            Dictionary<int, bool> recursedNodes = new Dictionary<int, bool>();
            for (var i = 1; i <= N; i++)
            {
                visited.Clear();
                recursedNodes.Clear();
                if (!DFS(graph, i, visited, recursedNodes))
                {
                    return false;
                }
            }
            return true;
        }

        private bool DFS(List<int>[] graph, int node, Dictionary<int, bool> visited, Dictionary<int, bool> recursedNodes  )
        {
            if (recursedNodes.ContainsKey(node))
            {
                return true; // cycle is there
            }
            if (visited.ContainsKey(node))
            {
                return false; //no need to further explore from here
            }

            recursedNodes[node] = true;
            visited[node] = true;

            foreach (var neighbor in graph[node])
            {
                if (!DFS(graph, neighbor, visited, recursedNodes))
                {
                    return false;
                }
            }

            recursedNodes.Remove(node);

            return true;
        }
    }
}
