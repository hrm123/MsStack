using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    public class Solution
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
            for (int i = 0; i < nums.Length; i++)
            {
                GenerateSubset(i + 1);
            }
            return output;
        }

        private void GenerateSubset(int batchSize)
        {
            int maxIters = (int)nCr(_n, batchSize);
            // Console.WriteLine($"maxIters={maxIters}");
            for (int j = 0; j < maxIters; j++)
            { // iterate all numbers of given array
                List<int> arr = new List<int>();
                for (int k = j; k < (j + batchSize); k++)
                {
                    if (k < _n)
                    { // does not overshoot array
                      // Console.WriteLine($"batchSize={batchSize},j={j},k={k}");
                        arr.Add(_nums[k]);
                    }
                    else
                    { //overshoots array
                        arr.Add(_nums[k - _n]);
                    }
                }
                output.Add(arr);
            }

        }

        private long nCr(int n, int r)
        {
            // naive: return Factorial(n) / (Factorial(r) * Factorial(n - r));
            return nPr(n, r) / Factorial(r);
        }

        private long nPr(int n, int r)
        {
            // naive: return Factorial(n) / Factorial(n - r);
            return FactorialDivision(n, n - r);
        }

        private long FactorialDivision(int topFactorial, int divisorFactorial)
        {
            long result = 1;
            for (int i = topFactorial; i > divisorFactorial; i--)
                result *= i;
            return result;
        }

        private long Factorial(int i)
        {
            if (i <= 1)
                return 1;
            return i * Factorial(i - 1);
        }
    }
}
