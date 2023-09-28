using AlgoDemos.ExpressionTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    /// <summary>
    /// Another algorithm - runtime beats 6% of c# users. 80MB - beats 82% of c# users
    /// </summary>
    public class NonOverlappingIntvls
    {
        public static void TestCase()
        {
            int[][] intvls = new int[][] { new int[] { 81, 97 }, new int[] { -71, 60 }, new int[] { 36, 97 }, new int[] { 76, 96 }, new int[] { 59, 68 }, new int[] { 54, 88 }, new int[] { -65, 40 }, new int[] { 83, 84 }, new int[] { 27, 50 }, new int[] { -59, -50 }, new int[] { 73, 78 }, new int[] { 50, 57 }, new int[] { -49, 81 }, new int[] { -16, 90 }, new int[] { -83, -23 }, new int[] { -58, 98 }, new int[] { 78, 99 }, new int[] { -57, 81 }, new int[] { -2, 85 }, new int[] { -88, 45 }, new int[] { 85, 90 }, new int[] { -64, 17 }, new int[] { 76, 78 }, new int[] { -17, 5 }, new int[] { -98, 15 }, new int[] { 86, 100 } };
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
            var pointsNew = new LinkedList<Tuple<int, int>>();
            int removed = 0;
            Dictionary<int, int> counts = new Dictionary<int, int>();
            while (i < points.Count())
            {
                pointsNew.AddLast(new LinkedListNode<Tuple<int, int>>( points[i]));
                i++;
                while (i < points.Count() && points[i].Item1 == points[i - 1].Item1)
                {
                    removed++;
                    i++;
                }
            }

            //create hash counter based on numebr of starts and number of ends at particular point
            
            var iterator = pointsNew.GetEnumerator();
            Tuple<int, int> current;
            while(iterator.MoveNext())
            {
                current = iterator.Current;
                if (!counts.ContainsKey(current.Item1)) { counts[current.Item1] = 0; }
                if (!counts.ContainsKey(current.Item2)) { counts[current.Item2] = 0; }
                counts[current.Item1] += 1;
                counts[current.Item2] -= 1;
            }

            //remove overlaps based on depth at any given point
            int depth = 0;
            List<Tuple<int, int>> removeList = new List<Tuple<int, int>>();


            LinkedListNode<Tuple<int, int>> currentNode = pointsNew.First;
            while(currentNode!= null)
            {
                var previous = currentNode.Previous;
                if (previous != null && currentNode.Value.Item1 > previous.Value.Item2)
                {
                    depth += counts[previous.Value.Item2];
                }
                depth += counts[currentNode.Value.Item1];
                if (depth <= 1)
                {
                    currentNode = currentNode.Next;
                    continue;
                }

                //add point that has max Item2 to remove list
                if (currentNode.Value.Item2 > previous.Value.Item2)
                {
                    var cur = currentNode.Next;
                    counts[currentNode.Value.Item2] += 1; // since earlier it ends there -1 was added. Now it does not end there anymore
                    counts[currentNode.Value.Item1] -= 1;
                    pointsNew.Remove(currentNode);
                    removeList.Add(currentNode.Value);
                    currentNode = cur;
                    
                    depth -= 1;
                }
                else
                {
                    counts[previous.Value.Item2] += 1; // since earlier it ends there -1 was added. Now it does not end there anymore
                    counts[previous.Value.Item1] -= 1;
                    removeList.Add(previous.Value);
                    pointsNew.Remove(previous);
                    currentNode = currentNode.Next;
                    depth -= 1;
                }
            }
            return removed + removeList.Count();
        }
    }
}
