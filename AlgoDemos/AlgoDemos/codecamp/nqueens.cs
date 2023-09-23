using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    /// <summary>
    /// Given nxn empty chessboard and n queens place all of them so that no 2 queens are attacking each other.
    /// </summary>

    public class nqueens
    {
         int[,] _board;
        int _n;
        bool[] _occupiedCols;
        List<int[,]> _allBoards;

        public static void TestCase()
        {
            nqueens nq = new nqueens();
            nq.SolveNQueens(4);
        }

        public IList<IList<string>> SolveNQueens(int n) {
             _board = new int[n,n];
            _n = n;
            _occupiedCols = new bool[n];
            Array.Fill(_occupiedCols, false);
            for(int i=0;i<n;i++)
            for(int j=0;j<n;j++){
                _board[i,j] = -1;
            }

            PlaceQueenRecursive(0);

            IList<IList<string>>  answer = new List<IList<string>>();
            for(int s=0;s<_allBoards.Count();s++){
                StringBuilder sb = new StringBuilder();
                IList<string> soln1 = new List<string>();
                for(int i=0;i<_n;i++){
                    sb.Clear();
                    for(int j=0;j<_n;j++){
                        sb.Append(_board[i,j] > 0 ? "Q" : ",");
                    }
                    soln1.Add(sb.ToString());
                }
                answer.Add(soln1);
            }
            return answer;
        }

        bool IsSafe(int row, int col)
        {
            return !_occupiedCols[col] && _board[row,col]<0 ;
            /*
            int x1 = row, y1 = col;
            while (x1 < _n && y1 >=0)
            {   
                if(_board[x1, y1] >= 0) return false;
                x1++; y1--;
            }
            x1 = row; y1 = col;
            while (x1 >=0 && y1 < _n)
            {
                if (_board[x1, y1] >= 0) return false;
                x1--; y1++;
            }
            x1 = row; y1 = col;
            while (x1 >= 0 && y1 >=0)
            {
                if (_board[x1, y1] >= 0) return false;
                x1--; y1--;
            }
            x1 = row; y1 = col;
            while (x1 < _n && y1 < _n)
            {
                if (_board[x1, y1] >= 0) return false;
                x1++; y1++;
                
            }
                return true;
            */
        }
 
        void QueenPlaced(int i, int j){
            for(int x = 0; x < _n; x++) // x axis
            {
                _board[x,j] = 0;
            }
            for (int x = 0; x < _n; x++) // y axis
            {
                _board[i, x] = 0;
            }

            int x1 = i, y1 = j;
            while (x1 < _n && y1 >=0)
            {
                _board[x1, y1] = 0;
                x1++; y1--;
                
            }
            x1 = i; y1 = j;
            while (x1 >=0 && y1 < _n)
            {
                _board[x1, y1] = 0;
                x1--; y1++;
                
            }
            x1 = i; y1 = j;
            while (x1 >= 0 && y1 >=0)
            {
                _board[x1, y1] = 0;
                x1--; y1--;
                
            }
            x1 = i; y1 = j;
            while (x1 < _n && y1 < _n)
            {
                _board[x1, y1] = 0;
                x1++; y1++;
                
            }
        }

        void QueenUnPlaced(int i, int j){
            for (int x = 0; x < _n; x++) // x axis
            {
                _board[x, j] = -1;
            }
            for (int x = 0; x < _n; x++) // y axis
            {
                _board[i, x] = -1;
            }

            int x1 = i, y1 = j;
            while (x1 < _n && y1 >= 0)
            {
                _board[x1, y1] = -1;
                x1++; y1--;
                
            }
            x1 = i; y1 = j;
            while (x1 >= 0 && y1 < _n)
            {
                _board[x1, y1] = -1;
                x1--; y1++;
                
            }
            x1 = i; y1 = j;
            while (x1 >= 0 && y1 >= 0)
            {
                _board[x1, y1] = -1;
                x1--; y1--;
                
            }
            x1 = i; y1 = j;
            while (x1 < _n && y1 < _n)
            {
                _board[x1, y1] = -1;
                x1++; y1++;
                
            }
        }

        void PlaceQueenRecursive(int d){
            // dth queen is placed in dth row only, in any of n columns of dth row
            if(d >= _n){
                //all n queens are placed
                _allBoards.Add(_board); // hopefully it adds copy not reference
                return;
            }

            for(int col=0;col<_n;col++){
                if(IsSafe(d,col)){
                    _occupiedCols[col] = true;
                    QueenPlaced(d,col);
                    _board[d,col] = d+1;
                    PlaceQueenRecursive(d+1);
                    QueenUnPlaced(d,col);
                    _occupiedCols[col] = false;
                }
            }

        }
    }
}
