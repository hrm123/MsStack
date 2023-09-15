using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    //801. Minimum Swaps To Make Sequences Increasing

    public class MinSwapsAscArray
    {
        int[] _a1;
        int[] _a2;

        public int MinSwap(int[] nums1, int[] nums2)
        {
            _a1 = nums1;
            _a2 = nums2;
            dp(0, 0);
            return _minSwaps;
        }

        void swap(int depth)
        {
            // Console.WriteLine($"swap depth={depth}");
            int tmp = _a1[depth];
            _a1[depth] = _a2[depth];
            _a2[depth] = tmp;
        }

        int _minSwaps = Int32.MaxValue;

        bool InOrder(int depth)
        {

            if (depth > _a1.Length || depth < 0)
            {
                // Console.WriteLine($"error at {depth}");
                return false;
            }

            if (depth < 2)
            {
                return true;
            }
            // from start of array to  depth  each array should have ascending order
            for (int i = 0; i <= depth - 2; i++)
            {
                if (_a1[i] >= _a1[i + 1] || _a2[i] >= _a2[i + 1])
                {
                    return false;
                }
            }
            return true;
        }

        Dictionary<string, bool> _cache = new Dictionary<string, bool>();

        void dp(int depth, int total)
        {
            if (depth > _a1.Length || _minSwaps == 0)
            {
                return; // end of recursion 
            }

            // Console.WriteLine($"depth={depth}");

            bool inOrder = InOrder(depth);
            if (!inOrder)
            {
                return; // end of recursion - failure
            }

            if (inOrder && (depth == _a1.Length))
            {
                if (_minSwaps > total)
                {
                    _minSwaps = total;
                }
                // Console.WriteLine($"Reached order at {depth} with value {_minSwaps}");
                return;  // end of recursion - success

            }

            dp(depth + 1, total); // without swap
            swap(depth);
            dp(depth + 1, total + 1);

        }

    }
}
