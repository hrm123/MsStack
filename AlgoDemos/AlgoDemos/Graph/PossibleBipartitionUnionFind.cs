using AlgoDemos.ExpressionTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AlgoDemos.Graph
{
    /// <summary>
    /// 886. Possible Bipartition - 311 ms - beats 8% c# users. 76MB - beats 10% c# users
    /// </summary>
    public class PossibleBipartitionSlnUF
    {
        public static void TestCase()
        {
            int[][] arr = new int[][] { new int[] { 4, 7 }, new int[] { 4, 8 }, new int[] { 5, 6 }, new int[] { 1, 6 }, new int[] { 3, 7 }, new int[] { 2, 5 }, new int[] { 5, 8 }, new int[] { 1, 2 }, new int[] { 4, 9 }, new int[] { 6, 10 }, new int[] { 8, 10 }, new int[] { 3, 6 }, new int[] { 2, 10 }, new int[] { 9, 10 }, new int[] { 3, 9 }, new int[] { 2, 3 }, new int[] { 1, 9 }, new int[] { 4, 6 }, new int[] { 5, 7 }, new int[] { 3, 8 }, new int[] { 1, 8 }, new int[] { 1, 7 }, new int[] { 2, 4 } };
            int n = 10;

            var pbs = new PossibleBipartitionSlnUF();
            Console.WriteLine($"Answer = {pbs.PossibleBipartition(n, arr)}");
        }

        public static void TestCase2()
        {
            int[][] arr = new int[][] { new int[] { 1, 2 }, new int[] { 1, 3 }, new int[] { 2, 3 } };
            int n = 3;

            var pbs = new PossibleBipartitionSlnUF();
            Console.WriteLine($"Answer = {pbs.PossibleBipartition(n, arr)}");
        }

        public static void TestCase1()
        {
            int[][] arr = new int[][] { new int[] { 1, 2 }, new int[] { 1, 3 }, new int[] { 2, 4 } };
            int n = 4;

            var pbs = new PossibleBipartitionSlnUF();
            Console.WriteLine($"Answer = {pbs.PossibleBipartition(n, arr)}");
        }

        int[] groups;
        public bool PossibleBipartition(int N, int[][] dislikes)
        {
            groups = new int[N + 1]; // +1 just to match numebrs of person.. 0 will be discarded
            for (int i = 0; i < groups.Length; i++)
            {
                groups[i] = i;
            }

            for (int i = 0; i < dislikes.Length; i++)
            {
                int p1 = dislikes[i][0];
                int p2 = dislikes[i][1];
                if (Find(p2) != Find(p1))
                {
                    // make them in same group
                    Union(p1, p2);
                }
            }

            Dictionary<int, int> counts = new Dictionary<int, int>();
            for (int i = 1; i < groups.Length; i++)
            {
                counts[groups[i]] = counts.ContainsKey(counts[groups[i]]) ? (1 + counts[groups[i]]) : 1;
            }

            foreach (var (key, value) in counts)
            {
                Console.WriteLine($"{key} - {value}");
            }


            foreach (var (key, value) in counts)
            {
                if (value % 2 == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public void Union(int x, int y)
        {
            groups[Find(y)] = groups[Find(x)];
        }

        public int Find(int x)
        {
            if (x != groups[x])
            {
                groups[x] = Find(groups[x]);
            }
            return groups[x];
        }

    }
}
