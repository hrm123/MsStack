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
        string[,] _allNumbers = new string[9, 9];
        Dictionary<int,List<int>> _remains = new Dictionary<int,List<int>>();
        int _ctr = 0;
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
                        _allNumbers[i, j] = "";
                        _board[i, j] = 0;
                        _boardOrig[i, j] = 0;
                    }
                    else
                    {
                        _allNumbers[i, j] = board[i][j]+",";
                        _board[i, j] = (int)char.GetNumericValue(board[i][j]);
                        _boardOrig[i, j] = _board[i, j];
                    }
                }
            }
        }

        char[][] ConvertBoardToResult()
        {
            var outp =  new char[9][];
            for(int i = 0; i < 9; i++)
            {
                outp[i] = new char[9];
                for (int j = 0; j < 9; j++)
                {
                    outp[i][j] = _boardOrig[i, j].ToString()[0];
                }
            }


            return outp;
        }

        public void SolveSudoku(char[][] board)
        {
            ConvertToSudokuBoard(board);

            SudokuRecursive();


            char[][] outp = ConvertBoardToResult();


            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    board[i][j] = outp[i][j];
                }
            }
        }

        private bool MatrixNotFilled(int x1, int y1, int x2, int y2)
        {
            // check if any of cell of 9x9 is 0
            for(int i=x1; i<=x2; i++)
            {
                for (int j = y1; j <= y2; j++)
                {
                    if (_boardOrig[i,j] == 0)
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

        private Tuple<int, List<int>> GetSubMatrixRemainingNumbers(int x1, int y1, int x2, int y2)
        {
            int remaining = 0;
            var numbers = GetNumbersDict();
            List<int> list = new List<int>();

            for (int i = x1; i <= x2; i++)
            {
                for (int j = y1; j <= y2; j++)
                {
                    if (_boardOrig[i, j] != 0)
                    {
                        numbers[_boardOrig[i, j]]++;
                    }
                }
            }

            for (int i = 1; i <= 9; i++)
            {
                if (numbers[i] == 0)
                {
                    list.Add(i);
                    remaining |= 1 << i;
                }
            }

            return new Tuple<int, List<int>>(remaining, list);
        }


        private Tuple<int, List<int>> GetRowRemainingNumbers(int col)
        {
            int remaining = 0;
            var numbers = GetNumbersDict();
            List<int> list = new List<int>();
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
                    list.Add(i);
                    remaining |= 1 << i;
                }
            }
            return new Tuple<int, List<int>>(remaining, list);
        }

        private Tuple<int,List<int>> GetColumnRemainingNumbers(int row)
        {
            int remaining = 0;
            List<int> list = new List<int>();
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
                    list.Add(i);
                    remaining |= 1 << i;
                }
            }
            return new Tuple<int,List<int>>(remaining, list);

        }

        
        void FillRow(int col, int n)
        {
            for (int i = 0; i < 9; i++)
            {
                if (_boardOrig[i, col] == 0)
                {
                    _boardOrig[i, col] = n;
                    _board[i, col] = n;
                    _allNumbers[i, col] = n.ToString();
                    _ctr++;
                }
            }
        }

        private void FillSubMatrix(int x1, int y1, int x2, int y2, int n)
        {
            List<int> list = new List<int>();

            for (int i = x1; i <= x2; i++)
            {
                for (int j = y1; j <= y2; j++)
                {
                    if (_boardOrig[i, j] == 0)
                    {
                        _boardOrig[i, j] = n;
                        _board[i, j] = n;
                        _allNumbers[i, j] = n.ToString();
                        _ctr++;
                    }
                }
            }
        }

        private string GetNumbersDelimited(int n)
        {

            StringBuilder sb = new StringBuilder();
            // Iterate through bits of n till we find a set bit
            // i&n will be non-zero only when 'i' and 'n' have a set bit
            // at same position
            for(int i=1;i<=9;i++)
            {
                if ((1<<i & n) > 0)
                {
                    sb.Append($"{i},");
                }
            }
            return sb.ToString();

        }


        private int[] GetNumbersOfState(int n)
        {
            List<int> list = new List<int> ();
            StringBuilder sb = new StringBuilder();
            // Iterate through bits of n till we find a set bit
            // i&n will be non-zero only when 'i' and 'n' have a set bit
            // at same position
            for (int i = 1; i <= 9; i++)
            {
                if ((1 << i & n) > 0)
                {
                    list.Add(i);
                }
            }
            return list.ToArray();

        }

        private void FillMatrix(int x1, int y1, int x2, int y2)
        {
            // collect all filled values and make int with all remaining numbers
            var remaining = GetSubMatrixRemainingNumbers(x1, y1, x2, y2);

            // fill that int of remaining numbers in all the empty spaces
            for (int i = x1; i <= x2; i++)
            {
                for (int j = y1; j <= y2; j++)
                {
                    if (_boardOrig[i, j] == 0)
                    {
                        var remainingOfRow = GetRowRemainingNumbers(j);
                        var remainingOfColumn = GetColumnRemainingNumbers(i);
                        int remains = (remaining.Item1 & remainingOfRow.Item1) & remainingOfColumn.Item1;
                        if (_board[i,j]>0)
                        {
                            //this means mixed state is set here by earlier iterations
                            remains = remains & _board[i, j];
                        }
                        //if remains has only one bit set to 1 then that is correct number got
                        int res = findPositionOfTheOnlyOneBit(remains);
                        if (res != -1)
                        {
                            _boardOrig[i, j] = res; // sucessfully filed one empty space
                            _board[i,j] = res;
                            _allNumbers[i, j] = res.ToString();
                            _ctr++;
                        } else
                        {
                            _board[i, j] = remains;
                            _allNumbers[i, j] = GetNumbersDelimited(remains);
                        }
                        
                    }

                    
                }
            }

        }


        private void FillCol(int row, int n)
        {
            for (int i = 0; i < 9; i++)
            {
                if (_boardOrig[row, i] == 0)
                {
                    _boardOrig[row, i] = n;
                    _board[row, i] = n;
                    _allNumbers[row, i] = n.ToString();
                    _ctr++;
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

            int i = 1, pos = 0;

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

        Tuple<int, int, int[]>[] ListOfZero(int[,] newBoard)
        {
            List<Tuple<int, int, int[]>> outp = new List<Tuple<int, int, int[]>>();
            for (int i = 0; i < _board.GetLength(0); i++)
            {
                for (int j = 0; j < _board.GetLength(1); j++)
                {
                    if (newBoard[i, j] == 0)
                    {
                        int[] numbers = GetNumbersOfState(_board[i, j]);
                        outp.Add(new Tuple<int, int, int[]>(i, j, numbers));
                    }
                }
            }
            return outp.ToArray();
        }




        private bool IsSafeRow(int[,] newBoard, int rowNumber)
        {
            
            int total = 0;
            for (int j = 0; j < newBoard.GetLength(1); j++)
            {
                total += newBoard[rowNumber, j];
            }
            if (total != 45)
            {
                return false;
            }

            var numbers = GetNumbersDict();
            List<int> list = new List<int>();

            for (int j = 0; j < newBoard.GetLength(1); j++)
            {
                  numbers[newBoard[rowNumber, j]]++;
            }

            for (int i = 1; i <= 9; i++)
            {
                if (numbers[i] > 1)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsSafeColumn(int[,] newBoard, int colNumber)
        {
            int total = 0;
            for (int j = 0; j < newBoard.GetLength(1); j++)
            {
                total += newBoard[j, colNumber];
            }
            if (total != 45)
            {
                return false;
            }

            var numbers = GetNumbersDict();
            List<int> list = new List<int>();

            for (int j = 0; j < newBoard.GetLength(1); j++)
            {
                numbers[newBoard[j, colNumber]]++;
            }

            for (int i = 1; i <= 9; i++)
            {
                if (numbers[i] > 1)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsSafe(int[,] newBoard)
        {
            for (int i = 0; i < newBoard.GetLength(0); i++)
            {
                int total = 0;
                for (int j = 0; j < newBoard.GetLength(1); j++)
                {
                    total += newBoard[i, j];
                }
                if(total != 36)
                {
                    return false;
                }
            }

            for (int i = 0; i < newBoard.GetLength(0); i++)
            {
                int total = 0;
                for (int j = 0; j < newBoard.GetLength(1); j++)
                {
                    total += newBoard[j, i];
                }
                if (total != 36)
                {
                    return false;
                }
            }
            return true;
        }

        /*
        private bool FindNumberByElimination(int[,] newBoard, Tuple<int, int, int[]>[] zeros, int index)
        {
            if(index == zeros.Length) //end of recursion
            {
                return false;
                

            }
            else
            {
                if (IsSafe(newBoard))
                {
                    return true;
                }
            }
            var current = zeros[index];
            for (int i = 0; i < current.Item3.Length; i++)
            {
                newBoard[current.Item1, current.Item2] = current.Item3[i];
                if (FindNumberByElimination(newBoard, zeros, index+1))
                {
                    return true;
                }
            }
            return false;
        }
        */
       


        private void SudokuRecursive()
        {
            while (MatrixNotFilled(0,0,8,8))
            {
                _ctr = 0;
                for(int i=0;i<_threexthrees.Count;i++)
                {
                    var item = _threexthrees[i];
                    if (MatrixNotFilled(item[0], item[1], item[2], item[3])){

                        var remaining = GetSubMatrixRemainingNumbers(item[0], item[1], item[2], item[3]);
                        if (remaining.Item2.Count() == 1)
                        {
                            // only one remaining number can be filled
                            FillSubMatrix(item[0], item[1], item[2], item[3], remaining.Item2[0]);
                            continue; // since state updated dont do further processing till next iteration
                        }

                        for (int y = 0; y < 9; y++)
                        {
                            var remainingOfColumn = GetColumnRemainingNumbers(y);
                            if (remainingOfColumn.Item2.Count() == 1)
                            {
                                // only one remaining number can be filled
                                FillCol(y, remainingOfColumn.Item2[0]);
                                continue; // since state updated dont do further processing till next iteration
                            }
                        }

                        for (int j = 0; j < 9; j++)
                        {
                            var remainingOfRow = GetRowRemainingNumbers(j);
                            if (remainingOfRow.Item2.Count() == 1)
                            {
                                // only one remaining number can be filled
                                FillRow(j, remainingOfRow.Item2[0]);
                                continue; // since state updated dont do further processing till next iteration
                            }
                        }
                        FillMatrix(item[0], item[1], item[2], item[3]);
                    }
                }
                if(_ctr == 0)
                {

                    int[,] newBoard = new int[9, 9];

                    ;
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            newBoard[i, j] = _boardOrig[i, j];
                        }
                    }

                    // no change happened after iterating all elements.. now change approach to elimination strategy
                    // var zeros = ListOfZero(newBoard);
                    // FindNumberByElimination(newBoard, zeros, 0);

                    for(int i=0;i < 9; i++)
                    {
                        _rowCtr = 0;
                        if(!FixRow(newBoard, i, 0))
                        {
                            _ctr = 0;
                        }
                    }

                    //final check
                    bool isSafeMatrix = true;
                    for (int i = 0; i < 9; i++)
                    {
                        if (!IsSafeRow(newBoard, i))
                        {
                            isSafeMatrix = false;
                        }
                    }

                    for (int i = 0; i < 9; i++)
                    {
                        if (!IsSafeColumn(newBoard, i))
                        {
                            isSafeMatrix = false;
                        }
                    }

                    if (isSafeMatrix)
                    {
                        _board = newBoard;
                        return;
                    }
                    else
                    {
                        throw new Exception("Server error");
                    }



                }
            }
        }
        int _rowCtr = 0;

        private bool FixRow(int[,] newBoard, int rowNumber, int index)
        {
            _rowCtr++;
            if (index == 9)
            {
                return false;
            }
            if(IsSafeRow(newBoard, rowNumber))
            {
                return true;
            }
            for(int col=index;col<9; col++) 
            {
                if (newBoard[rowNumber,col] == 0) 
                {
                    int[] numbers = GetNumbersOfState(_board[rowNumber, col]);
                    for(int j=0;j<numbers.Length; j++)
                    {
                        newBoard[rowNumber, col] = numbers[j];
                        if(FixRow(newBoard, rowNumber, index + 1))
                        {
                            return true;
                        }
                        newBoard[rowNumber, col] = 0;
                    }
                }

            }
            return false;
        }
    }
}
