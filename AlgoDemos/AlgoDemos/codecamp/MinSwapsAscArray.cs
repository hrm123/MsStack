using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    //801. Minimum Swaps To Make Sequences Increasing

    public class MinSwapsAcsArray
    {
        int[] _a1;
        int[] _a2;
        int[] dp;

        public int MinSwap(int[] nums1, int[] nums2)
        {
            _a1 = nums1;
            _a2 = nums2;
            dp = new int[_a1.Length];
            dp[0] = 0; // no swaps when only one element arrays
            return soln();
        }


        bool swap(int depth)
        {
            // Console.WriteLine($"swap depth={depth}");
            if (depth < 0 || depth >= _a1.Length)
            {
                return false; // nothing to do
            }
            int tmp = _a1[depth];
            _a1[depth] = _a2[depth];
            _a2[depth] = tmp;
            return true;
        }

        int _minSwaps = Int32.MaxValue;
        bool endRecursion = false;
        bool InOrder(int depth)
        {

            if (depth >= _a1.Length || depth < 0)
            {
                Console.WriteLine($"error at {depth}");
                return false;
            }

            // from start of array to  depth  each array should have ascending order
            for (int i = 0; i <= depth - 1; i++)
            {
                if (_a1[i] >= _a1[i + 1])
                {
                    Console.WriteLine($"{_a1[i]},{_a1[i + 1]} mismatch");
                    return false;
                }
                if (_a2[i] >= _a2[i + 1])
                {
                    Console.WriteLine($"{_a2[i]},{_a2[i + 1]} mismatch");
                    return false;
                }
            }
            return true;
        }


        int soln()
        {
            for (int x = 1; x < _a1.Length; x++)
            {
                bool inOrder = InOrder(x);

                if (inOrder)
                {
                    Console.WriteLine($"in order at depth={x}");
                    dp[x] = dp[x - 1];
                    continue;
                }

                Console.WriteLine($"not in order at depth={x}");
                //choices - swap (i-1) / (i)  to see which on yields best result
                int numswaps = 0;
                numswaps += swap(x) ? 1 : 0; // with only (i) swapped
                Console.WriteLine($"swapped {_a1[x]},{_a2[x]}");
                if (!InOrder(x))
                {
                    //unswap (i)
                    numswaps -= swap(x) ? 1 : 0;
                    Console.WriteLine($"unswapped {_a1[x]},{_a2[x]}");

                    numswaps += swap(x - 1) ? 1 : 0; // now we have swapped  i-1
                    Console.WriteLine($"swapped {_a1[x - 1]},{_a2[x - 1]}");

                    if (!InOrder(x))
                    {
                        //swap (i)
                        numswaps -= swap(x) ? 1 : 0;
                        Console.WriteLine($"swapped both {_a1[x - 1]},{_a2[x - 1]} & {_a1[x]},{_a2[x]}");
                        if (!InOrder(x))
                        { // // now we have swapped both i / (i-1)
                          //fail
                            return -1;
                        }
                    }
                }
                Console.WriteLine($"numswaps={numswaps} at depth={x}");
                dp[x] = dp[x - 1] + numswaps;
            }
            return dp[_a1.Length - 1];
        }
    }
}
