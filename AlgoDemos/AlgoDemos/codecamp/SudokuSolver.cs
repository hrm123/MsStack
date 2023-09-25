using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    /// <summary>
    /// 37. Sudoku solver - Each of 1-9 should occur exactly  one time  in each row. Each of 1-9 should occur exactly  one time in each column.
    ///  Each of 1-9 should occur exactly  one time in each 3x3 subgrid. Input is 9x9 grid. Some cells are filled some cells are empty.
    ///  Have to output entirely filled grid according to given rules.
    ///  174ms - beats 13.8% of c# users. 46 MB - beats 26% of c# users
    /// </summary>
    public class SudokuSolver
    {
        int[,] _board = new int[9, 9];

        public static void TestCase()
        {
            // char[,] input = new char[9,9] { { '5', '3', '.', '.', '7', '.', '.', '.', '.' }, { '6', '.', '.', '1', '9', '5', '.', '.', '.' }, { '.', '9', '8', '.', '.', '.', '.', '6', '.' }, { '8', '.', '.', '.', '6', '.', '.', '.', '3' }, { '4', '.', '.', '8', '.', '3', '.', '.', '1' }, { '7', '.', '.', '.', '2', '.', '.', '.', '6' }, { '.', '6', '.', '.', '.', '.', '2', '8', '.' }, { '.', '.', '.', '4', '1', '9', '.', '.', '5' }, { '.', '.', '.', '.', '8', '.', '.', '7', '9' } };
            char[,] input = new char[9, 9] { { '.', '.', '9', '7', '4', '8', '.', '.', '.' }, { '7', '.', '.', '.', '.', '.', '.', '.', '.' }, { '.', '2', '.', '1', '.', '9', '.', '.', '.' }, { '.', '.', '7', '.', '.', '.', '2', '4', '.' }, { '.', '6', '4', '.', '1', '.', '5', '9', '.' }, { '.', '9', '8', '.', '.', '.', '3', '.', '.' }, { '.', '.', '.', '8', '.', '3', '.', '2', '.' }, { '.', '.', '.', '.', '.', '.', '.', '.', '6' }, { '.', '.', '.', '2', '7', '5', '9', '.', '.' } };
            char[][] inputFormatted = new char[input.GetLength(0)][];
            for (int i = 0; i < 9; i++)
            {
                inputFormatted[i] = new char[9];
                for (int j = 0; j < 9; j++)
                {
                    inputFormatted[i][j] = input[i, j];
                }
            }
            SudokuSolver s = new SudokuSolver();
            s.SolveSudoku(inputFormatted);
        }


        void ConvertToSudokuBoard(char[][] board)
        {
            //char[][] to int[,]

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (board[i][j] == '.')
                    {
                        _board[i, j] = 0;
                    }
                    else
                    {
                        _board[i, j] = (int)char.GetNumericValue(board[i][j]);
                    }
                }
            }
        }

        char[][] ConvertBoardToResult()
        {
            var outp = new char[9][];
            for (int i = 0; i < 9; i++)
            {
                outp[i] = new char[9];
                for (int j = 0; j < 9; j++)
                {
                    outp[i][j] = _board[i, j].ToString()[0];
                }
            }


            return outp;
        }

        public void SolveSudoku(char[][] board)
        {
            _n = 9;
            ConvertToSudokuBoard(board);
            if (!RecursiveSudoku())
            {
                Console.WriteLine("sudoku failed");
                return;
            }

            var outp = ConvertBoardToResult();



            for (int i = 0; i < _n; i++)
            {
                for (int j = 0; j < _n; j++)
                {
                    board[i][j] = outp[i][j];
                }
            }
        }

        int _n;

        private bool RecursiveSudoku()
        {
            
            int i=0, j=0;
            bool zeroFound = false;
            for(i= 0; i < _n; i++)
            {
                for (j = 0; j < _n; j++)
                {
                    if (_board[i,j] == 0)
                    {
                        zeroFound = true;
                        break;
                    }
                }
                if(zeroFound)
                {
                    break;
                }

            }
            // Console.WriteLine($"i={i},j={j}");
            if(i==_n && j ==_n) { return true; } // end of recursion
            for(int x = 1; x <= _n; x++) // all the possible 'n' numbers are tried one at at time in current cell which has value 0
            {
                if (IsSafe(i, j, x))
                {
                    _board[i, j] = x;
                    if (RecursiveSudoku()) { return true; }
                    _board[i,j] = 0;
                }
            }
            return false; // none of [1,n] values works in current cell for solution

        }

        private bool IsSafe(int i, int j, int x)
        {
            for(int  k=0;k<_n;k++ )
            {
                if (_board[k,j] == x || _board[i,k]== x) // check both column and row that corresponds to give (i,j)
                {
                    return false;
                }
            }
            int s = (int) Math.Sqrt(_n);
            int rs = i - i % s;
            int ls = j - j % s;
            for (i = 0; i < s; i++)
            {
                for (j = 0; j < s; j++)
                {
                    if (_board[i+rs, j + ls] == x)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
