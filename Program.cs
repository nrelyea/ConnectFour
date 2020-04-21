using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectFour
{
    static class Program
    {
        [STAThread]
        static void Main()
        {

            Board brd = new Board();
            brd.AddPiece(1, 1);
            brd.AddPiece(1, 1);
            brd.AddPiece(1, 1);
            brd.AddPiece(1, 2);
            brd.AddPiece(1, 2);
            brd.AddPiece(1, 3);

            brd.AddPiece(2, 1);
            brd.AddPiece(2, 2);
            brd.AddPiece(2, 3);
            brd.AddPiece(2, 4);



            brd.display();

            Console.WriteLine("Winner: " + brd.Winner());





            /*
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            */
        }
    }

    
}
