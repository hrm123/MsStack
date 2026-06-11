using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.Lists
{
  
  
    

    public class ReverseNumbers
    {
        /// <summary>
        /// 14 ms beats 93.15%. 29.04 MB beats 59.26%.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public int Reverse(int x)
        {
            int response = 0;
            int val = x;
            x = x < 0 ? -x : x;
            long temp = 0;

            while (x > 0)
            {
                //get last digit of x
                int lastDigit = x>9 ? x % 10 : x;
                temp = response * 10L + lastDigit;
                if (temp > int.MaxValue || temp < int.MinValue)
                {
                    return 0;
                }
                
                //remove last digit of x
                x = (int)Math.Round(x / 10.0 - lastDigit / 10.0,MidpointRounding.AwayFromZero);

                // add last digit to response
                response *= 10;
                response += lastDigit;
            }
            return val < 0 ? -response : response;
        }

        public static void Demo()
        {
            ReverseNumbers reverseNumbers = new ReverseNumbers();

            int response = -1;
            // response reverseNumbers.Reverse(123);
            // Console.WriteLine($"{response} should be 321");
            // response = reverseNumbers.Reverse(-123);
            // Console.WriteLine($"{response} should be -321");
            // response = reverseNumbers.Reverse(120);
            // Console.WriteLine($"{response} should be 21");
            int numb = 1534236469;
            // response = reverseNumbers.Reverse(numb);
            // Console.WriteLine($"{response} should be 0");
            numb = -23935;
            response = reverseNumbers.Reverse(numb);
            Console.WriteLine($"{response} should be -53932");
        }
    }
}
