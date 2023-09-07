using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    class SegmentTree
    {
        int[] st;

        SegmentTree(int[]  arr, int n)
        {
            int x = (int) (Math.Ceiling(Math.Log(n) / Math.Log(2)));
            int max_size = 2 * (int)Math.Pow(2, x) - 1;
            st = new int[max_size];
            ConstructST(arr, 0, n - 1, 0);
        }


        static public void testCase()
        {
            int[] arr = { 1, 3, 5, 7, 9, 11 };
            int n = arr.Length;
            SegmentTree st = new SegmentTree(arr, n);
            Console.WriteLine($"Sum of values in range [1,3] = {st.getSum(n,1,3)}");
            st.updateValue(arr, n, 1, 10);
            Console.WriteLine($"Sum of values in range [1,3] after update = {st.getSum(n, 1, 3)}");
        }

        /// <summary>
        /// Recursive function to constuct segment tree
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="ss">segment start</param>
        /// <param name="se">segment end</param>
        /// <param name="si">current segment index</param>
        /// <returns></returns>
        int ConstructST(int[] arr, int ss, int se, int si)
        {
            if(ss == se)
            {
                st[si] = arr[ss]; // value of node
                return arr[ss];
            }

            int mid = getMid(ss, se);
            st[si] = ConstructST(arr, ss, mid, si * 2 + 1) +
                ConstructST(arr, mid+1 , se, si * 2 + 2) ;
            return st[si];

        }

        int getMid(int s, int e)
        {
            return s + (e-s) / 2;
        }


        int getSum(int n, int qs, int qe)
        {
            if(qs<0 || qe > n-1 || qs > qe)
            {
                return -1;
            }
            return getSumUtil(0, n-1, qs, qe, 0);
        }

        /// <summary>
        /// Recursive function to calculate sum of elements in given range (qs, qe)
        /// </summary>
        /// <param name="ss">current segment start</param>
        /// <param name="se">current segment end</param>
        /// <param name="qs">query range start</param>
        /// <param name="qe">query range start</param>
        /// <param name="si">current segment index</param>
        int getSumUtil(int ss, int se, int qs, int qe, int si)
        {
            if(qs <= ss && qe >= se) // current node range lies within query range
            {
                return st[si]; // return value of current ode.. which is first node haginv its range within query range
            } else if(se <qs || ss > qe) // current node if out of query range
            {
                return 0;
            } else // part of segment range overlaps with part of query range.. move left of right based on mid of segment
            {
                int mid = getMid(ss, se);
                return getSumUtil(ss, mid, qs, qe, 2 * si + 1) +
                    getSumUtil(mid+1, se, qs, qe, 2 * si + 2);

            }

        }

        /// <summary>
        /// Function to update a value in input array and segment tree
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="n"></param>
        /// <param name="i"></param>
        /// <param name="new_val"></param>
        void updateValue(int[] arr, int n, int i, int new_val)
        {
            if (i < 0 || i > n - 1){
                return;
            }
            int diff = new_val - arr[i];

            //update input array
            arr[i] = new_val;

            //update segment tree
            updateValueUtil(0, n - 1, i, diff, 0);

        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ss">start of segment</param>
        /// <param name="se">end of segment</param>
        /// <param name="i">Index of element to be updated</param>
        /// <param name="diff">Value to be added to all nodes which have i in their range</param>
        /// <param name="si"></param>
        void updateValueUtil(int ss, int se, int i, int diff, int si)
        {
            //if input index is outside of range of current node then return
            if (i < ss || i > se) return;

            // input node is in range of current node.. hence update value by diff
            st[si] = st[si] + diff;
            if(se != ss) // not reached leaf node yet
            {
                int mid = getMid(ss, se);
                updateValueUtil(ss, mid, i, diff, 2 * si + 1);
                updateValueUtil(mid + 1, se, i, diff, 2 * si + 2);
            }
        }



    }
}
