using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    /// <summary>
    /// 91. decode ways - beats 46% runtime. Beats 38% memory.
    /// </summary>
    public class DecodeWays
    {
        char[] mapping = new char[26] { 'A', 'B','C','D', 'E','F','G', 'H','I','J', 'K','L','M', 'N','O',
    'P', 'Q','R','S', 'T','U','V', 'W','X','Y','Z'};
        string _s;
        int _n;
        Dictionary<string, int> _cache = new Dictionary<string, int>();

        public int NumDecodings(string s)
        {
            _s = s;
            _n = s.Length;
            return Decodings(0);
        }
        bool charsMoreThanRange(int l)
        {
            return l != _n - 1 && Int32.Parse(_s[l] + "" + _s[l + 1]) > 26;
        }

        private int Decodings(int l)
        {

            if (l == _n)
            {
                return 1; // finsihed n chars without any error
            }
            if (l == _n - 1 && _s[l] == 0)
            {
                // last remaining char is zero => invalid branch
                return -1;
            }

            string cachekey = _s.Substring(l);
            if (_cache.ContainsKey(cachekey))
            {
                return _cache[cachekey];
            }
            // include only lth char
            int total1 = (_s[l] == '0') ? -1 : Decodings(l + 1);

            // include both lth, l+1th char
            int total2 = 0;
            if (
                l == _n - 1 || _s[l] == '0' ||
                charsMoreThanRange(l)
            )
            {
                total2 = -1;
            }
            else
            {
                total2 = Decodings(l + 2);
            }

            int finalTotal = (total1 != -1 ? total1 : 0) + (total2 != -1 ? total2 : 0);
            _cache[cachekey] = finalTotal;
            return finalTotal;

        }
    }
}
