using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectFour
{
    class Board
    {
        public int PieceCount;
        public int Turn;
        public int[,] Grid;
        public int LastMove;
        public Board(int pieces, int turn, int[,] grid, int last)
        {
            PieceCount = pieces;
            Turn = turn;
            Grid = grid;
            LastMove = last;
        }

        // attempt to add a piece to a column
        // returns true if piece was successfully added to column
        // returns false if column is full
        public bool AddPiece(int player, int column)
        {
            for (int r = 0; r < 6; r++)
            {
                if (this.Grid[r, column] == 0)
                {
                    this.Grid[r, column] = player;
                    PieceCount++;
                    LastMove = column;
                    // change turns
                    Turn = OtherPlayer(Turn);
                    return true;
                }
            }
            return false;
        }

        // determines winner based on the current grid
        // returns 1 if player 1 has won
        // returns 2 if player 2 has won
        // returns 0 if no winner
                                    // DIAGNOLS CAN STILL BE OPTIMIZED! ADD IN QUIT CHECKS LIKE IN HORIZONTAL AND VERTICAL CHECKS
                                                                            // tried but they were buggy and didnt do much
        public int Winner()
        {
            int lineLength = 0;
            int playerChecking = 1;

            //check for a horizontal win
            for (int r = 0; r < 6; r++)
            {
                lineLength = 0;
                for (int c = 0; c < 7; c++)
                {
                    // if no win possible with the length of row remaining, start next row
                    if (7 - c < 4 - lineLength) { break; }

                    // otherwise, continue checking for winner on current row
                    if (this.Grid[r, c] == 0)
                    {
                        lineLength = 0;
                    }
                    else if (this.Grid[r, c] != playerChecking)
                    {
                        lineLength = 1;
                        playerChecking = OtherPlayer(playerChecking);
                    }
                    else
                    {
                        lineLength++;
                        if (lineLength > 3) { return playerChecking; }
                    }
                }
            }

            //check for a vertical win
            for (int c = 0; c < 7; c++)
            {
                lineLength = 0;
                for (int r = 0; r < 6; r++)
                {
                    // if no win possible with the length of column remaining, start next column
                    if (6 - r < 4 - lineLength) { break; }

                    // otherwise, continue checking for winner on current column
                    if (this.Grid[r, c] == 0)
                    {
                        lineLength = 0;
                    }
                    else if (this.Grid[r, c] != playerChecking)
                    {
                        lineLength = 1;
                        playerChecking = OtherPlayer(playerChecking);
                    }
                    else
                    {
                        lineLength++;
                        if (lineLength > 3) { return playerChecking; }
                    }
                }
            }

            // diagnols from column 1-3 ==> r+, c+
            for (int startC = 1; startC < 4; startC++)
            {

                lineLength = 0;
                for (int r = 0, c = startC; c < 7; c++, r++)
                {
                    if (this.Grid[r, c] == 0)
                    {
                        lineLength = 0;
                    }
                    else if (this.Grid[r, c] != playerChecking)
                    {
                        lineLength = 1;
                        playerChecking = OtherPlayer(playerChecking);
                    }
                    else
                    {
                        lineLength++;
                        if (lineLength > 3) { return playerChecking; }
                    }                   
                }
            }

            // diagnols from row 0-2 ==> r+, c+
            for (int startR = 0; startR < 3; startR++)
            {
                lineLength = 0;
                for (int c = 0, r = startR; r < 6; c++, r++)
                {
                    if (this.Grid[r, c] == 0)
                    {
                        lineLength = 0;
                    }
                    else if (this.Grid[r, c] != playerChecking)
                    {
                        lineLength = 1;
                        playerChecking = OtherPlayer(playerChecking);
                    }
                    else
                    {
                        lineLength++;
                        if (lineLength > 3) { return playerChecking; }
                    }
                }
            }

            // diagnols from column 1-3 ==> r-, c+
            for (int startC = 1; startC < 4; startC++)
            {
                lineLength = 0;
                for (int r = 5, c = startC; c < 7; c++, r--)
                {
                    if (this.Grid[r, c] == 0)
                    {
                        lineLength = 0;
                    }
                    else if (this.Grid[r, c] != playerChecking)
                    {
                        lineLength = 1;
                        playerChecking = OtherPlayer(playerChecking);
                    }
                    else
                    {
                        lineLength++;
                        if (lineLength > 3) { return playerChecking; }
                    }
                }
            }

            // diagnols from row 0-2 ==> r-, c+
            for (int startR = 3; startR < 6; startR++)
            {
                lineLength = 0;
                for (int c = 0, r = startR; r > -1; c++, r--)
                {                    
                    if (this.Grid[r, c] == 0)
                    {
                        lineLength = 0;
                    }
                    else if (this.Grid[r, c] != playerChecking)
                    {
                        lineLength = 1;
                        playerChecking = OtherPlayer(playerChecking);
                    }
                    else
                    {
                        lineLength++;
                        if (lineLength > 3) { return playerChecking; }
                    }
                }
            }



            return 0;
        }

        private int OtherPlayer(int p)
        {
            if (p == 1) { return 2; }
            else { return 1; }
        }

        public void display()
        {
            //Console.WriteLine(PieceCount + "|" + Player1Turn);
            if(LastMove != -1)
            {
                Console.Write("Computer played: " + (LastMove + 1));
            }
            Console.WriteLine();
            Console.WriteLine("-------------");
            for (int r = 5; r > -1; r--)
            {
                for (int c = 0; c < 7; c++)
                {
                    if (this.Grid[r, c] == 1)
                    {
                        Console.Write("O ");
                    }
                    else if (this.Grid[r, c] == 2)
                    {
                        Console.Write("X ");
                    }
                    else
                    {
                        Console.Write("- ");
                    }
                }
                Console.Write("\n");
            }
            Console.WriteLine("-------------");
            Console.WriteLine("1 2 3 4 5 6 7");
        }

        // returns a deep copy of the given Board
        public Board Copy()
        {
            int[,] copiedGrid = this.Grid.Clone() as int[,];
            Board deepcopyBoard = new Board(this.PieceCount,
                               this.Turn, copiedGrid, this.LastMove);

            return deepcopyBoard;
        }

        public int ValidMoveCount()
        {
            int count = 7;
            
            for(int c = 0; c < 7; c++)
            {
                if(Grid[5,c] != 0)
                {
                    count--;
                }
            }

            return count;
        }

    }
}
