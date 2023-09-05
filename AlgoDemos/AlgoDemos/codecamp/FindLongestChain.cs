using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    /* 646 Maximum length of pair chain
     * 116ms (beats 92% users with C#)  50 MB (beats 10% users withC#)
     */
    internal class FindLongestChainSoln
    {
        public int FindLongestChain(int[][] pairs)
        {
            PriorityQueue<int[], int> pq = new PriorityQueue<int[], int>();
            int n = pairs.Length;
            for (int i = 0; i < n; i++)
            {
                pq.Enqueue(pairs[i], pairs[i][1]);
            }
            int target = -100000;
            int iter = 0;
            while (pq.TryDequeue(out int[] current, out int priority))
            {
                if (target < current[0])
                {
                    target = current[1];
                    iter++;
                }
            }
            return iter;
        }
    }
}
