using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.bits
{
    /// <summary>
    /// C# bit operators are defined for int types (int, uint, long, ulong). They acan also be applied on few other 
    /// types (sbyte, byte, short, ushort, char) which
    /// are first converted to Integer and then bit wise operations applied 
    /// Operators -  [ OR = | ] [ AND = & ] [ XOR = ^ ] [ COMPLEMENT = ~ ] [ LEFTSHIFT = << ] [ RIGHTSHIFT = >> ] [UNSIGNEDRIGHTSHIFT = >>> ]
    /// Bitwise and shift operators never cause overflow
    /// Operator precedence (highest to lowest) - '~' , '<<'/'>>'/'>>>' , & , ^ , |
    /// </summary>
    public class BitDemo
    {

        /// <summary>
        /// Toggles all bits of operand - making 1s to be 0's and viceversa
        /// </summary>
        public static void Complement_Demo()
        {
            uint a = 0b_0000_1111_0000_1111_0000_1111_0000_1100; // uint is 32 bit
            uint b = ~a;
            Console.WriteLine(Convert.ToString(b, toBase: 2));
            // Output: 
            // 11110000111100001111000011110011
        }

        /// <summary>
        /// Shifts bits of left-hand operand by number of bits defined by right-hand operand. This operation
        /// discards the higher order bits that are outside the range of result type and sets lower-order 
        /// empty bit positions  to 0
        /// </summary>
        public static void LeftShift_Demo()
        {
            int a = 1;
            Console.WriteLine($"Before: {Convert.ToString(a, toBase: 2)}");
            int b = a << 30;
            Console.WriteLine($"After left shift by 30 places: {Convert.ToString(b, toBase: 2)}");


            uint x = 0b_1100_1001_0000_0000_0000_0000_0001_0001;
            Console.WriteLine($"Before: {Convert.ToString(x, toBase: 2)}");

            uint y = x << 4;
            Console.WriteLine($"After:  {Convert.ToString(y, toBase: 2)}");
            // Output:
            // Before: 11001001000000000000000000010001
            // After:  10010000000000000000000100010000
        }

        /// <summary>
        /// '>>' operator shifts its left hand operand right by the number of bits defined by its right hand
        /// operand.  It discards lower order bits that go out of  last position after shift. How the higher order bits 
        /// are handled depends on datatype. 
        /// For int/long - >> performs arithmetic shift - if number is -ve then  higher order empty bits become 1. If number 
        /// is positive then higher order empty bits become 0.
        /// For uint/ulong >> performs logical shift - higher order empty positions are always set to 0
        /// </summary>
        public static void RightShift_Demo()
        {
            uint x = 0b_1001;
            Console.WriteLine($"Before: {Convert.ToString(x, toBase: 2),4}");

            uint y = x >> 2;
            Console.WriteLine($"After:  {Convert.ToString(y, toBase: 2).PadLeft(4, '0'),4}");
            // Output:
            // Before: 1001
            // After:  0010
        }


        /// <summary>
        /// '>>>' always performs logical shift. That means higher order empty bits are set to 0. Lower order
        /// bits that go out of last position are discarded. >>> performs arithmetic shift if lef thand operand is signed type
        /// Only available in C# 11.0
        /// </summary>
        public static void UnsignedRightShift_Demo()
        {
            int x = -8;
            Console.WriteLine($"Before:    {x,11}, hex: {x,8:x}, binary: {Convert.ToString(x, toBase: 2),32}");

            int y = x >> 2;
            Console.WriteLine($"After  >>: {y,11}, hex: {y,8:x}, binary: {Convert.ToString(y, toBase: 2),32}");

            int z = 0; // x >>> 2;
            Console.WriteLine($"After >>>: {z,11}, hex: {z,8:x}, binary: {Convert.ToString(z, toBase: 2).PadLeft(32, '0'),32}");
            // Output:
            // Before:             -8, hex: fffffff8, binary: 11111111111111111111111111111000
            // After  >>:          -2, hex: fffffffe, binary: 11111111111111111111111111111110
            // After >>>:  1073741822, hex: 3ffffffe, binary: 00111111111111111111111111111110
        }

        public void And_Demo()
        {
            uint a = 0b_1111_1000;
            uint b = 0b_1001_1101;
            uint c = a & b;
            Console.WriteLine(Convert.ToString(c, toBase: 2));
            // Output:
            // 10011000
        }

        public void Xor_Demo()
        {
            uint a = 0b_1111_1000;
            uint b = 0b_0001_1100;
            uint c = a ^ b;
            Console.WriteLine(Convert.ToString(c, toBase: 2));
            // Output:
            // 11100100
        }
        public void Or_Demo()
        {
            uint a = 0b_1010_0000;
            uint b = 0b_1001_0001;
            uint c = a | b;
            Console.WriteLine(Convert.ToString(c, toBase: 2));
            // Output:
            // 10110001
        }

    }
}
