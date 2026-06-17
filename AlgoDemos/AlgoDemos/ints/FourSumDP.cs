using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.ints
{
    public class FourSumDP
    {
        int _n;
        int[] _arr;
        Dictionary<int, HashSet<int>> _numsSet = new();
        Dictionary<string, IList<IList<int>>> _TwoSumCache = new Dictionary<string, IList<IList<int>>>();
        
        private IList<IList<int>>  AppendToLists(int cur, IList<IList<int>>  curList)
        {
            IList<IList<int>> out10 = new List<IList<int>>();
            HashSet<string> uniqueResponse = new();
            foreach (var lst in curList)
            {
                var newList = new List<int>(lst);
                newList.Add(cur);
                newList.Sort();
                uniqueResponse.Add(string.Join(",", newList.ToArray()));
            }
            foreach(var resp in uniqueResponse)
            {
                out10.Add(new List<int>(resp.Split(",").Select(s => int.Parse(s))));
            }
            return out10;
        }


        private HashSet<ValueTuple<int, int>> TwoSum(int target)
        {
            HashSet<ValueTuple<int, int>> response = new();
            for (int y = 0; y < _n; y++)
            {
                int curNum = _arr[y];
                if (_numsSet.ContainsKey(target - curNum))
                {
                    var validMatchExists = _numsSet[target - curNum].Any(a => a != y);
                    if (validMatchExists)
                    {
                        response.Add(
                            curNum > target - curNum ? new ValueTuple<int, int>(target - curNum, curNum) : new ValueTuple<int, int>(curNum, target - curNum)
                            );
                    }
                }
            }

            return response;
        } 

        private HashSet<string> FourSumRecursive(int target, int left)
        {
            HashSet<string> ol = new();
            if (left > _n - 1)
            {
                return ol;
            }
            if(left == _n - 2)
            {
                ol = TwoSum();
                return ol;
            }

            int cur = _arr[left];

            ol = FourSumRecursive(left + 1, target - cur);
            
            if (cur != 0)
            {
                HashSet<string> ol1 = FourSumRecursive(left+1,target);
                ol.Add(ol1);
            }
        }
        public IList<IList<int>> FourSum(int[] nums, int target)
        {
            _arr = nums;
            _n = nums.Length;
            
            for (int y = 0; y < _n; y++)
            {
                if (_numsSet.ContainsKey(nums[y]))
                {
                    _numsSet[nums[y]].Add(y);
                }
                else
                {
                    _numsSet[nums[y]] = new HashSet<int> { y };
                }
            }

            IList<IList<int>> response = new List<IList<int>>();
            var finalresponse = FourSumRecursive(target, 0);
            foreach(var str in finalresponse){
                response.Add(str.Split(",").Select(s => int.Parse(s)).ToList());
            }
            return response;
        }
}
