using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    /// <summary>
    /// 2106. Maximum Fruits Harvested After at Most K Steps
    // Invalid dynamic programming solution - does not consider that harvested places
    // do not have  fruits anymore
    /// </summary>
    public class FruitHarvest
    {

        int[,,] _dp;
        int[][] _fruits;
        int _k;
        int _startPos;
        int[,] _fruitsWithinReach;
        // Dictionary<int,int> positionFruitCounts = new Dictionary<int,int>();
        int _startValue = 0;


        public static void TestCase()
        {
            FruitHarvest fruitHarvest = new FruitHarvest();
            int[][] input = new int[3][];
            input[0] = new int[2] { 2, 8 };
            input[1] = new int[2] { 6, 3 };
            input[2] = new int[2] { 8, 6 };
            int answer = fruitHarvest.MaxTotalFruits(input, 5, 4);
        }

        public static void TestCase1()
        {
            FruitHarvest fruitHarvest = new FruitHarvest();
            int[][] input = new int[6][];
            input[0] = new int[2] { 0, 9 };
            input[1] = new int[2] { 4, 1 };
            input[2] = new int[2] { 5, 7 };
            input[3] = new int[2] { 6, 2 };
            input[4] = new int[2] { 7, 4 };
            input[5] = new int[2] { 10, 9 };
            int answer = fruitHarvest.MaxTotalFruits(input, 5, 4);
        }

        private int[,] ValidPositions()
        {
            List<int[]> validPositions = new List<int[]>();
            validPositions.Add(new int[2] { 0, 0 });
            for (int i = 0; i < _fruits.Length; i++)
            {
                if (Math.Abs(_startPos - _fruits[i][0]) <= _k)
                {
                    validPositions.Add(new int[2] { _fruits[i][0], _fruits[i][1] });
                    if (_startPos - _fruits[i][0] == 0)
                    {
                        _startValue = _fruits[i][1];
                    }
                }
            }
            // return validPositions.ToArray();
            int[,] validFruits = new int[validPositions.Count(), 2];
            for (int i = 0; i < validPositions.Count(); i++)
            {
                validFruits[i, 0] = validPositions[i][0];
                validFruits[i, 1] = validPositions[i][1];
            }
            return validFruits;
        }

        const int LEFT_EMPTY = 1;
        const int RIGHT_EMPTY = 2;
        const int BOTH_EMPTY = 0;
        const int BOTH_FILLED = 4;

        public int MaxTotalFruits(int[][] fruits, int startPos, int k)
        {
            _fruits = fruits;
            _k = k;
            _startPos = startPos;

            // validation

            //ignore positions that are more than k-steps away

            //collect all fruits at current position (if any)

            //dp of remaining positions



            // create new array of fruits that are k distance from current position
            _fruitsWithinReach = ValidPositions();
            
            _dp = new int[_fruitsWithinReach.GetLength(0), k + 1, 4]; // 0 => both left/right empty, 1 = left empty, 2 = right empty, 4 = both filled
            for (int i = 0; i < _dp.GetLength(0); i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    _dp[i, 0,j] = _fruitsWithinReach[i, 1];
                }
            }

            for (int i = 0; i < k + 1; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    _dp[0, i,j] = 0;
                }
            }

            for (int j = 1; j <= k; j++)
            {
                
                for (int i = 1; i < _dp.GetLength(0); i++)
                {
                    _dp[i, j, BOTH_EMPTY] = _fruitsWithinReach[i, 1];
                    int v_left = -1;
                    int v_right = -1;
                    int steps_to_left = (i==0 || i == 1) ? -1 : Math.Abs( _fruitsWithinReach[i, 0] - _fruitsWithinReach[i - 1, 0]);
                    int steps_to_right = (i== _fruitsWithinReach.GetLength(0)-1) ? -1 :  Math.Abs(_fruitsWithinReach[i, 0] - _fruitsWithinReach[i + 1, 0]);
                    if(steps_to_right != -1 && j >= steps_to_right)
                    {
                        v_right = _dp[i + 1, j-steps_to_right, LEFT_EMPTY];
                    }
                    if(steps_to_left != -1 && j >= steps_to_left)
                    {
                        v_left = _dp[i - 1, j-steps_to_left, RIGHT_EMPTY  ];
                    }
                    if (v_left == -1) 
                    {
                        if (v_right != -1) {  // right can be reached left cannot be reached
                            _dp[i, j, BOTH_FILLED] = _fruitsWithinReach[i, 1] +  v_right ;
                            _dp[i, j, LEFT_EMPTY] = _fruitsWithinReach[i, 1] + v_right;
                            _dp[i, j, RIGHT_EMPTY] = _fruitsWithinReach[i, 1]; //right can be reached but right is empty
                        }
                        else // both sides cannot be reached
                        {
                            _dp[i, j, LEFT_EMPTY] = _fruitsWithinReach[i, 1];
                            _dp[i, j, RIGHT_EMPTY] = _fruitsWithinReach[i, 1];
                            _dp[i, j, BOTH_FILLED] = _fruitsWithinReach[i, 1]; // only curren fruits can be added sicne both left/right cannot be gone to
                        }
                    }
                    else
                    {
                        if (v_right == -1) { // left can be reached right cannot be reached
                            _dp[i, j, BOTH_FILLED] = _fruitsWithinReach[i, 1] +   v_left ;
                            _dp[i, j, RIGHT_EMPTY] = _fruitsWithinReach[i, 1] + v_left;
                            _dp[i, j, LEFT_EMPTY] = _fruitsWithinReach[i, 1]; // left can be reached but is empty
                        }
                        else
                        {
                            //both left and right could be mvoed to
                            _dp[i, j, BOTH_FILLED] = _fruitsWithinReach[i, 1] + Math.Max(v_left, v_right);
                            _dp[i, j, LEFT_EMPTY] = _fruitsWithinReach[i, 1] + v_right;
                            _dp[i, j, RIGHT_EMPTY] = _fruitsWithinReach[i, 1] + v_left;
                        }
                    }
                    

                }
            }

            int leftStart = -1, rightStart = -1, leftIndex = -1, rightIndex = -1;
            for(int i=0;i<_fruitsWithinReach.GetLength(0);i++)
            {
                if (_fruitsWithinReach[i,0] <= _startPos)
                {
                    leftStart = _fruitsWithinReach[i,0];
                    leftIndex = i;
                }

            }

            for (int i = 0; i < _fruitsWithinReach.GetLength(0); i++)
            {
                if (_fruitsWithinReach[i, 0] >= _startPos)
                {
                    rightStart = _fruitsWithinReach[i, 0];
                    rightIndex = i;
                    break;
                }

            }

            if (leftIndex == rightIndex)
            {
                return  _dp[rightIndex, k, BOTH_FILLED];
            }
            else
            {

                return  Math.Max(_dp[leftIndex, k - Math.Abs(_startPos - leftStart), BOTH_FILLED], 
                    _dp[rightIndex, k - Math.Abs(_startPos - rightStart), BOTH_FILLED]);
            }

        }
    }
}
