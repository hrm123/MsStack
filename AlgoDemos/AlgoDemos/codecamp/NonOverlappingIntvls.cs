using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    public class NonOverlappingIntvls
    {
        public static void TestCase()
        {
            int[][] intvls = new int[][] { new int[] { 0,1 }, new int[] { 3,4 }, new int[] { 1,2 }};
            NonOverlappingIntvls noi = new NonOverlappingIntvls();
            int answer = noi.EraseOverlapIntervals(intvls);
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

            //sort points
            points = points.OrderBy(p => p.Item1).ThenBy(p => p.Item2).ToList();

            // remove multiple routes starting from same point (retain only the first of such)
            i = 0;
            List<Tuple<int, int>> pointsNew = new List<Tuple<int, int>>();
            int removed = 0;
            Dictionary<int, int> counts = new Dictionary<int, int>();
            while (i < points.Count())
            {
                pointsNew.Add(points[i]);
                i++;
                while (i < points.Count() && points[i].Item1 == points[i - 1].Item1)
                {
                    removed++;
                    i++;
                }
            }

            //create hash counter based on numebr of starts and number of ends at particular point
            for (i = 0; i < pointsNew.Count(); i++)
            {
                if (!counts.ContainsKey(pointsNew[i].Item1)) { counts[pointsNew[i].Item1] = 0; }
                if (!counts.ContainsKey(pointsNew[i].Item2)) { counts[pointsNew[i].Item2] = 0; }
                counts[pointsNew[i].Item1] += 1;
                counts[pointsNew[i].Item2] -= 1;
            }

            //remove overlaps based on depth at any given point
            int depth = 0;
            List<Tuple<int, int>> removeList = new List<Tuple<int, int>>();
            for (i = 0; i < pointsNew.Count(); i++)
            {

                if(i>0 && pointsNew[i].Item1 != pointsNew[i - 1].Item2)
                {
                    depth += counts[pointsNew[i-1].Item2];
                }

                depth += counts[pointsNew[i].Item1];
                if (depth > 1)
                {
                    //add point that has max Item2 to remove list
                    if (pointsNew[i].Item2 > pointsNew[i-1].Item2 )
                    {
                        //remove pointsNew[i]
                        removeList.Add(pointsNew[i]);
                        counts[pointsNew[i].Item2] += 1; // since earlier it ends there -1 was added. Now it does not end there anymore
                        counts[pointsNew[i].Item1] -= 1;

                    }
                    else
                    {
                        //remove pointsNew[i-1]
                        removeList.Add(pointsNew[i - 1]);
                        counts[pointsNew[i - 1].Item2] += 1; // since earlier it ends there -1 was added. Now it does not end there anymore
                        counts[pointsNew[i - 1].Item1] -= 1;

                    }
                    depth -= 1;
                }
            }
            return removed + removeList.Count();
        }
    }
}
