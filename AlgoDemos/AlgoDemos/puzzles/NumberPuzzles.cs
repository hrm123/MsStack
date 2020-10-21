using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoDemos.puzzles
{
    public class NumberPuzzles
    {

        public void demo()
        {
            int[]  binaryform = NumberToBinary(156);
            Console.Write("binary form of 156 : ");

            for (int iter = binaryform.Length - 1; iter >= 0; iter--)
            {
                Console.Write(binaryform[iter]);
            }
            Console.WriteLine();


            int[] decimalform = NumberToDecimal(156);
            Console.Write("decimal form of 156 : ");

            for (int iter = decimalform.Length -1 ; iter >= 0 ; iter--)
            {
                Console.Write(decimalform[iter]);
            }
            Console.WriteLine();
        }

        private  int GetLCM(int num1, int num2)
        {
            return (num1 * num2) / GetGCD(num1, num2);
        }

        private int GetGCD(int num1, int num2)
        {
            while (num1 != num2)
            {
                if (num1 > num2)
                    num1 = num1 - num2;

                if (num2 > num1)
                    num2 = num2 - num1;
            }
            return num1;
        }


        private int[] NumberToDecimal(int number)
        {
            int[] output = null;
            while (number > 0)
            {
                int divisor = 1;
                int power = 1;
                int quotient = 0;

                while (number * 1.0 / divisor > 1)
                {
                    quotient = (int)Math.Floor(number * 1.0 / divisor);
                    divisor *= 10;
                    power += 1;
                }

                if (output == null)
                {
                    output = new int[power-1];
                }

                
                number = number - quotient * (divisor/10);
                output[power-2] = quotient;
                // number = number - divisor;

            }
            return output;
        }

        private int[] NumberToBinary(int number)
        {
            int[] output = null;
            while(number > 0)
            {
                int divisor = 1;
                int power = 0;
                while (number / divisor > 1)
                {
                    divisor *= 2;
                    power += 1;
                }
                    if(output == null)
                    {
                        output = new int[power+1];
                    }
                    output[power] = 1;
                    number = number - divisor;
                
            }
            return output;
        }
    }
}
