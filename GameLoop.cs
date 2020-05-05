using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ConnectFour
{
    class GameLoop
    {
        // public variable to be used by FasterMiniMaxBot+
        int[] strengths;
        public GameLoop()
        {
            strengths = new int[7];
            Array.Fill(strengths, -1);

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

                    //brd = Random_Bot(brd);
                    //brd = Easy_Bot(brd);
                    //brd = MiniMax_Bot(brd, 6);
                    brd = FasterMiniMax_Bot(brd, 7);
                    //brd = BetterMiniMax_Bot(brd);

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
        private Board Random_Bot(Board b)
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
        private Board Easy_Bot(Board b)
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
            return Random_Bot(b);
        }

        // uses a minimax algorithm to search for the strongest move
        private Board MiniMax_Bot(Board b, int depth)
        {
            Console.WriteLine("thinking...");

            int bestWinningMove = -1;
            int bestWinningMoveStrength = 0;

            int bestAlternateMove = -1;
            int bestAlternateMoveStrength = 1000;

            for (int move = 0; move < 7; move++)
            {
                int strength = moveStrength(b, move, depth);
                if (strength != -1)
                {
                    Console.WriteLine("Strength of move " + move + ": " + strength);

                    if((strength / 100) == 2 && strength > bestWinningMoveStrength)
                    {
                        bestWinningMove = move;
                        bestWinningMoveStrength = strength;
                    }
                    else if (strength == 0 || ((strength / 100) == 1 && strength < bestAlternateMoveStrength))
                    {
                        bestAlternateMove = move;
                        bestAlternateMoveStrength = strength;
                    }                    
                    
                }
            }

            if(bestWinningMove != -1)   // if there is a winning move to play, play the strongest one
            {
                //Console.WriteLine("\npursuing winning move: " + bestWinningMove);
                b.AddPiece(2, bestWinningMove);
                return (b);
            }
            else                        // if there is no winning move, play the optimal alternate one
            {
                //Console.WriteLine("\npursuing optimal alternate move: " + bestAlternateMove + " (strength " + bestAlternateMoveStrength + ")");
                b.AddPiece(2, bestAlternateMove);
                return (b);
            }
        }

        // Same minimax algorithm but optimized with multithreading
        private Board FasterMiniMax_Bot(Board b, int depth)
        {
            Array.Fill(strengths, -1);

            Console.WriteLine("\nthinking...\n");

            int bestWinningMove = -1;
            int bestWinningMoveStrength = 0;

            int bestAlternateMove = -1;
            int bestAlternateMoveStrength = 1000;

            depth += (7 - b.ValidMoveCount());

            Console.WriteLine("Searching at depth " + depth + "...");

            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < 7; i++)
            {
                Thread t = new Thread(new ParameterizedThreadStart(MeasureStrength));
                MoveData data = new MoveData(b, i, depth);
                t.Start(data);
                threads.Add(t);
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }

            for (int move = 0; move < 7; move++)
            {
                //int strength = moveStrength(b, move, depth);
                if (strengths[move] != -1)
                {
                    Console.WriteLine("Strength of move " + move + ": " + strengths[move]);

                    if ((strengths[move] / 100) == 2 && strengths[move] > bestWinningMoveStrength)
                    {
                        bestWinningMove = move;
                        bestWinningMoveStrength = strengths[move];
                    }
                    else if (strengths[move] == 0 || ((strengths[move] / 100) == 1 && strengths[move] < bestAlternateMoveStrength))
                    {
                        bestAlternateMove = move;
                        bestAlternateMoveStrength = strengths[move];
                    }

                }
            }

            if (bestWinningMove != -1)   // if there is a winning move to play, play the strongest one
            {
                //Console.WriteLine("\npursuing winning move: " + bestWinningMove);
                b.AddPiece(2, bestWinningMove);
                return (b);
            }
            else                        // if there is no winning move, play the optimal alternate one
            {
                //Console.WriteLine("\npursuing optimal alternate move: " + bestAlternateMove + " (strength " + bestAlternateMoveStrength + ")");
                b.AddPiece(2, bestAlternateMove);
                return (b);
            }
        }

        // uses a more intuitive approach in combination with minimax algorithm to search for the strongest move
        private Board BetterMiniMax_Bot(Board b)
        {
            Console.WriteLine("thinking...");

            int bestWinningMove = -1;
            int bestWinningMoveStrength = 0;

            int bestAlternateMove = -1;
            int bestAlternateMoveStrength = 1000;

            List<int> tieMoves = new List<int> { };

            for (int move = 0; move < 7; move++)
            {
                // hardcoded depth of 6 move lookahead
                int strength = moveStrength(b, move, 6);
                if (strength != -1)
                {
                    //Console.WriteLine("Strength of move " + move + ": " + strength);

                    if ((strength / 100) == 2 && strength > bestWinningMoveStrength)
                    {
                        bestWinningMove = move;
                        bestWinningMoveStrength = strength;
                    }
                    else if (strength == 0 || ((strength / 100) == 1 && strength < bestAlternateMoveStrength))
                    {
                        bestAlternateMove = move;
                        bestAlternateMoveStrength = strength;
                        if(strength == 0)
                        {
                            tieMoves.Add(move);
                        }
                    }

                }
            }

            if (bestWinningMove != -1)   // if there is a winning move to play, play the strongest one
            {
                Console.WriteLine("Pursuing CPU win");
                b.AddPiece(2, bestWinningMove);
                return (b);
            }
            else if (bestAlternateMoveStrength > 0)    // if all moves are losing, play the optimal LEAST WEAK move
            {
                Console.WriteLine("Pursuing optimal losing move");
                b.AddPiece(2, bestAlternateMove);
                return (b);
            }


            ////////////////////////////////////////////////////////////////////////////////
            // ----- Otherwise, some moves suggest tie, so play more intuitively... ----- //
            ////////////////////////////////////////////////////////////////////////////////


            Console.WriteLine("moves to consider:");
            printIntList(tieMoves);

            // if only one move is not losing, play it
            if(tieMoves.Count == 1)
            {
                b.AddPiece(2, tieMoves[0]);
                return (b);
            }

            // Otherwise, determine which of tying moves is optimal

            // ideal columns to have pieces in, ordered from most ideal to least ideal
            int[] idealColumns = { 3, 2, 4, 1, 5, 0, 6 };

            for(int i = 0; i < idealColumns.Length; i++)
            {
                if (tieMoves.Contains(idealColumns[i]))
                {
                    


                }
            }

            // placeholder, just play first available tie move
            b.AddPiece(2, tieMoves[0]);
            return (b);
        }
        
        // helper for minimax alg, recursively determines move strength
        private int moveStrength(Board b, int move, int depth)
        {
            int currentPlayer = b.Turn;
            Board copy = (Board)b.Copy();
            if (copy.AddPiece(copy.Turn, move))
            {
                // if this move would spell an immediate win for either player
                int winner = copy.Winner();
                if(winner != 0)
                {
                    // return the id of the current player * 10, with depth added
                    return (currentPlayer * 100) + depth;
                    //return (currentPlayer * 10);
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
                int strongestMoveStrength = 0;
                int bestBadMoveStrength = 0;

                for(int c = 0; c < 7; c++)
                {
                    int nextMoveStrength = moveStrength(copy, c, depth - 1);
                    // if the next move will ultimately result in a loss for currentPlayer, mark it as such
                    if((nextMoveStrength / 100) == copy.Turn && nextMoveStrength > strongestMoveStrength)
                    {
                        strongestMoveStrength = nextMoveStrength;
                    }
                    // if at least one of the next moves will continue towards a potential victory for currentPlayer
                    else if(nextMoveStrength == 0)
                    {
                        tieAvailable = true;
                    }
                    else if(nextMoveStrength > bestBadMoveStrength)
                    {
                        bestBadMoveStrength = nextMoveStrength;
                    }
                }
                if(strongestMoveStrength > 0)
                {
                    return strongestMoveStrength;
                }
                else if (tieAvailable)
                {
                    return 0;
                }
                else
                {
                    return bestBadMoveStrength;
                }
            }
        }

        private void MeasureStrength(object md)
        {
            MoveData data = (MoveData)md;

            strengths[data.Move] = moveStrength(data.Board, data.Move, data.Depth);

        }

        private void printIntList(List<int> lst)
        {
            Console.Write("[");
            for(int i = 0; i < lst.Count; i++)
            {
                if(i != 0)
                {
                    Console.Write(",");
                }
                Console.Write(" " + lst[i]);
            }
            Console.WriteLine(" ]");
        }
    }

    class MoveData
    {
        public Board Board;
        public int Move;
        public int Depth;

        public MoveData(Board b, int m, int d)
        {
            Board = b;
            Move = m;
            Depth = d;
        }
    }
}
