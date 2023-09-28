using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    public class NonOverlappingIntvlsSimple
    {
        public static void TestCase()
        {
            int[][] intvls = new int[][] { new int[] { -52, 31 }, new int[] { -73, -26 }, new int[] { 82, 97 }, new int[] { -65, -11 }, new int[] { -62, -49 },
                new int[] { 95, 99 }, new int[] { 58, 95 }, new int[] { -31, 49 }, new int[] { 66, 98 }, new int[] { -63, 2 }, new int[] { 30, 47 }, new int[] { -40, -26 } };
            NonOverlappingIntvlsSimple nois = new NonOverlappingIntvlsSimple();
            int answer = nois.EraseOverlapIntervals(intvls);
          }

        Tuple<int, int> CreateTuple(int x, int y)
        {
            return new Tuple<int, int>(x, y);
        }
        public int EraseOverlapIntervals(int[][] intervals)
        {
            //covnert to tuple format
            List<Tuple<int, int>> points = new List<Tuple<int, int>>();
            int i;
            for (i = 0; i < intervals.GetLength(0); i++)
            {
                var tup = CreateTuple(intervals[i][0], intervals[i][1]);
                points.Add(tup);
            }

            //sort points based on end time
            points = points.OrderBy(p => p.Item2).ToList(); 


            //remove overlaps based on depth at any given point
            int endtime = Int32.MinValue, result = 0;
            for (i = 0; i < points.Count(); i++)
            {

                if (points[i].Item1 >= endtime)
                {
                    endtime = points[i].Item2; 
                }
                else
                {
                    result++;
                }
            }
            return result;
        }
    }
}
