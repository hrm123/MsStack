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
    /// LC says "Time Limit Exceeded Time Limit Exceeded 293 / 294 testcases passed. 
    /// just a little more optimization needed some where. I could optimize for few cases
    /// like all positive number array and negative target but this seems kludge. I am feeling I might have to 
    /// optimize algorithm further the, for example sorting the inpt array of numbers.
    /// </summary>
    public class FourSumOptimizedLong
    {
        Dictionary<long,HashSet<long>> _numsSet = new();
        long[] _nums;
        int _n=0;

        // Dictionary<(long, bool[]), IList<IList<long>>> _TwoSumCache = new Dictionary<(long, bool[]), IList<IList<long>>>();
        Dictionary<(long,BigInteger), ImmutableHashSet<(long, long)>> _TwoSumCache = new();

        private (long, BigInteger) ToDictionaryKey(long target, BitArray used)
        {
            int bytesSize = (used.Length + 7) / 8;
            byte[] byteArray = new byte[bytesSize];
            used.CopyTo(byteArray, 0);
            return (target,new BigInteger(byteArray));

        }

        private ImmutableHashSet<(long,long)> TwoSum(long target, BitArray used)
        {
            var dicKey = ToDictionaryKey(target, used);
            if (_TwoSumCache.ContainsKey(dicKey))
            {
                return _TwoSumCache[dicKey];
            }


            HashSet<(long, long)> response = new();
            for (int y = 0; y < _n; y++)
            {
                if (used[y]==true)
                {   
                    continue;
                }
                used[y] = true;
                long curNum = _nums[y];
                if (_numsSet.ContainsKey(target - curNum))
                {
                    var validMatchExists = _numsSet[target - curNum].Any(a => a != y && !used[(int)a]);
                    if (validMatchExists)
                    {
                        response.Add(
                            curNum > target - curNum ? new ValueTuple<long, long>(target - curNum, curNum) : new ValueTuple<long, long>(curNum, target - curNum)
                            );
                    }
                }
                used[y] = false;
            }
            
            var responseList = response.Select(v => (IList<long>)new List<long> { v.Item1, v.Item2 }.ToImmutableList()).ToList();
            _TwoSumCache[dicKey] = response.ToImmutableHashSet<(long,long)>();
            return _TwoSumCache[dicKey];
        }


        Dictionary<(long, BigInteger), ImmutableHashSet<(long, long,long)>> _ThreeSumCache = new();
        private ImmutableHashSet<(long, long,long)> ThreeSum(long target, BitArray used)
        {

            var dicKey = ToDictionaryKey(target, used);
            if (_ThreeSumCache.ContainsKey(dicKey))
            {
                return _ThreeSumCache[dicKey];
            }


            HashSet<(long, long, long)> response = new();
            for (int y = 0; y < _n; y++)
            {
                if (used[y])
                {
                    continue;
                }
                used[y] = true;
                long numb = _nums[y];
                long newTarget = target - _nums[y];
                var twoSetList = TwoSum(newTarget, used);
                foreach (var lst in twoSetList)
                {
                    if (numb < lst.Item1) // lst is already sorted ascending
                    {
                        response.Add(new ValueTuple<long, long, long>(numb, lst.Item1, lst.Item2));
                    }else if (numb > lst.Item2) // lst is already sorted ascending
                    {
                        response.Add(new ValueTuple<long, long, long>(lst.Item1, lst.Item2, numb));
                    }
                    else
                    {
                        response.Add(new ValueTuple<long, long, long>(lst.Item1, numb, lst.Item2));
                    }
                }
                used[y] = false;
            }

            _ThreeSumCache[dicKey] = response.ToImmutableHashSet();
            return _ThreeSumCache[dicKey];
        }
        public IList<IList<int>> FourSum(int[] nums, int target)
        {
            _nums = nums.Select(n => (long)n).ToArray();
            _n = nums.Length;
            
            for (int y=0;y<_n;y++)
            {
                if (_numsSet.ContainsKey(nums[y]))
                {
                    _numsSet[nums[y]].Add(y);
                }
                else
                {
                    _numsSet[nums[y]] = new HashSet<long> { y };
                }
            }
            
            HashSet<(long, long, long, long)> response = new();
            // bool[] used = new bool[nums.Length];
            BitArray used = new BitArray(nums.Length, false);

            for (int y = 0; y < _n; y++)
            {
                used[y] = true;
                long numb = _nums[y];
                var threeSetList = ThreeSum(target - numb, used);
                
                foreach (var lst in threeSetList)
                {
                    var newList = new List<long> { lst.Item1, lst.Item2, lst.Item3 };
                    newList.Add(numb);
                    ((List<long>)newList).Sort();
                    response.Add(new ValueTuple<long, long, long, long>(newList[0], newList[1], newList[2], newList[3]));
                }
                used[y] = false;
            }
            return response.Select(v => (IList<int>)new List<int> { (int)v.Item1, (int)v.Item2, (int)v.Item3, (int)v.Item4 }).ToList();
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
            FourSumOptimizedLong fso = new();
            int[] nums;
            int target;
            

            // nums = [1, 0, -1, 0, -2, 2];
            // target = 0;
            // Console.WriteLine($"answer={Stringify(fso.FourSum(nums, target))} should be of [[-2,-1,1,2],[-2,0,0,2],[-1,0,0,1]]");
            // 
            // nums = [2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2];
            // target = 8;
            // Console.WriteLine($"answer={Stringify(fso.FourSum(nums, target))} should be of [[2,2,2,2]]");
            // 
            // nums = [1000000000, 1000000000, 1000000000, 1000000000];
            // target = -294967296;
            // Console.WriteLine($"answer={Stringify(fso.FourSum(nums, target))} should be of [[]]");

            nums = [1000000000, -1, -1, -1];
            target = 999999997;
            Console.WriteLine($"answer={Stringify(fso.FourSum(nums, target))} should be of [[]]");
        }
    }
}
