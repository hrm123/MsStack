using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoDemos.puzzles
{
    class towersOfHonai
    {
        public void demo()
        {
            char startPeg = 'A'; // start tower in output
            char endPeg = 'C'; // end tower in output
            char tempPeg = 'B'; // temporary tower in output
            int totalDisks = 3; // number of disks

            solveTowers(totalDisks, startPeg, endPeg, tempPeg);
        }

        // ?????????
        private void solveTowers(int n, char startPeg, char endPeg, char tempPeg)
        {
            if (n > 0)
            {
                solveTowers(n - 1, startPeg, tempPeg, endPeg);
                Console.WriteLine("Move disk from " + startPeg + ' ' + endPeg);
                solveTowers(n - 1, tempPeg, endPeg, startPeg);

            }
        }

    }
}
