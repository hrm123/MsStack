using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    /// <summary>
    /// 2111. Minimum Operations to Make the Array K-Increasing
    /// </summary>
    public class kincreasing
    {
        public static void Testcase()
        {
            kincreasing soln = new kincreasing();
            // int answer = soln.k_increasing(new int[] { 5, 4, 3, 2, 1 }, 1);
            // int answer = soln.k_increasing(new int[] { 4, 1, 5, 2, 6, 2 }, 2);
            int answer = soln.k_increasing(new int[] { 2, 2, 2, 2, 2, 1, 1, 4, 4, 3, 3, 3, 3, 3 }, 1);
            


        }
        public int k_increasing(int[] arr, int k)
        {
            //create K subarray that need ot be in non-decreasing order
            List<List<int>> subarrays = new List<List<int>>();
            for (int i = 0; i < k; i++)
            {
                List<int> lst = new List<int>();
                subarrays.Add(lst);
                for (int j = i; j < arr.Count(); j += k)
                {
                    lst.Add(arr[j]);
                }
            }
            int lisCount = 0;
            foreach (List<int> current in subarrays)
            {
                lisCount += LengthOfLISWithRepetition(current.ToArray());
            }
            return arr.Length - lisCount;
        }
        public int LengthOfLISWithRepetition(int[] nums)
        {

            int n = nums.Length;
            List<int> list = new List<int>();
            list.Add(nums[0]);
            for (int i = 1; i < n; i++)
            {
                if (nums[i] >= list[list.Count() - 1])
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
                        if (list[mid] <= nums[i])
                        {
                            low = mid + 1;
                        }
                        else
                        {
                            high = mid - 1;
                        }
                    }
                    /*
                    if (list[low] == nums[i] && low == list.Count() -1 ) // dupe at end of list
                    {
                        list.Insert(low, nums[i]);
                    }
                    else
                    {
                    */
                        list[low] = nums[i];
                    //}
                }
            }
            return list.Count();
        }



    }
}
