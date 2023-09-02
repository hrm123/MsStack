using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    /*
     * Took 20 minutes to get algorithm. 16 minutes to inital running code. 40 more minutes to fix border errors + get satisfactory solution that passes all test cases of leetcode.
     * Solution took 90 ms (beats 24.4% of C# users) and 40 MB (beats 14% of C# users
     */

    public class FindPeakElementPuzzle
    {
        public int FindPeakElement(int[] nums)
        {
            return findSol(nums, 0, nums.Length - 1);
        }

        int findSol(int[] nums, int left, int right)
        {
            Console.WriteLine($"l={left} r={right}");
            //terminating contition TODO
            if (left < 0 || right < 0 || left > right || right > nums.Length - 1 || left > nums.Length - 1 || (right == left && nums.Length != 1)) // only 2 or lesser elements
            {
                return -1;
            }
            if (left == right) { return 0; }
            if (right == left + 1)
            { // only 2 elements
                if (left == 0 && nums[left] > nums[right]) { Console.WriteLine($"solution found {left}"); return left; }
                if (right == nums.Length - 1 && nums[left] < nums[right]) { Console.WriteLine($"solution found {right}"); return right; }
                return -1;
            }
            int c = left + (int)Math.Floor((right - left) / 2.0);
            Console.WriteLine($"c={c}, nums[c-1]={nums[c - 1]}, nums[c]={nums[c]}, nums[c+1]={nums[c + 1]}");
            if (nums[c] > nums[c - 1] && nums[c] > nums[c + 1])
            {
                //solution found 
                Console.WriteLine($"solution found {c}");
                return c;
            }

            int sol = findSol(nums, left, nums[c - 1] > nums[c] ? c : c - 1);
            if (sol != -1)
            {
                return sol;
            }
            sol = findSol(nums, (nums[c + 1] > nums[c]) ? c : c + 1, right);
            if (sol != -1)
            {
                return sol;
            }

            return -1;

        }
    }
}
