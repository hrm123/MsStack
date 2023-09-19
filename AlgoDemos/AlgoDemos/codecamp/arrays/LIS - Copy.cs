using System;
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
    /// abandoned algo since n log(n)) algo found
    /// </summary>
    public class LISCopy
    {

        int[] _arr; // value, index

        public static void TestLIS()
        {
            int[] arr = new int[] { 3, 8, 3, 5, 8, 3, 9 };
            LIS lis = new LIS();
            lis.LengthOfLIS(arr);

        }

        public int LengthOfLIS(int[] nums)
        {
            //merge sort while also keeping indexes
            MergeSorter ms = new MergeSorter();
            var sortedArr = ms.MergeSort(nums);

            //go through merge sorted array and choose the elements that have max index apart
            
            
            List<Tuple<int,int>> result = new List<Tuple<int,int>>();

            int left = 0, right = 0;
            for (int i = 1; i < sortedArr.Length; i++)
            {
                if (sortedArr[i - 1].Item2 < sortedArr[i].Item2)
                {
                    right++;
                }
                else
                {
                    if (left < right)
                    {
                        result.Add(new Tuple<int, int>(left, right));
                    }
                    left = i;
                    right = i;
                }
             }
            


            int max = -1, maxIndex = -1, ctr = 0;

            
            foreach(var list in result)
            {
                if((list.Item2 - list.Item1) > max)
                {
                    max = list.Item2 - list.Item1;
                    maxIndex = ctr;

                }
                ctr++;
            }
            


            string commaDelimited = String.Join(",", result[maxIndex]);
            Console.WriteLine($"LIS = {commaDelimited}");

            

            return max+1;
        }




        private void swap(int x, int y)
        {
            var tmp = _arr[x];
            _arr[x] = _arr[y];
            _arr[y] = tmp;
        }

    }
}
