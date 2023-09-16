using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.bits
{
    public class BitArrayDemo
    {
        BitArray _arr = new BitArray(200, false);

        public BitArray Bits { get { return _arr; } }

        void ToggleEvenBits()
        {
            //Console.WriteLine(_arr.BitToString());
            for (int y = 0; y < 200; y++)
            {
                if (y % 2 == 0)
                {
                    _arr[y] = true;
                }
            }
            //Console.WriteLine(_arr.BitToString());

        }

        void ToggleOddBits()
        {
            //Console.WriteLine(_arr.BitToString());
            for (int y = 0; y < 200; y++)
            {
                if (y % 2 != 0)
                {
                    _arr[y] = true;
                }
            }
            //Console.WriteLine(_arr.BitToString());

        }

        void ToggleNthBit(int n)
        {
            _arr[n] = !_arr[n];

        }

        public static void TestCase_Union()
        {
            BitArrayDemo bd1 = new BitArrayDemo();
            bd1.ToggleEvenBits();
            BitArrayDemo bd2 = new BitArrayDemo();
            bd2.ToggleOddBits();

            bd1.Bits.Or(bd2.Bits);  // result bit is 1, if and only if, either of the matched bits is one

            Console.WriteLine(bd1.Bits.BitToString()); // will output all ones


        }

        public static void TestCase_Intersect()
        {
            BitArrayDemo bd1 = new BitArrayDemo();
            bd1.ToggleEvenBits();
            BitArrayDemo bd2 = new BitArrayDemo();
            bd2.ToggleOddBits();

            bd1.Bits.And(bd2.Bits); // result bit is 1, if and only if, both of the matched bits are one

            Console.WriteLine(bd1.Bits.BitToString()); // will output all zeros


        }



        public static void TestCase_Xor()
        {
            BitArrayDemo bd1 = new BitArrayDemo();
            bd1.ToggleEvenBits();
            BitArrayDemo bd2 = new BitArrayDemo();
            bd2.ToggleEvenBits();

            bd1.Bits.Xor(bd2.Bits); // result bit is 1, if and only if, only one of the matched bits is one

            Console.WriteLine(bd1.Bits.BitToString()); // will output all zeros


        }




        public static void TestCase1()
        {
            BitArrayDemo bd = new BitArrayDemo();
            bd.ToggleEvenBits();
        }

    }
}
