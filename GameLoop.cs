﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectFour
{
    class GameLoop
    {
        public GameLoop()
        {
            Board brd = new Board(0, 1, new int[6, 7], -1);

            while (true)
            {
                Console.Clear();
                brd.display();


                Console.WriteLine("Your move: ");
                int move = 0;
                bool inputIsInt = int.TryParse(Console.ReadLine(), out move);
                if (inputIsInt && move > 0 && move < 8 && brd.AddPiece(1, move - 1))
                {
                    if (brd.Winner() != 0) { break; }

                    // DECIDE HOW PLAYER 2 WILL PLAY

                    //brd = RandomBot(brd);
                    //brd = EasyBot(brd);
                    brd = MiniMaxBot(brd, 7);

                    //Console.Read();


                    if (brd.Winner() != 0 || brd.PieceCount >= 42) { break; }
                }
            }

            Console.Clear();
            brd.display();

            if (brd.Winner() == 1)
            {
                Console.WriteLine("You won! Congratulations!");
            }
            else if (brd.Winner() == 2)
            {
                Console.WriteLine("You lost! Better luck next time!");
            }
            else
            {
                Console.WriteLine("Tie!");
            }

        }

        // plays randomly
        private Board RandomBot(Board b)
        {
            while (true)
            {
                Random rnd = new Random();
                if (b.AddPiece(2, rnd.Next(0, 7)))
                {
                    break;
                }
            }

            return b;
        }

        // looks for immediate win or loss, and moves to win or prevent loss
        private Board EasyBot(Board b)
        {
            // see if there is an immediate win available
            for(int c = 0; c < 7; c++)
            {
                Board copy1 = (Board)b.Copy();

                if (copy1.AddPiece(2, c) && copy1.Winner() == 2)
                {
                    b.AddPiece(2, c);
                    return b;
                }               
            }

            // see if there is an immediate threat needed to block
            for (int c = 0; c < 7; c++)
            {
                Board copy2 = (Board)b.Copy();

                if (copy2.AddPiece(1, c) && copy2.Winner() == 1)
                {
                    b.AddPiece(2, c);
                    return b;
                }
            }

            // otherwise, just pick a random move
            return RandomBot(b);
        }

        // uses a minimax algorithm to search for a strong move
        private Board MiniMaxBot(Board b, int depth)
        {
            Console.WriteLine("thinking...");
            int tieMove = -1;
            for(int move = 0; move < 7; move++)
            {
                int strength = moveStrength(b, move, depth);
                //Console.WriteLine("strength of move " + move + ": " + strength);
                if (strength != -1)
                {
                    // if a winning move is available, play that move
                    if(strength == 2)
                    {
                        b.AddPiece(2, move);
                        return (b);
                    }
                    else if(strength == 0)
                    {
                        tieMove = move;
                    }
                }
            }

            // if a tying move is available, play that move
            if(tieMove != -1)
            {
                b.AddPiece(2, tieMove);
                return (b);
            }
            // otherwise, move randomly, as a loss is inevitable
            else
            {
                //Console.WriteLine("fuck im gonna lose");
                //Console.Read();
                return (EasyBot(b));
            }

            /*
            Console.WriteLine("Col 0: " + moveStrength(b, 0, 1));
            Console.WriteLine("Col 1: " + moveStrength(b, 1, 1));
            Console.WriteLine("Col 2: " + moveStrength(b, 2, 1));
            Console.WriteLine("Col 3: " + moveStrength(b, 3, 1));
            Console.WriteLine("Col 4: " + moveStrength(b, 4, 1));
            Console.WriteLine("Col 5: " + moveStrength(b, 5, 1));
            Console.WriteLine("Col 6: " + moveStrength(b, 6, 1));

            return EasyBot(b);
            */
        }

        private int moveStrength(Board b, int move, int depth)
        {
            /*
            int potentialCurrentWin = b.Winner();
            if (potentialCurrentWin != 0)
            {
                return potentialCurrentWin;
            }
            */

            int currentPlayer = b.Turn;
            Board copy = (Board)b.Copy();
            if (copy.AddPiece(copy.Turn, move))
            {
                // if this move would spell an immediate win for either player
                int winner = copy.Winner();
                if(winner != 0)
                {                                     
                    // return the id of the current player
                    return currentPlayer;
                }
            }
            // if the move is illegal, return -1
            else
            {
                return -1;
            }

            // base case: if depth has hit 0 (and no immediate win after this move established by previous step)
            if(depth == 0)
            {
                return 0;
            }
            // otherwise, begin searching further through moves
            else
            {
                bool tieAvailable = false;

                for(int c = 0; c < 7; c++)
                {
                    int nextMoveStrength = moveStrength(copy, c, depth - 1);
                    //Console.WriteLine("D: " + depth + " | strength of potential player " + copy.Turn + " move @ column " + c + ": " + nextMoveStrength);
                    // if the next move will ultimately result in a loss for currentPlayer, mark it as such
                    if(nextMoveStrength == copy.Turn)
                    {
                        //Console.WriteLine("D: " + depth + " | determined: winning for " + copy.Turn);
                        return copy.Turn;
                    }
                    // if at least one of the next moves will continue towards a potential victory for currentPlayer
                    else if(nextMoveStrength == 0)
                    {
                        tieAvailable = true;
                    }
                }

                if (tieAvailable)
                {
                    //Console.WriteLine("D: " + depth + " | determined: tie");
                    return 0;
                }
                else
                {
                    //Console.WriteLine("D: " + depth + " | determined: winning for " + currentPlayer);
                    return currentPlayer;
                }
            }





            return 0;
        }

    }
}
