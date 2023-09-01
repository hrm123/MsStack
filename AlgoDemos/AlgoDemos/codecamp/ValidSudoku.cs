using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    /*
     * Runtime 119 ms (Beats 17% users with C#)
     * Memory 47.74 MB (beats 22.27% users with C#)
     * */
    public class ValidSudoku
    {

        Dictionary<int, int> _intCounts = new Dictionary<int, int>(); // utility
                                                                      // convert character board to number board
        int[][] _boardNum = {
        new int[]{0,0,0,0,0,0,0,0,0},
        new int[]{0,0,0,0,0,0,0,0,0},
        new int[]{0,0,0,0,0,0,0,0,0},
        new int[]{0,0,0,0,0,0,0,0,0},
        new int[]{0,0,0,0,0,0,0,0,0},
        new int[]{0,0,0,0,0,0,0,0,0},
        new int[]{0,0,0,0,0,0,0,0,0},
        new int[]{0,0,0,0,0,0,0,0,0},
        new int[]{0,0,0,0,0,0,0,0,0},
        };


        bool UpdateCurrentCount(int current)
        {
            if (current != -1)
            {
                if (current > 9 && current < 1) { return false; }
                if (_intCounts.ContainsKey(current))
                {
                    _intCounts[current] = _intCounts[current] + 1;
                }
                else { _intCounts[current] = 0; }
            }
            return true;
        }

        bool AreNumsValid(int x, int y)
        {
            _intCounts.Clear();
            //validate x=0/3/6 & y=0/3/6
            if (x != -1 && y != -1)
            {
                if (x != 0 && x != 3 && x != 6 && y != 0 && y != 3 && y != 6)
                {
                    Console.WriteLine("invalid input");
                    Console.WriteLine($"x={x}, y={y}");
                    return false;
                }
            }

            // initialize to 0 for numbers 1 to 9
            for (int i = 1; i < 10; i++)
            {
                _intCounts[i] = 0;
            }


            if (x != -1 && y != -1)
            { //validate 3x3 matrix starting at (x,y)
                for (int i = x; i < x + 3; i++)
                    for (int j = y; j < y + 3; j++)
                    {
                        int current = _boardNum[i][j];
                        // Console.WriteLine($"{current}");
                        if (!UpdateCurrentCount(current))
                        {
                            return false;
                        }
                    }
            }
            else if (x != -1)
            { //validate 9 elments of row'x'
                // Console.WriteLine($"validarting row {x}");
                for (int j = 0; j < 9; j++)
                {
                    int current = _boardNum[x][j];
                    Console.WriteLine($"{current}");
                    if (!UpdateCurrentCount(current))
                    {
                        return false;
                    }
                }
            }
            else if (y != -1)
            { //validate 9 elments of column 'y'
                for (int j = 0; j < 9; j++)
                {
                    int current = _boardNum[j][y];
                    if (!UpdateCurrentCount(current))
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

            foreach (var (key, value) in _intCounts)
            {
                if (value > 1)
                { // repetition
                    return false;
                }
            }
            return true;
        }


        public bool IsValidSudoku(char[][] board)
        {


            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                {
                    _boardNum[i][j] = (board[i][j] == '.') ? -1 : Int32.Parse(board[i][j] + "");
                }


            // row validation
            for (int x = 0; x < 9; x++)
            {
                Console.WriteLine($" validation  x={x}");
                if (!AreNumsValid(x, -1))
                {
                    // Console.WriteLine($"Row validation failed x={x}");
                    return false;
                }
            }

            // column validation
            for (int y = 0; y < 9; y++)
            {
                Console.WriteLine($" validation  y={y}");
                if (!AreNumsValid(-1, y))
                {
                    // Console.WriteLine($"column validation failed y={y}");
                    return false;
                }
            }

            // 3x3 matrix validation
            for (int i = 0; i < 9; i = i + 3)
                for (int j = 0; j < 9; j = j + 3)
                {
                    if (!AreNumsValid(i, j))
                    {
                        // Console.WriteLine($"sub matrix validation failed x={i},y={j}");
                        return false;
                    }
                }
            return true;
        }
    }
}
