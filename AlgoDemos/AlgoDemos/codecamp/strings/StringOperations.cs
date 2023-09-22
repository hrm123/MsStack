using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp.strings
{
    public class StringOperations
    {
        int _len;
        string _str;
        bool[] _isVisited;
        char[] _chars;
        List<string> _list = new List<string>();
        #region Public

        public static void TestCase_Permutations()
        {
            StringOperations so = new StringOperations();
            var list = so.Permutations("ABC");
            if(list.Length != so.factorial("ABC".Length))
            {
                Console.WriteLine("Something wrong");
            }

        }

        public string[] Permutations(string str)
        {
            _str = str;
            _len = str.Length - 1;
            _isVisited = new bool[_len+1];
            Array.Fill(_isVisited, false);
            _chars = new char[_len+1];
            _list.Clear();
            PermutationsRecursive(0);
            return _list.ToArray();
        }

        #endregion Public

        #region private

        public int factorial(int n)
        {
            int res = 1;
            while (n != 1)
            {
                res = res * n;
                n = n - 1;
            }
            return res;
        }

        private void PermutationsRecursive(int d)
        {
            if(d == _len + 1)
            {
                _list.Add(String.Join("",_chars));
                return;
            }
            for(int i = 0; i <= _len; i++)
            {
                if (_isVisited[i]) continue;

                _chars[d] = _str[i];
                _isVisited[i] = true;
                PermutationsRecursive(d + 1);
                _isVisited[i] = false;
            }
        }

        #endregion private
    }
}
