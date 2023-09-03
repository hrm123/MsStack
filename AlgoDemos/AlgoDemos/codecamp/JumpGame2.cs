using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    internal class JumpGame2
    {
        public int Jump(int[] nums)
        {
            int _bigNumber = 100000; //more than 104 
            int _l = nums.Length;
            int[] minJumps = new int[_l];
            for (int i = 0; i < _l; i++)
            {
                minJumps[i] = -1;
            }
            minJumps[_l - 1] = 0; // no jump needed from last node -> last node
            List<int> mins = new List<int>();
            for (int i = _l - 2; i >= 0; i--)
            {
                mins.Clear();
                for (int j = 1; j <= nums[i]; j++)
                {
                    if (i + j >= _l)
                    {
                        continue;
                    }
                    mins.Add(minJumps[i + j]);
                }
                if (mins.Count() == 0)
                {
                    Console.WriteLine($"mins zero at {i}");
                    minJumps[i] = _bigNumber;
                }
                else
                {
                    minJumps[i] = 1 + mins.ToArray().Min();
                    Console.WriteLine($"minJumps[i]={minJumps[i]},i={i}");
                }
            }
            Console.WriteLine($"Answer={minJumps[0]}");
            return minJumps[0];

        }
    }
}
