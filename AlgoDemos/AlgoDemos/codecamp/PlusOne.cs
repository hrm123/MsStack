using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    /*
     * 127 ms (Beats 72% users of C#). Memory 42 MB (beats 50% of users of C#)
     */
    internal class PlusOneSoln
    {
        int[] _d;
        public int[] PlusOne(int[] digits)
        {
            int l = digits.Length;
            _d = digits;
            PlusOneRecursive(l - 1);
            return _d;
        }

        private void PlusOneRecursive(int r)
        {
            if (_d[r] == 9)
            {
                if (r == 0)
                {
                    // new array need to be created
                    int[] tmp = new int[_d.Length + 1];
                    tmp[0] = 1;
                    _d.CopyTo(tmp, 1);

                    tmp[1] = 0;
                    _d = tmp;
                }
                else
                {
                    _d[r] = 0;
                    PlusOneRecursive(r - 1);
                }
            }
            else
            {
                _d[r] = _d[r] + 1;
            }
        }
    }
}
