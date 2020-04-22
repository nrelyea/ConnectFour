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

            //GameLoop game = new GameLoop();

            Board x = new Board(0, true, new int[6, 7]);
            x.display();

            //Board y = new Board(x.PieceCount, x.Player1Turn, x.Grid);
            Board y = new Board(0, true, x.GetGrid());
            y.AddPiece(1, 1);

            y.display();
            x.display();


            /*
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            */
        }
    }

    
}
