using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AlgoDemos.puzzles
{
    public class stringstuff
    {
        public void demo()
        {
            // Permutations("alaska");
            Combinations("let");
        }

        public void Combinations(string source)
        {
            bool[] used = new bool[source.Length];
            combinationsCount = 0;
            stringToPermute = source;
            for (int iter = 0; iter < stringToPermute.Length; iter++)
            {
                used[iter] = false;
            }

            WriteCombinations("", used, 0);
            Console.WriteLine("combinations count :" + permuCount);
            int targetCount = 1;
            for (int iter = 1; iter <= source.Length; iter++)
            {
                targetCount *= iter;
            }
            Console.WriteLine("Combinations count target :" + targetCount);
        }

        private void WriteCombinations(string newString, bool[] used, int level)
        {
            if (newString.Length == stringToPermute.Length)
            {
                combinationsCount++;
                Console.WriteLine(newString);

                return; // terminating condition
            }
            combinationsCount++;
            Console.WriteLine(newString);

            for (int iter = 0; iter < stringToPermute.Length; iter++)
            {
                if (used[iter]) continue;
                used[iter] = true;
                // newString += stringToPermute[iter]; -- never change inputs of recursive function locally
                WriteCombinations(newString + stringToPermute[iter], used, level + 1);
                used[iter] = false;
            }
        }
        public string stringToPermute { get; set; }
        private int permuCount;
        private int combinationsCount;

        public void Permutations(string source)
        {
            char[] chars = source.ToCharArray();
            bool[] used = new bool[source.Length];
            permuCount = 0;
            stringToPermute = source;
            for (int iter = 0; iter < stringToPermute.Length; iter++)
            {
                used[iter] = false;
            }
            
            WritePermutations("", used, 0);
            Console.WriteLine("permu count :" + permuCount);
            int targetCount = 1;
            for(int iter=1; iter <= source.Length; iter++)
            {
                targetCount *= iter;
            }
            Console.WriteLine("Permu count target :" + targetCount);
        }


        private void WritePermutations(string newString, bool[] used, int level)
        {
            // if(level ==1) { return;  }
            if(newString.Length == stringToPermute.Length)
            {
                Console.WriteLine(newString);
                permuCount++;
                return; // terminating condition
            }

            for(int iter=0;iter< stringToPermute.Length; iter++)
            {
                if(used[iter]) continue;
                used[iter] = true;
                // newString += stringToPermute[iter]; -- never change inputs of recursive function locally
                WritePermutations(newString + stringToPermute[iter], used, level + 1);
                used[iter] = false;
            }
        }
    }
}
