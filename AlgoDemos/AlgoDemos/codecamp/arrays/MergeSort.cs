using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp.arrays
{
    public class MergeSorter
    {

        Tuple<int, int>[] _arrForMergeSort = null;

        public static void TestMerge()
        {
            int[] arr = new int[] { 3, 8, 3, 5, 8, 3, 9 };
            MergeSorter ms = new MergeSorter();
            ms.MergeSort(arr);

        }

        private void swap(Tuple<int, int>[] arr, int x, int y)
        {
            var tmp = arr[x];
            arr[x] = arr[y];
            arr[y] = tmp;
        }


        private Tuple<int, int>[] ArrayWithIndexes(int[] arr)
        {
            Tuple<int, int>[] arrTemp = new Tuple<int, int>[arr.Length];

            for (int i = 0; i < arr.Length; i++)
            {
                arrTemp[i] = new Tuple<int, int>(arr[i], i);
            }
            return arrTemp;
        }

        public Tuple<int, int>[] MergeSort(int[] arr)
        {
            _arrForMergeSort = ArrayWithIndexes(arr);
            MergeSortRecursive(0, _arrForMergeSort.Length - 1);
            string commaDelimited = String.Join(",", _arrForMergeSort.Select(a => $"{a.Item1},{a.Item2};"));
            Console.WriteLine($"{commaDelimited}");
            return _arrForMergeSort;

        }

        private void MergeSortRecursive(int s, int e)
        {
            if (s > e)
            {
                Console.WriteLine("start > end");
                return;
            }
            if (s == e)
            {
                //end recursion
                return;
            }
            if (e == s + 1)
            {
                if (_arrForMergeSort[s].Item1 > _arrForMergeSort[e].Item1)
                {
                    swap(_arrForMergeSort, s, e);
                }
                return;
            }
            int mid = (s + e) / 2;
            MergeSortRecursive(s, mid);
            MergeSortRecursive(mid + 1, e);
            merge(s, e);


        }

        private void merge(int s, int e)
        {
            int end = e;
            int left = s;
            int mid = (s + e) / 2;
            int right = mid + 1;
            var tmpArray = new Tuple<int, int>[e - s + 1];
            int ctr = 0;
            while (left <= mid && right <= end)
            {
                tmpArray[ctr++] = (_arrForMergeSort[left].Item1 < _arrForMergeSort[right].Item1) ? _arrForMergeSort[left++] : _arrForMergeSort[right++];
            }
            if (right <= end)
            {
                while (right <= end)
                {
                    tmpArray[ctr++] = _arrForMergeSort[right++];
                }
            }
            if (left <= mid)
            {
                while (left <= mid)
                {
                    tmpArray[ctr++] = _arrForMergeSort[left++];
                }
            }
            ctr = 0;
            for (int i = s; i <= e; i++)
            {
                _arrForMergeSort[i] = tmpArray[ctr++];
            }
        }
    }
}
