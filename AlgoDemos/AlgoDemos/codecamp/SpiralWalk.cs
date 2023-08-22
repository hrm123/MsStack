using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    internal class SpiralWalk
    {

        public static int[][] GenerateSpiral(int n)
        {
            int xVisitedMax = n, xVisitedMin = -1;
            int yVisitedMax = n, yVisitedMin = -1;

            int[][] spiralWalkDone = new int[n][n];
            int x = 0, y = 0, step = 1, current=1;
            spiralWalkDone[x][y] = current++;

            while(current < n*n)
            {
                switch (step)
                {
                    case 1:  // right
                        while (x < xVisitedMax)
                        {
                            x++;
                            spiralWalkDone[x][y] = current++;
                        }
                        yVisitedMin++;
                        step = (step == 4) ? 1 : step + 1;
                        break;
                    case 2: // down
                        while (y < xVisitedMax)
                        {
                            y++;
                            spiralWalkDone[x][y] = current++;
                        }
                        xVisitedMax--;
                        step = (step == 4) ? 1 : step + 1;
                        break;
                    case 3: // left
                        while (x > xVisitedMin)
                        {
                            x--;
                            spiralWalkDone[x][y] = current++;
                        }
                        yVisitedMax--;
                        step = (step == 4) ? 1 : step + 1;
                        break;
                    case 4: //up
                        while (y > yVisitedMin)
                        {
                            y--;
                            spiralWalkDone[x][y] = current++;
                        }
                        xVisitedMin++;
                        step = (step == 4) ? 1 : step + 1;
                        break;
                }
            }
            return spiralWalkDone;
        }
    }
}
