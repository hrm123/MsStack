using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace AlgoDemos.puzzles
{
    public class stringstuff
    {
        public void demo()
        {
            // Permutations("alaska");
            // Combinations("let");
            // PermutationsWithRepetitinsDynamicProg("rani");
            // PermutationsWithoutRepetitionsDynamicProg("raji");
            Permutations("raji");
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

        private int fact(int len)
        {
            int factorial = 1;
            for(int y = 1; y <= len; y++)
            {
                factorial *= y;
            }
            return factorial;
        }

        private int power(int len, int pow)
        {
            int outVal = 1;
            for (int y = 1; y <= pow; y++)
            {
                outVal *= len;
            }
            return outVal;
        }


        private void PermutationsWithoutRepetitionsDynamicProg(string strSource)
        {
            int sourceLength = strSource.Length;
            char[] sourceChars = strSource.ToCharArray();
            int totalSize = power(sourceLength, sourceLength);
            string[] grid = new string[totalSize]; // create enough arraay to hold all possible results
            string[] usedLocations = new string[totalSize]; // TODO size can be lesser
            int iter = 0;
            int filledGridLen = 0;
            //initialize
            for (int y = 0; y < sourceLength; y++)
            {
                grid[y] = strSource[y] + "";
                usedLocations[y] = y + "";
            }
            int newFilledLen = strSource.Length;
            int prevStart = 0;
            while (iter < strSource.Length - 1)
            {
                filledGridLen = newFilledLen;

                for (int y = prevStart; y < filledGridLen; y++)
                {
                    for (int y1= 0;y1 < sourceChars.Length; y1++)
                    {
                        if(usedLocations[y].IndexOf(y1+"") != -1) // no repetitions
                        {
                            continue;
                        }
                        grid[newFilledLen++] = grid[y] + sourceChars[y1];
                        usedLocations[newFilledLen - 1] = usedLocations[y] + y1;
                        if (iter == sourceLength - 2)
                        {
                            Console.WriteLine(grid[newFilledLen-1]);
                        }
                    }
                }
                prevStart = filledGridLen;
                iter++;
            }


        }

        private void PermutationsWithRepetitinsDynamicProg(string strSource)
        {
            int sourceLength = strSource.Length;
            char[] sourceChars = strSource.ToCharArray();
            int totalSize = 0;
            for(int y = 1; y <= sourceLength; y++)
            {
                totalSize += power(sourceLength, y);
            }
            string[] grid = new string[totalSize]; // create enough arraay to hold all possible results
            int iter = 0;
            int filledGridLen = 0;
            //initialize
            for(int y = 0; y < sourceLength; y++)
            {
                grid[y] = strSource[y]+"";
            }
            int newFilledLen = strSource.Length;
            int prevStart = 0;
            while (iter < strSource.Length-1)
            {
                filledGridLen = newFilledLen;
                
                for (int y = prevStart; y < filledGridLen; y++)
                {
                    foreach (char ch in sourceChars)
                    {
                        grid[newFilledLen++] = grid[y] + ch;
                        if(iter == sourceLength - 2)
                        {
                            Console.WriteLine(grid[y] + ch);
                        }
                    }
                }
                prevStart = filledGridLen;
                iter++;
            }


        }
    }
}
