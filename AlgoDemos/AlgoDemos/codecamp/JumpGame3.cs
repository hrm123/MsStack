using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    internal class JumpGame3
    {
        int _l;
        bool[] _visited;
        int[] _arr;
        public bool CanReach(int[] arr, int start)
        {
            _l = arr.Length;
            _arr = arr;
            _visited = new bool[_l];
            for (int i = 0; i < _l; i++)
            {
                _visited[i] = false;
            }
            return FindRecursiveSoln(start);
        }

        bool FindRecursiveSoln(int index)
        {
            if (index > _l - 1 || index < 0 || _visited[index]) { return false; }
            if (_arr[index] == 0)
            {
                return true;
            }
            _visited[index] = true;
            return FindRecursiveSoln(index + _arr[index]) || FindRecursiveSoln(index - _arr[index]);
        }
    }
}
