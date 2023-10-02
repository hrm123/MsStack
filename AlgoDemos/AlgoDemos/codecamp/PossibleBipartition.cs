using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    /// <summary>
    /// 886. Possible Bipartition
    /// </summary>
    public class PossibleBipartitionSln
    {
        public static void TestCase()
        {
            int[][] arr = new int[][] { new int[] { 1, 2 }, new int[] { 1,3 }, new int[] { 2,3 } }; // new int[] { 1, 2 }, new int[] { 3, 4 }, new int[] { 5, 6 }, new int[] { 6, 7 }, new int[] { 8, 9 }, new int[] { 7, 8 } };
            int n = 10;

            var pbs = new PossibleBipartitionSln();
            Console.WriteLine($"Answer = {pbs.PossibleBipartition(n, arr)}");
        }


        public bool PossibleBipartition(int n, int[][] dislikes)
        {
            HashSet<int>[] sets = new HashSet<int>[2] { new HashSet<int>(), new HashSet<int>() };
            int l1 = -1, l2 = -1;
            int l1Exists = -1, l2Exists = -1;  // -1 means does not exist. 0 means first set. 1 means second set
            for (int i = 0; i < dislikes.GetLength(0); i++)
            {
                l1 = dislikes[i][0];
                l2 = dislikes[i][1];
                l1Exists = -1;
                l2Exists = -1;

                if (sets[0].Contains(l1)) { l1Exists = 0; }
                if (sets[1].Contains(l1)) { l1Exists = 1; }

                if (sets[0].Contains(l2)) { l2Exists = 0; }
                if (sets[1].Contains(l2)) { l2Exists = 1; }

                // Console.WriteLine($"l1={l1},l2={l2}, l1Exists={l1Exists}, l2Exists={l2Exists}");
                Console.WriteLine($"s1 = {string.Join("", sets[0])} / s2 = {string.Join("", sets[1])}");
                // Console.WriteLine($"here0");

                if (l1Exists != -1 && l2Exists != -1)
                { //both already there in separate sets
                    if (l1Exists == l2Exists)
                    {
                        // try to see if swapping l1 to another set (and all l1 dislikes to opposite)
                        for(int y = 0;y < i; y++)
                        {
                            if (dislikes[y][0] == l1)
                            {
                                if (!sets[l1Exists].Add(dislikes[y][1]))
                                {
                                    return false;
                                }
                                sets[l1Exists == 0 ? 1 : 0].Remove(dislikes[y][1]);

                            }
                            if (dislikes[y][1] == l1)
                            {
                                if (!sets[l1Exists].Add(dislikes[y][0]))
                                {
                                    return false;
                                }
                                sets[l1Exists == 0 ? 1 : 0].Remove(dislikes[y][0]);
                            }

                        }
                        sets[l1Exists == 0 ? 1 : 0].Add(l1);
                        sets[l1Exists ].Remove(l1);

                        //now check if all dislikes are still honored
                        for (int y = 0; y <= i; y++)
                        {
                            int l1_1 = dislikes[y][0];
                            int l2_1 = dislikes[y][1];
                            if ((sets[0].Contains(l1_1) && sets[0].Contains(l2_1)) ||
                                (sets[1].Contains(l1_1) && sets[1].Contains(l2_1)))
                            {
                                return false;
                            }
                        }
                        //if we are here that means conflict got resolved
                     }
                    continue;
                }

                // Console.WriteLine($"here1");
                if (l1Exists != -1 && l2Exists == -1)
                { // l1 exists
                  // we can put l2 in other set only

                    if (!sets[l1Exists == 0 ? 1 : 0].Add(l2))
                    {
                        return false;
                    }
                    continue;
                }

                // Console.WriteLine("here2");
                if (l1Exists == -1 && l2Exists != -1)
                { // l2 alraedy exists
                  // l1 can only be put in other set
                    if (!sets[l2Exists == 0 ? 1 : 0].Add(l1))
                    {
                        return false;
                    }
                    continue;
                }
                // Console.WriteLine($"here3");
                if (l1Exists == -1 && l2Exists == -1)
                { // both dont exist.. so we can put either in either set
                    Console.WriteLine("both dont exist");
                    sets[0].Add(l1);
                    sets[1].Add(l2);
                    continue;
                }

                return false;

            }
            return true;
        }
    }
}
