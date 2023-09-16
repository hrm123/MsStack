using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    //801. Minimum Swaps To Make Sequences Increasing
    // 413 ms - beats 12% of c# users. 66 MB - beats 33% of C# users
    public class MinSwapsAcsArray
    {
        int[] _a1;
        int[] _a2;
        int[,] dp;

        public static void testCase()
        {
            MinSwapsAcsArray soln = new MinSwapsAcsArray();
            int answer  = soln.MinSwap(new int[] { 0, 7, 8, 10, 10, 11, 12, 13, 19, 18 }, new int[] { 4, 4, 5, 7, 11, 14, 15, 16, 17, 20 });
            Console.WriteLine($"answer={answer}");
        }
        public int MinSwap(int[] nums1, int[] nums2)
        {
            _a1 = nums1;
            _a2 = nums2;
            dp = new int[_a1.Length, 2]; // for each node store minimum value with swap at node and without swap at that node
            dp[0, 0] = 0;
            dp[0, 1] = 1;
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

        // swap -  0=dont swap depth/depth-1 elements; 1=swap depth-1 element only; 2 =swapr depth element only; 3=swap both elements)
        bool InOrderNew(int depth)
        {
            return _a1[depth] > _a1[depth - 1] && _a2[depth] > _a2[depth - 1];
        }


        int soln()
        {
            // Console.WriteLine($" dp[0, 0]={dp[0, 0]}");
            for (int x = 1; x < _a1.Length; x++)
            {

                bool inOrderWithoutSwap = InOrderNew(x);
                swap(x);
                bool inOrderWithSwap = InOrderNew(x);
                swap(x); // revert swap
                if (inOrderWithoutSwap)
                {
                    // Console.WriteLine($"x={x}, dp[x - 1, 0]={dp[x - 1, 0]}");
                    dp[x, 0] = dp[x - 1, 0];
                }
                else
                {
                    dp[x, 0] = 1000000;
                }

                if (inOrderWithSwap)
                {
                    // Console.WriteLine($"x={x}, 1+ dp[x - 1, 0]={1+ dp[x - 1, 0]}");
                    dp[x, 1] = 1 + dp[x - 1, 0];
                }
                else
                {
                    dp[x, 1] = 1000000;
                }

                swap(x - 1); // swap previous item

                bool inOrderWithoutSwap1 = InOrderNew(x);
                swap(x);
                bool inOrderWithSwap1 = InOrderNew(x);
                swap(x); // revert swap
                swap(x - 1); // revert swap

                if (inOrderWithoutSwap1)
                {
                    // depth not swapped, depth-1 swapped
                    dp[x, 0] = Math.Min(dp[x - 1, 1], dp[x, 0]); // take minimum of 2 calculated values of dp[x,0]
                }
                if (inOrderWithSwap1)
                {
                    //depth swapped, depth-1 swapped
                    // Console.WriteLine($"1+dp[x-1,1]={1 + dp[x - 1, 1]}, dp[x,1]={dp[x, 1]}");
                    dp[x, 1] = Math.Min(1 + dp[x - 1, 1], dp[x, 1]);
                }

                // Console.WriteLine($"{x}, {inOrderWithoutSwap}, {inOrderWithSwap}, {inOrderWithoutSwap1}, {inOrderWithSwap1}");
                // Console.WriteLine($"{dp[x,0]},{dp[x,1]}");

                if (!inOrderWithoutSwap && !inOrderWithSwap && !inOrderWithoutSwap1 && !inOrderWithSwap1)
                {
                    //no solution
                    return -1;
                }

            }
            return Math.Min(dp[_a1.Length - 1, 0], dp[_a1.Length - 1, 1]);
        }
    }
}
