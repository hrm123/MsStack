using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    /*
     * Input
        nums =
        [1,2,3]
        Output
        [[],[1],[2],[3],[1,2],[2,3],[3,1],[1,2,3]]
        Expected
        [[],[1],[2],[1,2],[3],[1,3],[2,3],[1,2,3]]

    */
    public class Solution
    {
        IList<IList<int>> output = new List<IList<int>>();
        int[] _nums;
        int _n;
        List<string> outputS = new List<string>();

        public IList<IList<int>> Subsets(int[] nums)
        {
            _nums = nums;
            _n = _nums.Length;
            //add empty set
            List<int> empty = new List<int>();
            output.Add(empty);
            bool[] used = new bool[nums.Length];


            for (int i = 0; i < nums.Length; i++)
            {
                string s = "";

                for (int j = 0; j < nums.Length; j++)
                {
                    used[j] = false;
                }
                GenerateSubset(i + 1, 0, ref used, s);
            }
            foreach (string s in outputS)
            {
                IList<int> arr = new List<int>();
                output.Add(s.Split(",").Select(int.Parse).ToList());
            }
            return output;
        }

        // bs = batch size, cd = current depth, used
        private void GenerateSubset(int bs, int cd, ref bool[] used, string s)
        {
            if (cd == bs)
            {
                Console.WriteLine(s);
                outputS.Add(s);
                return;
            }
            for (int i = 0; i < _n; i++)
            {
                if (used[i] == true) continue;
                used[i] = true;
                GenerateSubset(bs, cd + 1, ref used, s.Length == 0 ? (_nums[i] + "") : s + "," + _nums[i]);
                used[i] = false;
            }
        }

    }
}
