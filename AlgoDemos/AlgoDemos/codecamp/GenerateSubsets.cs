using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    public class GenerateSubsets
    {
        /*
         *  time - 125 ms - beats 90% of users with C#. Memory 43.13 - beats 16% of users with C#
         */
        IList<IList<int>> output = new List<IList<int>>();
        int[] _nums;
        int _n;

        public IList<IList<int>> Subsets(int[] nums)
        {
            _nums = nums;
            _n = _nums.Length;
            //add empty set
            List<int> empty = new List<int>();
            output.Add(empty);

            IList<int> arr = new List<int>();
            for (int i = 0; i < nums.Length; i++)
            {
                arr.Clear();
                GenerateSubset(i + 1, 0, -1, arr);
            }
            return output;
        }

        // bs = batch size, cd = current depth, used
        private void GenerateSubset(int bs, int cd, int prevIndex, IList<int> arr)
        {
            if (cd == bs)
            {
                // Console.WriteLine(s);
                IList<int> arrStored = arr.ToList();
                output.Add(arrStored);
                return;
            }
            for (int i = 0; i < _n; i++)
            {
                if (i <= prevIndex) continue;
                arr.Add(_nums[i]);
                GenerateSubset(bs, cd + 1, i, arr);
                arr.RemoveAt(arr.Count - 1);
            }
        }

    }
}
