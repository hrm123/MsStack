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

            if (depth >= _a1.Length)
            {
                Console.WriteLine($"greater than length at {depth}");
                depth = _a1.Length - 1;
            }
            if (depth > _a1.Length)
            {
                Console.WriteLine($"error depth =  {depth}");
                return false;
            }

            // from start of array to  depth  each array should have ascending order
            for (int i = 0; i <= depth - 1; i++)
            {
                if (_a1[i] >= _a1[i + 1])
                {
                    // Console.WriteLine($"{_a1[i]},{_a1[i + 1]} mismatch");
                    return false;
                }
                if (_a2[i] >= _a2[i + 1])
                {
                    // Console.WriteLine($"{_a2[i]},{_a2[i + 1]} mismatch");
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
                // if swapping i causes mismatch in depth+1 where as swapping i-1does not cause mismatch till depth then choose swap (i-1)

                int numswaps = 0;
                bool depthSwapWorks = false;
                bool depthSwapWorksOptimized = false;
                bool prevToDepthSwapWorks = false;

                //swap only i
                if (swap(x))
                {
                    if (InOrder(x))
                    {
                        depthSwapWorks = true;
                    }
                    if (InOrder(x + 1))
                    {
                        depthSwapWorksOptimized = true;
                    }
                    //unswap i
                    swap(x);
                }


                //swap only i-1
                if (swap(x - 1))
                {
                    if (InOrder(x))
                    {
                        prevToDepthSwapWorks = true;
                    }
                    //unswap i-1
                    swap(x - 1);
                }

                if (depthSwapWorksOptimized)
                {
                    swap(x); // prefer swap at current depth since it facilitates depth+1 order too
                    numswaps++;
                }
                else
                {
                    if (depthSwapWorks)
                    {
                        if (prevToDepthSwapWorks)
                        {
                            swap(x - 1); // prefer depth-1 swap since it will nto cause depth+1 order to go wrong
                            numswaps++;
                        }
                        else
                        {
                            swap(x); // better swap at current depth even though it breaks the depth+1 order too
                            numswaps++;
                        }
                    }
                    else
                    { //depthSwap does not work
                        if (prevToDepthSwapWorks)
                        {
                            swap(x - 1); // prefer depth-1 swap since it will nto cause depth+1 order to go wrong
                            numswaps++;
                        }
                        else
                        {
                            //last option - try swapping both x & x-1
                            swap(x);
                            swap(x - 1);
                            if (InOrder(x))
                            {
                                numswaps += 2;
                            }
                            else
                            {
                                //failure
                                return -1;
                            }
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
