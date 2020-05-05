using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectFour
{
    class Program
    {

        [STAThread]
        static void Main()
        {

            GameLoop game = new GameLoop();

            //Tests test = new Tests();



        }


        static void HashTableEx()
        {
            
            Hashtable ht = new Hashtable();

            int meme = 9;
            string msg = "ayy lmao";
            List<int> lst = new List<int> { 11, 22, 33, 44 };

            ht.Add("first", meme);
            ht.Add("second", msg);
            ht.Add("third", lst);

            try
            {
                ht.Add("first", "hehe");
            }
            catch
            {
                var retrieved = ht["third"];

                if(retrieved is int || retrieved is string)
                {
                    Console.WriteLine(retrieved);
                }
                else
                {
                    Console.WriteLine(retrieved + " " + ((List<int>)retrieved)[0]);
                }
            }
            
        }
    }

    
}
