using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgoDemos.puzzles
{

    /*
     * plass N queens on chess board so that no queen threatens other
     * */
    public class Nqueen
    {
        int[,] chessBoard = null;
        List<int[,]> matchingChessBoards = new List<int[,]>();
        int[] queenStatus = null;
        int NOT_PLACED = -1;
        int PLACED = 1;
        int HASQUEEN = 1; //TODO short might suffice
        int EMPTY = -2;
        int THREAT = -1;
        int nq = 1;
        public Nqueen(int numQueens)
        {
            nq = numQueens;
            chessBoard = new int[8, 8];
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    chessBoard[r, c] = EMPTY;
                }
            }

            queenStatus = new int[nq];
            for(int i = 0; i < nq; i++)
            {
                queenStatus[i] = NOT_PLACED;
            }
        }


        public void demo()
        {

            PlaceQueens(nq);
        }

        /// <summary>
        /// Assumptions  :
        /// 1. All queens are equal. So positions where one queen is at N queens are at
        ///     N places in chessBoard is same if the queens are swapped
        /// 2. Function finds all the configurations that match given conditions (no threat)
        /// </summary>
        /// <param name="nq">Number of queens</param>
        void PlaceQueens(int nq)
        {
            /********** input validation **********/
            // 2 or more queens

            /********** start: algo - brute force **********/
            // start with one queen place it all valid squares on the chess board 
            // while exists queens that are not placed take next queen and place at 
            // valid square. once all queens are placed note the chessBaord snapshot
            

            for (int r = 0; r < 8; r++) {
                for (int c = 0; c < 8; c++) {
                    // get a unplaced queen
                    int unplacedQueen = getUnplacedQueen(queenStatus, nq);
                    if (unplacedQueen != -1)
                    {
                        //place queen at current position and recursive call function 
                        if(placeQueen(unplacedQueen, r, c))
                        {
                            unplacedQueen = getUnplacedQueen(queenStatus, nq);
                            if (unplacedQueen == -1)
                            {
                                saveSnapshot();
                                unplaceQueen(nq - 1); // last queen is always unplaced so that it moves to next position
                                
                            }
                            else
                            {
                                PlaceQueens(nq);
                            }
                        }
                        // queen cannot placed at current position .. move to next iteration
                    }
                    else // no unplaced queen? then save the chessboard snapshot
                    {
                        //should not get here
                        saveSnapshot();
                        // move to next iteration
                        unplaceQueen(nq-1); // last queen is always unplaced so that it moves to next position
                    }
                }
            }
            /********** end: algo - brute force **********/

            /********** result validation **********/

        }

        void saveSnapshot()
        {
            matchingChessBoards.Add(chessBoard); // TODO make sure that a copy is added
        }

        bool isThreat(int row, int col)
        {
            return chessBoard[row,col] != EMPTY;
        }

        void printChessBoard()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            for (int a = 0; a < 8; a++)
            {
                for(int b = 0; b < 8; b++)
                {
                    sb.Append(chessBoard[a,b] + "\t");
                }
                sb.AppendLine();
            }
            Console.Write(sb.ToString());
        }

        void updateThreatsquares(int queenNumber, bool isAdded, int row, int col)
        {
            if (isAdded)
            {
                var nums = Enumerable.Range(0, 8);
                foreach(int n in nums)
                {
                    chessBoard[row, n] = chessBoard[row, n] + THREAT;
                }
                foreach (int n in nums)
                {
                    chessBoard[n, col] = chessBoard[n, col] + THREAT;
                }
                chessBoard[row, col] = queenNumber;
            }
            else //removed
            {
                var nums = Enumerable.Range(0, 8);
                foreach (int n in nums)
                {
                    chessBoard[row, n] = chessBoard[row, n] - THREAT;
                }
                foreach (int n in nums)
                {
                    chessBoard[n, col] = chessBoard[n, col] - THREAT;
                }
                chessBoard[row, col] = EMPTY;
            }
            printChessBoard();
        }

        bool unplaceQueen(int queenNum)
        {
            bool resp = false;
            for(int a = 0; a < 8; a++)
            {
                for (int b = 0; b < 8; b++)
                {
                    if(chessBoard[a,b] == queenNum)
                    {
                        queenStatus[queenNum] = NOT_PLACED;
                        updateThreatsquares(queenNum, false, a, b);
                        resp = true;
                        return resp;
                    }
                }
            }
            return resp;
        }

        bool placeQueen(int queenNum, int row, int col)
        {
            bool resp = false;
            if(!isThreat(row, col))
            {
                chessBoard[row, col] = queenNum;
                queenStatus[queenNum] = PLACED;
                updateThreatsquares(queenNum, true, row, col);
                return true;
            }
            
            return resp;
        }

        void initQueens(int nq)
        {
            int[] queenStatus = new int[nq]; // TODO change to bool[]

            for (int i = 0; i < nq; i++)
            {
                queenStatus[i] = NOT_PLACED;
            }
        }

        int getUnplacedQueen(int[] queenStatus, int nq)
        {
            for (int i = 0; i < nq; i++)
            {
                if (queenStatus[i] == NOT_PLACED)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
