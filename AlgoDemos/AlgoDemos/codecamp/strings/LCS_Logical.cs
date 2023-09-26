using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp.strings
{
    public class LCS_Logical
    {
        int[] _soln;
        int _solnPtr = 0;
        int _m, _n;


        public static void TestCase()
        {
            string text1 = "jgtargjctqvijshexyjcjcre"; // "papmretkborsrurgtina";
            string text2 = "pyzazexujqtsjebcnadahobwf"; // "nsnupotstmnkfcfavaxgl";
            LCS_Logical lcsDp = new LCS_Logical();
            var answer = lcsDp.LongestCommonSubsequence(text1, text2);
            Console.WriteLine($"LCS = {answer} should be 6");
        }

        public int LongestCommonSubsequence(string text1, string text2)
        {
            _m = text1.Length;
            _n = text2.Length;
            _soln = new int[_m];
            Dictionary<char, List<int>> _locations = new Dictionary<char, List<int>>();
            char[] alpha = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            foreach (char c in alpha)
            {
                _locations[c] = new List<int>();
            }

            for (int i = 0; i < _n; i++)
            {
                _locations[text2[i]].Add(i);
            }



            for (int i = 0; i < _m; i++)
            {
                char current = text1[i];
                if (_locations[current].Count() == 0) // current character is not there in text2
                {
                    continue;
                }
                    
                
                if (_solnPtr == 0)
                {
                    _soln[_solnPtr++] = _locations[current][0];
                    _locations[current].RemoveAt(0);
                    continue;
                }

                int il = 0;
                bool nothingGreater = true;
                bool nothingLesser = true;
                while (_locations[current][il] <= _soln[_solnPtr - 1]) // try to fetch occurence that is after current end of solution array
                {
                    nothingLesser = false;
                    il++;
                    if(il == _locations[current].Count())
                    {
                        break;
                    }
                }

                if(il < _locations[current].Count())
                {
                    nothingGreater = false;
                }

                // if character is there at greater location then add that location at front
                if (!nothingGreater)
                {
                    _soln[_solnPtr++] = _locations[current][nothingLesser ? 0 : il];
                    _locations[current].RemoveAt(nothingLesser ? 0 : il);
                }

                if (!nothingLesser)
                {
                    // AddIndexRecursive(_locations[current][0], 0, _solnPtr - 1);
                    // _locations[current].RemoveAt(0);

                    //  add first of lesser locations that is between _solnPtr-1 & _solnPtr-2 locations if it exists
                    // if not exits, then dont add since by adding one we dont want to distrub more than one location

                    for (int y = 0; y < _locations[current].Count(); y++)
                    {
                        if ( (_solnPtr < 2 && (_locations[current][y] < _soln[_solnPtr - 1])) ||
                            (_solnPtr > 2 && (_locations[current][y] > _soln[_solnPtr - 2] && _locations[current][y] < _soln[_solnPtr - 1])))
                        {
                            _soln[_solnPtr - 1] = _locations[current][y];
                            _locations[current].RemoveAt(y);
                            break;
                        }
                        if(_solnPtr == 2  )
                        {
                            if (_locations[current][y] < _soln[_solnPtr - 2])
                            {
                                _soln[_solnPtr - 2] = _locations[current][y];
                                _locations[current].RemoveAt(y);
                            }
                            else if (_locations[current][y] < _soln[_solnPtr - 1])
                            {
                                _soln[_solnPtr - 1] = _locations[current][y];
                                _locations[current].RemoveAt(y);
                            }
                            break;
                        }
                    }
                }
               
                
            }
            return _solnPtr;

        }


        void AddIndexRecursive(int n, int l, int r)
        {
            if (l == r)
            { // end recursion
                _soln[l] = n;
                return;
            }
            int mid = (l + r) / 2;
            if (_soln[mid] < n)
            {
                AddIndexRecursive(n, mid + 1, r);
            }
            else
            {
                AddIndexRecursive(n, l, mid);
            }
        }
    }
}
