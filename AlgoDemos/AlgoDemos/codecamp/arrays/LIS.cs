using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp.arrays
{
    /// <summary>
    /// 300. Longest Increasing Subsequence
    /// solution - do mergesort to sort array elements while keeping the index of original array.  Then go through sorted array
    /// and get the indexes that are max apart
    /// </summary>
    public class LIS
    {

        int[] _arr; // value, index

        public static void TestLIS()
        {
            int[] arr = new int[] { 10, 9, 2, 5, 3, 7, 101, 18}; // 3, 8, 3, 5, 8, 3, 9 };
            LIS lis = new LIS();
            lis.LengthOfLIS(arr);

        }

        public int LengthOfLIS(int[] nums)
        {

            int n = nums.Length;
            List<int> list = new List<int>();
            list.Add(nums[0]);
            for (int i = 1; i < n; i++)
            {
                if (nums[i] > list[list.Count() - 1])
                {
                    list.Add(nums[i]);
                }
                else
                {
                    int low = 0;
                    int high = list.Count() - 1;
                    while (low <= high)
                    {
                        int mid = (high + low) / 2;
                        if (list[mid] < nums[i])
                        {
                            low = mid + 1;
                        }
                        else
                        {
                            high = mid - 1;
                        }
                    }
                    list[low] =  nums[i];
                }
            }
            return list.Count();
        }





    }
}
