using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp.arrays
{
    /// <summary>
    /// Given an integer array nums of unique elements, return all possible subsets (the power set).
    /// The solution set must not contain duplicate subsets. Return the solution in any order.
    /// Input: nums = [1,2,3] then Output: [[],[1],[2],[1,2],[3],[1,3],[2,3],[1,2,3]]
    /// 118 ms - beats 96% of c# users. 42 MB - beats 69% of C# users.
    /// </summary>
    public class SubsetsOfInts
    {

        int[] _nums;
        IList<IList<int>> _responseActual;
        int _len;


        public IList<IList<int>> Subsets(int[] nums)
        {
            _nums = nums;
            _len = _nums.Length;
            _responseActual = new List<IList<int>>();
            //add empty set
            List<int> empty = new List<int>();
            _responseActual.Add(empty);
            SubsetRecursive( 0, new List<int>());
            return _responseActual;

        }

        void SubsetRecursive(int prevIndex, IList<int> currentArr)
        {
            if(prevIndex == _len)
            {
                return;
            }
            for (int i = prevIndex; i < _len; i++)
            {
                currentArr.Add(_nums[i]);
                _responseActual.Add(currentArr.ToList());
                SubsetRecursive(i + 1, currentArr);
                currentArr.Remove(_nums[i]);
            }

        }
    }

    /// <summary>
    /// 121 ms - beats 91.81% of users with C#. 42.88 MB - beats 69.86% of C# users
    /// </summary>
    public class SolutionAnother
    {
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
