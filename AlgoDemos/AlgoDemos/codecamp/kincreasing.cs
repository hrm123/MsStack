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
            int answer = soln.k_increasing(new int[] { 12, 6, 12, 6, 14, 2, 13, 17, 3, 8, 11, 7, 4, 11, 18, 8, 8, 3 }, 1);
            


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
                    Console.WriteLine($"{j}");
                    lst.Add(arr[j]);
                }
            }
            int operationsReqd = 0;
            foreach (List<int> current in subarrays)
            {
                operationsReqd += soln(current.ToArray());
            }
            return operationsReqd;
        }

        int[,] _dp; // 0 => i-1 is changed (to min number of processed array)/ 1 => i is changed (to same as i-1) / 2 => both changed (both made '1')


        bool IsInOrder(int[] arr, int depth, int mode)
        {
            if (mode == 0) // mode used on (depth-1)
            {
                return (arr[depth] >= arr[depth - 1]);
            }
            if (mode == 1) // both numbers i,i-1 have been made equal
            {
                return true;
            }
            if (mode == 2) // depth-1 would have been 1 (since both are changed both will be made equal to 1)
            {
                return true;
            }
            return false;
        }
        int _currentMax = 1;
        const int CHANGETOMAX = 0, CHANGETOONE = 1, NOCHANGE = 2;


        int GetMin(int depth)
        {
            return Math.Min(_dp[depth, CHANGETOMAX], Math.Min(_dp[depth, CHANGETOONE], _dp[depth, NOCHANGE]));
        }

        Dictionary<int, int> _counter = new Dictionary<int, int>();
        Tuple<int, int> GetMinMax(int[] arr)
        {
            int min = 100000;
            int max = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                min = arr[i] < min ? arr[i] : min;
                max = arr[i] > max ? arr[i] : max;
                _counter[arr[i]] = 1 + _counter[arr[i]];
            }
            return new Tuple<int, int>(min, max);
        }

        int soln(int[] arr)
        {
            for (int i = 1; i <= arr.Length; i++)
            {
                _counter[i] = 0;
            }
            var minmax = GetMinMax(arr);
            _currentMax = minmax.Item1;
            _dp = new int[arr.Length, 3];


            _dp[0, CHANGETOMAX] = (arr[0] != _currentMax) ? 1 : 0; ;
            _dp[0, CHANGETOONE] = (arr[0] != 1) ? 1 : 0;
            _dp[0, NOCHANGE] = 0;

            for (int i = 1; i < arr.Length; i++)
            {

                //3 options are there for current index

                //1. change previous/current number to be same (applies to any previous state)
                _currentMax = (i>1) ? ( (arr[i] > _currentMax) ? _currentMax : arr[i-1]) : arr[i];
                _dp[i, CHANGETOMAX] = Math.Min(_dp[i - 1, CHANGETOMAX], _dp[i - 1, CHANGETOONE]) + (arr[i] == _currentMax  ? 0 : 1);
                _dp[i, CHANGETOMAX] = Math.Min(_dp[i, CHANGETOMAX],
                _dp[i - 1, NOCHANGE] + ((arr[i] == _currentMax) ? 0 : 1) + ((arr[i - 1] == _currentMax) ? 0 : 1));

                //2. change both numbers to (1,1) .. possible based on previous (1,1)
                _dp[i, CHANGETOONE] = _dp[i - 1, CHANGETOONE] + (arr[i] == 10 ? 0 : 1);

                //3. Dont change current number  (could be possible / not possible) - previous (change to one case / previous no change case / previous changes to current greatest case)

                _dp[i, NOCHANGE] = (arr[i] < arr[i - 1]) ? 100000 : _dp[i - 1, NOCHANGE]; // previous no change case
                _dp[i, NOCHANGE] = Math.Min(_dp[i, NOCHANGE], _dp[i - 1, CHANGETOONE]); // previous change to one case
                _dp[i, NOCHANGE] = Math.Min(_dp[i, NOCHANGE], (arr[i] < _currentMax) ? 100000 : _dp[i - 1, CHANGETOMAX]); // previouschange to max case

            }
            return GetMin(arr.Length - 1);
        }



    }
}
