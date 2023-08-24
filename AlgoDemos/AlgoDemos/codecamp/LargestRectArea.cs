using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    /*
     * 
     *  95 / 98 testcases passed
     *  time limit exceeded for huge testcase
    */
    public class LargestRectArea
    {
        int[] _heights;
        public int LargestRectangleArea(int[] heights)
        {
            _heights = heights;
            RecursiveLargestArea(0, heights.Length - 1);
            return _largest;
        }

        int _largest = 0;

        public void RecursiveLargestArea(int start, int end)
        {
            // Console.WriteLine($"start={start},end={end}");

            if (start > end)
            {
                // fallacy of recursion
                return;
            }
            if (start == end)
            { // only one bar
                if (_heights[end] * 1 > _largest)
                {
                    _largest = _heights[end];
                }
                return;
            }



            int minHtIndex = getMinimumHtIndex(_heights, start, end);
            if (minHtIndex == -1)
            {
                return;
            }

            int minHtArea = GetAreaOfMinHtbarAtIndex(minHtIndex, start, end);
            // Console.WriteLine($"minHtArea={minHtArea}");
            if (minHtArea > _largest)
            {
                _largest = minHtArea;
            }

            // split array on both sides of min size indes
            RecursiveLargestArea(start, minHtIndex - 1);
            RecursiveLargestArea(minHtIndex + 1, end);

        }

        private int GetAreaOfMinHtbarAtIndex(int index, int start, int end)
        {
            return (end - start + 1) * _heights[index];
        }

        private int getMinimumHtIndex(int[] a, int start, int end)
        {
            int minHt = 100000, minHtIndex = -1;
            for (int j = start; j <= end; j++)
            {
                if (a[j] < minHt) { minHt = a[j]; minHtIndex = j; }
            }
            return minHtIndex;
        }
    }
}
