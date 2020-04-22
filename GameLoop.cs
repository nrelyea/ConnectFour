using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectFour
{
    class GameLoop
    {
        public GameLoop()
        {
            Board brd = new Board(0, true, new int[6, 7]);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Pieces: " + brd.PieceCount);
                brd.display();


                Console.WriteLine("Your move: ");
                int move = 0;
                bool inputIsInt = int.TryParse(Console.ReadLine(), out move);
                if (inputIsInt && move > 0 && move < 8 && brd.AddPiece(1, move - 1))
                {
                    if (brd.Winner() != 0) { break; }

                    // DECIDE HOW PLAYER 2 WILL PLAY

                    brd = RandomBot(brd);




                    if (brd.Winner() != 0 || brd.PieceCount >= 42) { break; }
                }
            }

            Console.Clear();
            brd.display();

            if (brd.Winner() != 0)
            {
                Console.WriteLine("Winner: Player " + brd.Winner() + "!");
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

        // stops immediate lines of 3 if it can
        private Board EasyBot(Board b)
        {
            
            
            return b;
        }

       
    }
}
