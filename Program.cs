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

            GameLoop game = new GameLoop();

            //int meme = 19 / 10;
            //Console.WriteLine(meme);


            /*
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            */
        }
    }

    
}
