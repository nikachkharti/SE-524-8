using System.Net.Sockets;

namespace Lecture32
{
    internal class Program
    {
        static void Main(string[] args)
        {
        }

        #region Race Condition

        //private static int numberToIncrease = 0;
        //private static object locker = new object();

        //Thread t1 = new(Increment);
        //Thread t2 = new(Increment);
        //Thread t3 = new(Increment);

        //t1.Start();
        //t2.Start();
        //t3.Start();

        //t1.Join();
        //t2.Join();
        //t3.Join();

        //Console.WriteLine($"Actual: {numberToIncrease}"); //300000

        //private static void Increment()
        //{
        //    lock (locker)
        //    {
        //        for (int i = 0; i < 100000; i++)
        //        {
        //            numberToIncrease++;
        //        }
        //    }
        //}

        #endregion


        #region Split Array In Threads Example

        /*
            int[] ar = [1, 2, 3, 4, 5, 6, 7, 8];

            ProcessArray(ar);
            Console.WriteLine(string.Join(", ", ar));
         */

        private static void ProcessArray(int[] ar)
        {
            int mid = ar.Length / 2;

            Thread t1 = new(() => Pow(ar, 0, mid));
            Thread t2 = new(() => Pow(ar, mid, ar.Length));

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();
        }
        private static void Pow(int[] ar, int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                Thread.Sleep(1000);
                ar[i] *= 2;
            }
        }

        #endregion


        #region Multithreading Example
        /*
            var currentThread = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"{currentThread} Started");

            Thread t1 = new(() => IncrementCounter()); //11
            Thread t2 = new(() => DecrementCounter()); //12

            t1.Start();
            t1.Join();

            t2.Start();
            t2.Join();

            Console.WriteLine($"{currentThread} Finished");         
         */

        private static void DecrementCounter()
        {
            var currentThread = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"{currentThread} Started");

            for (int i = 10 - 1; i >= 0; i--)
            {
                Thread.Sleep(1000);
                Console.WriteLine(i);
            }

            Console.WriteLine($"{currentThread} Finished");
        }
        private static void IncrementCounter()
        {
            var currentThread = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"{currentThread} Started");

            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine(i);
            }

            Console.WriteLine($"{currentThread} Finished");
        }
        #endregion
    }
}
