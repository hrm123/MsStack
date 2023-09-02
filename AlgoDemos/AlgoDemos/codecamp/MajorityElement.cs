using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    /*
     * Runtime 101ms (beats 60% of C# users). memory 44 MB (beats 60% of C# users)
     */
    public class MajorityElementSoln
    {
        int[] _nums;
        private void swap(int l, int r)
        {
            int temp = _nums[l];
            _nums[l] = _nums[r];
            _nums[r] = temp;
        }
        public int MajorityElement(int[] nums)
        {
            int n = nums.Length;
            _nums = nums;
            if (n == 1 || n == 2) { return nums[0]; }
            int avg = (int)Math.Floor(nums.Average());
            int l = 0, r = n - 1;
            while (l < r)
            {
                if (nums[l] > avg)
                {
                    swap(l, r);
                    r--;
                }
                else
                {
                    l++;
                }
            }

            if (nums[n / 2] == nums[n / 2 + 1])
            {
                return nums[n / 2];
            }
            else
            {
                return nums[n / 2 - 1];
            }
            return nums[n / 2];
        }
    }
}
