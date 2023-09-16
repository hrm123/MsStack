using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.bits
{
    public static class BitExtensions
    {
        public static string BitToString(this BitArray arr)
        {
            var bitEnum = arr.GetEnumerator();
            StringBuilder sb = new StringBuilder();
            while (bitEnum.MoveNext())
            {
                sb.Append(((bool)bitEnum.Current == false) ? "0" : "1");
            }
            return sb.ToString();
        }
    }
}
