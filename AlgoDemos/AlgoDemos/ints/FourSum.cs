using AlgoDemos.String;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.ints
{  
    /// <summary>
    /// LC says "Time Limit Exceeded for [200 2s] target =8
    /// </summary>
    public class FourSumFailed
    {
        Dictionary<int,HashSet<int>> _numsSet = new();
        int[] _nums;
        int ctr2 = 0, ctr3 = 0, _n=0;
        private IList<IList<int>> TwoSum(int target, bool[] used)
        {

            // List<IList<int>> response = new();
            HashSet<ValueTuple<int, int>> response = new();
            for (int y = 0; y < _n; y++)
            {
                if (used[y])
                {
                    continue;
                }
                used[y] = true;
                int curNum = _nums[y];
                if (_numsSet.ContainsKey(target - curNum))
                {
                    var validMatchExists = _numsSet[target - curNum].Any(a => a != y && !used[a]);
                    if (validMatchExists)
                    {
                        response.Add(
                            curNum > target - curNum ? new ValueTuple<int, int>(target - curNum, curNum) : new ValueTuple<int, int>(curNum, target - curNum)
                            );
                    }
                }
                used[y] = false;
            }
            if (ctr2 == 0)
            {
                string dbgOutput = string.Join(Environment.NewLine, response.Select(row => string.Join(" ", row)));
                Console.WriteLine($"{dbgOutput}");
                Console.WriteLine("-------TwoSum--------");
                ctr2++;
            }
            return response.Select(v => (IList<int>)new List<int> { v.Item1, v.Item2 }).ToList();
        }

        private IList<IList<int>> ThreeSum(int target, bool[] used)
        {
            // List<IList<int>> response = new();
            HashSet<ValueTuple<int, int, int>> response = new();
            for (int y = 0; y < _n; y++)
            {
                if (used[y])
                {
                    continue;
                }
                used[y] = true;
                int numb = _nums[y];
                int newTarget = target - _nums[y];
                var twoSetList = TwoSum(newTarget, used);
                foreach (var lst in twoSetList)
                {   
                    lst.Add(numb);
                    ((List<int>)lst).Sort();
                    if (ctr2 < 2)
                    {
                        string dbgOutput = string.Join(Environment.NewLine, lst.Select(row => string.Join(" ", row)));
                        Console.WriteLine($"{dbgOutput}");
                        Console.WriteLine("-------ThreeSum--------");
                        ctr2++;
                    }
                    response.Add(new ValueTuple<int, int, int>(lst[0], lst[1], lst[2]));
                }
                used[y] = false;
            }

            if (ctr3 == 0)
            {
                string dbgOutput = string.Join(Environment.NewLine, response.Select(row => string.Join(" ", row)));
                Console.WriteLine($"{dbgOutput}");
                Console.WriteLine("-------ThreeSum--------");
                ctr3++;
            }
            return response.Select(v => (IList<int>)new List<int> { v.Item1, v.Item2, v.Item3 }).ToList();
        }
        public IList<IList<int>> FourSum(int[] nums, int target)
        {
            _nums = nums;
            _n = nums.Length;
            
            for (int y=0;y<_n;y++)
            {
                if (_numsSet.ContainsKey(nums[y]))
                {
                    _numsSet[nums[y]].Add(y);
                }
                else
                {
                    _numsSet[nums[y]] = new HashSet<int> { y };
                }
            }
            
            HashSet<ValueTuple<int, int, int, int>> response = new();
            bool[] used = new bool[nums.Length];
            Array.Fill(used, false);
            for (int y = 0; y < _n; y++)
            {
                used[y] = true;
                int numb = _nums[y];
                var threeSetList = ThreeSum(target - numb, used);
                foreach (var lst in threeSetList)
                {
                    lst.Add(numb);
                    ((List<int>)lst).Sort();
                    response.Add(new ValueTuple<int, int, int, int>(lst[0], lst[1], lst[2], lst[3]));
                }
                used[y] = false;
            }
            return response.Select(v => (IList<int>)new List<int> { v.Item1, v.Item2, v.Item3, v.Item4 }).ToList();
        }


        private static string Stringify(IList<IList<int>> ints)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (var listOfInts in ints)
            {
                sb.Append("[");
                foreach(var intObj in listOfInts)
                {  
                   sb.Append(intObj.ToString());
                   sb.Append(",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
            }
            sb.Append("]");
            return sb.ToString();
        }
        public static void Demo()
        {
            FourSumFailed fsf = new FourSumFailed();
            int[] nums;
            int target;
            
            PalindromeSubstring ps = new PalindromeSubstring();

            nums = [1, 0, -1, 0, -2, 2];
            target = 0;
            // Console.WriteLine($"answer={Stringify(fsf.FourSum(nums, target))} should be of [[-2,-1,1,2],[-2,0,0,2],[-1,0,0,1]]");

            nums = [2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2];
            target = 8;
            Console.WriteLine($"answer={Stringify(fsf.FourSum(nums, target))} should be of [[2,2,2,2]]");
        }
    }
}
