using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    internal class CanPlaceFlowersSoln
    {
        /*
         * Runtime - 98ms (beats 82% users with C#). Memory 46 MB (beats 66% users with C#)
         */
        public bool CanPlaceFlowers(int[] flowerbed, int n)
        {
            int ctr = 0;
            int i = 0;
            int l = flowerbed.Length;
            if (n == 0) { return true; }
            if (l == 0) { return n == 0 ? true : false; }
            if (l == 1)
            {
                if (flowerbed[0] == 0)
                {
                    ctr++;
                    if (ctr >= n)
                    {
                        flowerbed[0] = 1;
                        return true;
                    }
                }
            }


            while (i <= l - 1)
            {
                if (ctr >= n)
                {
                    return true;
                }
                if (i == 0 || i == l - 2)
                {
                    if (flowerbed[i] == 0 && flowerbed[i + 1] == 0)
                    {
                        ctr++;
                        flowerbed[i == 0 ? 0 : i + 1] = 1;
                    }
                }
                else if (flowerbed[i] == 0 && flowerbed[i - 1] == 0 && flowerbed[i + 1] == 0)
                {
                    ctr++;
                    flowerbed[i] = 1;
                }
                i++;
            }
            return false;
        }
    }
}
