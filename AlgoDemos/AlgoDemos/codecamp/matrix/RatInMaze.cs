using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp.matrix
{
    public class RatInMaze
    {
        int[][] _soln;
        int[][] _matrix;
        int _len_i = 0, _len_j = 0;


        public static void Testcase()
        {
            RatInMaze rim = new RatInMaze();
            int[][] matrx = new int[3][];
            matrx[0] = new int[3] {1,0,1};
            matrx[1] = new int[3] {1,1,0};
            matrx[2] = new int[3] {0,1,1};
            bool answer = rim.FindRoute(matrx);
            Console.Write(answer);
        }

        bool FindRoute(int[][] matrix)
        {
            _len_i = matrix.GetLength(0);
            _len_j = matrix[0].Length;
            _soln = new int[_len_i][];
            _matrix = matrix;
            for(int i1=0; i1< _len_i; i1++)
            {
                _soln[i1] = new int[_len_j];
                Array.Fill(_soln[i1], -1);
            }

            //always starts at (0,0) goes to (n-1,n-1)
            bool answr = RecursiveSolution(0, 0);
            return answr;
        }


        bool CanMoveRight(int i, int j)
        {
            return _soln[i][j]!=0 && i +1<_len_i &&  _matrix[i + 1][j] == 1;
        }

        bool CanMoveDown(int i, int j)
        {
            return _soln[i][j] != 0 &&  j +1<_len_j && _matrix[i][j+1] == 1;
        }

        bool RecursiveSolution(int i, int j)
        {
            _soln[i][j] = 1;

            if (i==_len_i-1 && j == _len_j-1)
            {
                //solution reached
                return true;

            }
            bool canMoveR = CanMoveRight(i, j);
            bool canMoveD = CanMoveDown(i, j);
            if(!canMoveR && !canMoveD) {
                _soln[i][j] = 0;
                return false; // end of recursion of this branch
            }
            if (canMoveR && RecursiveSolution(i + 1, j))
            {
               return true;
            } else if (canMoveD && RecursiveSolution(i, j + 1))
            {
                return true;
            }
            _soln[i][j] = 0;
            return false;

        }
    }
}
