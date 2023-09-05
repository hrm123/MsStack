using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    /*
     * 2369 
     * 355 ms (beats 5% of users with C# 76 MB (beats 5% of users with c#)
     */
    internal class ValidPartitionSoln
    {
        int _n;
        int[] _nums;
        Dictionary<string, bool> _cache = new Dictionary<string, bool>();

        public bool ValidPartition(int[] nums)
        {
            _n = nums.Length;
            _nums = nums;
            return HasValidPartition(0, _n - 1);
        }

        bool isValid(int start, int end)
        {
            if (end > _n - 1)
            {
                return false;
            }
            if (end == start + 1)
            { // 2 chars
                return _nums[start] == _nums[end];
            }
            if (end == start + 2)
            { //3 chars
                return (_nums[start] == _nums[start + 1] && _nums[start + 1] == _nums[end]) ||
                    (_nums[end] == _nums[start + 1] + 1 && _nums[start + 1] == _nums[start] + 1);
            }
            return false;
        }

        bool HasValidPartition(int start, int end)
        {

            string key = start + "," + end;
            if (!_cache.ContainsKey(key))
            {
                _cache[key] = true;
            }
            else
            {
                return _cache[key];
            }

            if (end - start + 1 <= 0)
            {
                //nothing left in nums.. so valid ending
                return true;
            }
            if (end - start + 1 < 2)
            {
                _cache[key] = false;
                return false;
            }

            if (isValid(start, start + 1))
            {
                if (HasValidPartition(start + 2, end))
                {
                    return true;
                }
            }
            if (isValid(start, start + 2))
            {
                bool output = HasValidPartition(start + 3, end);
                _cache[key] = output;
                return output;
            }

            _cache[key] = false;
            return false;
        }
    }
}
