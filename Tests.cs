using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;

namespace ConnectFour
{
    class Tests
    {
        int[] times;

        public Tests()
        {
            times = new int[7];

            runTests();
        }

        private void runTests()
        {
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < 7; i++)
            {
                Thread t = new Thread(new ParameterizedThreadStart(Sleepy));
                t.Start(i);
                threads.Add(t);
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine("\nCOMPLETE!\n");

            PrintIntArr(times);
        }

        private void Sleepy(object obj)
        {
            Random rnd = new Random();
            int time = rnd.Next(1, 8);
            Console.WriteLine(obj + " is going to sleep for " + time + " seconds");
            Thread.Sleep(time * 1000);
            Console.WriteLine(obj + " woke up!");

            times[(int)obj] = time;
        }

        private void PrintIntArr(int[] arr)
        {
           for(int i = 0; i < arr.Length; i++)
           {
                Console.WriteLine("Thread " + i + " slept " + arr[i] + " seconds");
           }

        }

    }
}
