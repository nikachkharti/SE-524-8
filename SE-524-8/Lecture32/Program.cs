namespace Lecture32
{
    internal class Program
    {
        static void Main(string[] args)
        {


        }



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
