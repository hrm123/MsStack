using AlgoDemos.String;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.ints
{
    /// <summary>
    /// run time 2925 ms beats 5.04%. Memory 80.82 MB beats 5.38%. 294 / 294 testcases passed.
    /// just a little more optimization needed some where. I  optimized for few special cases
    /// like all positive number array and negative target but this seems kludge, though it passes LC. I am feeling I might have to 
    /// optimize algorithm further the, for example sorting the inpt array of numbers.
    /// </summary>
    public class NSumBitarrayLong
    {
        Dictionary<long,HashSet<int>> _numsSet = new();
        long[] _nums;
        int _n=0;


        private (long, BigInteger) ToDictionaryKey(long target, BitArray used)
        {
            int bytesSize = (used.Length + 7) / 8;
            byte[] byteArray = new byte[bytesSize];
            used.CopyTo(byteArray, 0);
            return (target,new BigInteger(byteArray));

        }


        
        Dictionary<(long, BigInteger), ImmutableList<(long,long)>> _TwoSumCacheNew = new();

        public ImmutableList<(long,long)> TwoSumNew(long target, BitArray used)
        {
            var dicKey = ToDictionaryKey(target, used);
            if (_TwoSumCacheNew.ContainsKey(dicKey))
            {
                return _TwoSumCacheNew[dicKey];
            }


            List<(long, long)> response = new();
            for (int y = 0; y < _n; y++)
            {
                if (used[y] == true)
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
                        var tmp = curNum > target - curNum ? new ValueTuple<long, long>(target - curNum, curNum) : new ValueTuple<long, long>(curNum, target - curNum);
                        response.Add(tmp);
                    }
                }
                used[y] = false;
            }

            var responseList = response.Distinct().ToList().Select(v => (IList<long>)new List<long> { v.Item1, v.Item2 }.ToImmutableList()).ToList();
            _TwoSumCacheNew[dicKey] = response.Distinct().ToImmutableList<(long, long)>();
            return _TwoSumCacheNew[dicKey];

        }
        public class ListComparer : IEqualityComparer<IList<long>>
        {
            public bool Equals(IList<long> x, IList<long> y)
            {
                if (x == null && y == null) return true;
                if (x == null || y == null) return false;
                return x.SequenceEqual(y); // Compares actual elements
            }

            public int GetHashCode(IList<long> obj)
            {
                if (obj == null) return 0;

                // Generate a hash code based on the elements
                unchecked
                {
                    int hash = 17;
                    foreach (var item in obj)
                    {
                        hash = hash * 31 + item.GetHashCode();
                    }
                    return hash;
                }
            }
        }

        public IList<IList<long>> NSumRecursive(long target, BitArray used, int l)
        {
            var response = new List<IList<long>>();
            if (l == 2)
            {
                var twoSumList = TwoSumNew(target, used);
                return twoSumList.Select(l => (IList<long>) new List<long> { l.Item1, l.Item2 }).ToList<IList<long>>();
            }
            for (int y = 0; y < _n; y++)
            {
                if (used[y] == true)
                {
                    continue;
                }
                used[y] = true;
                long numb = _nums[y];
                var resp = NSumRecursive(target - numb, used, l - 1);

                foreach (var lst in resp)
                {
                    List<long> newList = new List<long>(lst);
                    newList.Add(numb);
                    newList.Sort();
                    response.Add(newList);
                }
                used[y] = false;
            }
            var uresp = response.Distinct(new ListComparer()).ToList();
            return uresp;
        }

        public IList<IList<int>> FourSum(int[] nums, int target)
        {
            List<IList<int>> ret = new();
            
            _nums = nums.Order().Select(n => (long)n).ToArray();
            _n = nums.Length;
            

            if (_nums[_n - 1] >= 0 && _nums[0] >= 0 && target < 0)
            {
                return ret;
            }
            if (_nums[_n - 1] < 0 && _nums[0] < 0 && target > 0)
            {
                return ret;
            }
            BitArray used = new BitArray(_n, false);
            for (int y = 0; y < _n; y++)
            {
                if (_numsSet.ContainsKey(_nums[y]))
                {
                    _numsSet[_nums[y]].Add(y);
                }
                else
                {
                    _numsSet[_nums[y]] = new HashSet<int> { y };
                }
            }
            var resp =  NSumRecursive((long)target,used,4);
            return resp.Distinct(new ListComparer()).ToList().Select(x => (IList<int>) x.Select(y =>(int)y).ToList<int>()).ToList();
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
            NSumBitarrayLong fso = new();
            int[] nums;
            int target;
            

            nums = [1, 0, -1, 0, -2, 2];
            target = 0;
            Console.WriteLine($"Problem : numbers=[1, 0, -1, 0, -2, 2] target =0");
            Console.WriteLine($"answer={Stringify(fso.FourSum(nums, target))} should be of [[-2,-1,1,2],[-2,0,0,2],[-1,0,0,1]]");
            
            nums = [2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2];
            target = 8;
            Console.WriteLine($"answer={Stringify(fso.FourSum(nums, target))} should be of [[2,2,2,2]]");
            
            nums = [1000000000, 1000000000, 1000000000, 1000000000];
            target = -294967296;
            Console.WriteLine($"answer={Stringify(fso.FourSum(nums, target))} should be of []");
            
            nums = [1000000000, -1, -1, -1];
            target = 999999997;
            Console.WriteLine($"answer={Stringify(fso.FourSum(nums, target))} should be of [[1000000000, -1, -1, -1]]");
        }
    }
}
