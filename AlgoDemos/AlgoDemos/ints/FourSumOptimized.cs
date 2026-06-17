using AlgoDemos.String;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.ints
{  
    /// <summary>
    /// Even cached result version - LC says "Time Limit Exceeded for [200 2s] target =8. Other approach I could think of is synamic programming.
    /// Soln([0,n]) = [0,Soln[1,n]] union [Soln([1,n])]
    /// </summary>
    public class FourSumOptimized
    {
        Dictionary<int,HashSet<int>> _numsSet = new();
        int[] _nums;
        int ctr2 = 0, ctr3 = 0, _n=0;

        // Dictionary<(int, bool[]), IList<IList<int>>> _TwoSumCache = new Dictionary<(int, bool[]), IList<IList<int>>>();
        Dictionary<(int,BigInteger), ImmutableHashSet<(int, int)>> _TwoSumCache = new();

        private (int, BigInteger) ToDictionaryKey(int target, BitArray used)
        {
            int bytesSize = (used.Length + 7) / 8;
            byte[] byteArray = new byte[bytesSize];
            used.CopyTo(byteArray, 0);
            return (target,new BigInteger(byteArray));

        }

        private ImmutableHashSet<(int,int)> TwoSum(int target, BitArray used)
        {
            var dicKey = ToDictionaryKey(target, used);
            if (_TwoSumCache.ContainsKey(dicKey))
            {
                return _TwoSumCache[dicKey];
            }


            HashSet<(int, int)> response = new();
            for (int y = 0; y < _n; y++)
            {
                if (used[y]==true)
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
            
            var responseList = response.Select(v => (IList<int>)new List<int> { v.Item1, v.Item2 }.ToImmutableList()).ToList();
            _TwoSumCache[dicKey] = response.ToImmutableHashSet<(int,int)>();
            return _TwoSumCache[dicKey];
        }


        Dictionary<(int, BigInteger), ImmutableHashSet<(int, int,int)>> _ThreeSumCache = new();
        private ImmutableHashSet<(int, int,int)> ThreeSum(int target, BitArray used)
        {

            var dicKey = ToDictionaryKey(target, used);
            if (_ThreeSumCache.ContainsKey(dicKey))
            {
                return _ThreeSumCache[dicKey];
            }


            HashSet<(int, int, int)> response = new();
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
                    if (numb < lst.Item1) // lst is already sorted ascending
                    {
                        response.Add(new ValueTuple<int, int, int>(numb, lst.Item1, lst.Item2));
                    }else if (numb > lst.Item2) // lst is already sorted ascending
                    {
                        response.Add(new ValueTuple<int, int, int>(lst.Item1, lst.Item2, numb));
                    }
                    else
                    {
                        response.Add(new ValueTuple<int, int, int>(lst.Item1, numb, lst.Item2));
                    }
                }
                used[y] = false;
            }

            _ThreeSumCache[dicKey] = response.ToImmutableHashSet();
            return _ThreeSumCache[dicKey];
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
            
            HashSet<(int, int, int, int)> response = new();
            // bool[] used = new bool[nums.Length];
            BitArray used = new BitArray(nums.Length, false);

            for (int y = 0; y < _n; y++)
            {
                used[y] = true;
                int numb = _nums[y];
                var threeSetList = ThreeSum(target - numb, used);
                
                foreach (var lst in threeSetList)
                {
                    var newList = new List<int> { lst.Item1, lst.Item2, lst.Item3 };
                    newList.Add(numb);
                    ((List<int>)newList).Sort();
                    response.Add(new ValueTuple<int, int, int, int>(newList[0], newList[1], newList[2], newList[3]));
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
            FourSumOptimized fso = new FourSumOptimized();
            int[] nums;
            int target;
            

            nums = [1, 0, -1, 0, -2, 2];
            target = 0;
            // Console.WriteLine($"answer={Stringify(fso.FourSum(nums, target))} should be of [[-2,-1,1,2],[-2,0,0,2],[-1,0,0,1]]");

            nums = [2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2];
            target = 8;
             Console.WriteLine($"answer={Stringify(fso.FourSum(nums, target))} should be of [[2,2,2,2]]");

            
        }
    }
}
