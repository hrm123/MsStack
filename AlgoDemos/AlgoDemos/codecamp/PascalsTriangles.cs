using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    
    /*
     * 
     * speed  - 92 ms - beats 62% of C# users. Memory - 37.35 MB - beats 7% of C# Users
     * */
    public class PascalsTriangles
    {
        IList<IList<int>> _output = new List<IList<int>>() as IList<IList<int>>;

        public IList<int> getListForRowOne()
        {
            var tmp = new List<int>();
            tmp.Add(1);
            return tmp;
        }
        public IList<int> getListForRowTwo()
        {
            var tmp = new List<int>();
            tmp.Add(1);
            tmp.Add(1);
            return tmp;
        }

        public IList<IList<int>> Generate(int numRows)
        {
            _output.Add(getListForRowOne());
            if (numRows == 1) { return _output; }
            _output.Add(getListForRowTwo());
            if (numRows == 2) { return _output; }
            for (int i = 2; i < numRows; i++)
            {
                IList<int> prevRow = _output[i - 1];
                Console.WriteLine($"prevRow.Count()={prevRow.Count()}");
                var tmp = new List<int>();
                tmp.Add(1);
                for (int j = 0; j < prevRow.Count() - 1; j++)
                {
                    Console.WriteLine($"i,j={i},{j}");
                    tmp.Add(prevRow[j] + prevRow[j + 1]);
                }
                tmp.Add(1);
                _output.Add(tmp);
            }
            return _output;
        }
    }
}
