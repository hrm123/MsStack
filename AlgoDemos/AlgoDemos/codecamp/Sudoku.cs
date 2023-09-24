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
    /// </summary>
    public class Sudoku
    {
        int[,] _board = new int[9, 9];
        int[,] _boardOrig = new int[9, 9];
        bool[,] _empty = new bool[9, 9];

        public static void TestCase()
        {
            char[,] input = new char[9,9] { { '5', '3', '.', '.', '7', '.', '.', '.', '.' }, { '6', '.', '.', '1', '9', '5', '.', '.', '.' }, { '.', '9', '8', '.', '.', '.', '.', '6', '.' }, { '8', '.', '.', '.', '6', '.', '.', '.', '3' }, { '4', '.', '.', '8', '.', '3', '.', '.', '1' }, { '7', '.', '.', '.', '2', '.', '.', '.', '6' }, { '.', '6', '.', '.', '.', '.', '2', '8', '.' }, { '.', '.', '.', '4', '1', '9', '.', '.', '5' }, { '.', '.', '.', '.', '8', '.', '.', '7', '9' } };
            char[][] inputFormatted = new char[input.GetLength(0)][];
            for (int i = 0; i < 9; i++)
            {
                inputFormatted[i] = new char[9];
                for (int j = 0; j < 9; j++)
                {
                    inputFormatted[i][j] = input[i, j];
                }
            }
            Sudoku s = new Sudoku();
            s.SolveSudoku(inputFormatted);
        }

        List<int[]> _threexthrees = new List<int[]> { 
            new int[] { 0,0, 2,2}, new int[] { 3, 0, 5, 2 }, new int[] { 6, 0, 8, 2 },
            new int[] { 0,3, 2,5}, new int[] { 3, 3, 5, 5 }, new int[] { 6, 3, 8, 5 },
            new int[] { 0,6, 2,8}, new int[] { 3, 6, 5, 8 }, new int[] { 6, 6, 8, 8 },

        };
        void ConvertToSudokuBoard(char[][] board)
        {
            //char[][] to int[,]

            for(int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (board[i][j] == '.')
                    {
                        _empty[i, j] = true;
                        _board[i, j] = 0;
                        _boardOrig[i, j] = 0;
                    }
                    else
                    {
                        _empty[i, j] = false;
                        _board[i, j] = (int)char.GetNumericValue(board[i][j]);
                        _boardOrig[i, j] = _board[i, j];
                    }
                }
            }
        }

        char[][] ConvertBoardToResult()
        {
            var outp =  new char[1][];
            return outp;
        }

        public void SolveSudoku(char[][] board)
        {
            ConvertToSudokuBoard(board);

            SudokuRecursive();


            ConvertBoardToResult();
        }

        private bool MatrixNotFilled(int x1, int y1, int x2, int y2)
        {
            // check if any of cell of 9x9 is 0
            for(int i=x1; i<=x2; i++)
            {
                for (int j = x1; j <= x2; j++)
                {
                    if (_board[i,j] == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private Dictionary<int, int> GetNumbersDict()
        {
            Dictionary<int, int> numbers = new Dictionary<int, int>();
            for (int i = 1; i <= 9; i++)
            {
                numbers[i] = 0;
            }
            return numbers;
        }


        private int GetRowRemainingNumbers(int col)
        {
            int remaining = 0;
            var numbers = GetNumbersDict();
            for (int i = 0; i < 9; i++)
            {
                if (_boardOrig[i, col] != 0)
                {
                    numbers[_boardOrig[i, col]]++;
                }
            }

            for (int i = 1; i <= 9; i++)
            {
                if (numbers[i] == 0)
                {
                    remaining |= 1 << i;
                }
            }
            return remaining;
        }

        private int GetColumnRemainingNumbers(int row)
        {
            int remaining = 0;
            var numbers = GetNumbersDict();
            for (int i = 0; i < 9; i++)
            {
                if (_boardOrig[row, i] != 0)
                {
                    numbers[_boardOrig[row, i]]++;
                }
            }

            for (int i = 1; i <= 9; i++)
            {
                if (numbers[i] == 0)
                {
                    remaining |= 1 << i;
                }
            }
            return remaining;

        }


        private void FillMatrix(int x1, int y1, int x2, int y2)
        {
            // collect all filled values and make int with all remaining numbers
            int remaining = 0;
            var numbers = GetNumbersDict();

            for (int i = x1; i <= x2; i++)
            {
                for (int j = x1; j <= x2; j++)
                {
                    if (_boardOrig[i, j] != 0)
                    {
                        numbers[_boardOrig[i, j]]++;
                    }
                }
            }

            for (int i = 1; i <= 9; i++)
            {
                if(numbers[i] == 0)
                {
                    remaining |= 1 << i;
                }
            }


            // fill that int of remaining numbers in all the empty spaces
            for (int i = x1; i <= x2; i++)
            {
                for (int j = x1; j <= x2; j++)
                {
                    if (_boardOrig[i, j] == 0)
                    {
                        int remainingOfRow = GetRowRemainingNumbers(i);
                        int remainingOfColumn = GetColumnRemainingNumbers(j);
                        int remains = remaining & remainingOfColumn & remainingOfRow;
                        //if remains has only one bit set to 1 then that is correct number got
                        int res = findPositionOfTheOnlyOneBit(remains);
                        if (res != -1)
                        {
                            _boardOrig[i, j] = res; // sucessfully filed one empty space
                        }
                        
                    }

                    
                }
            }

        }

        static bool isPowerOfTwo(int n)
        {
            return (n > 0 && ((n & (n - 1)) == 0)) ? true : false;
        }
        static int findPositionOfTheOnlyOneBit(int n)
        {
            if (!isPowerOfTwo(n))
                return -1;

            int i = 1, pos = 1;

            // Iterate through bits of n till we find a set bit
            // i&n will be non-zero only when 'i' and 'n' have a set bit
            // at same position
            while ((i & n) == 0)
            {
                // Unset current bit and set the next bit in 'i'
                i = i << 1;

                // increment position
                ++pos;
            }

            return pos;
        }

        private void SudokuRecursive()
        {
            while (MatrixNotFilled(0,0,8,8))
            {
                foreach(var item in _threexthrees)
                {
                    if (MatrixNotFilled(item[0], item[1], item[2], item[3])){
                        FillMatrix(item[0], item[1], item[2], item[3]);
                    }
                }
            }
        }
    }
}
