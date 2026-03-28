namespace Lecture33Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("MAIN START");

            Task<int> workResult = Task.Run(() => DoWork(10, 10));

            //int finalResult = workResult.Result; // !!!!!!!!!!! არასდორს არ დაწეროთ ესე კოდი !!!!!!!!!!!
            //workResult.Wait(); // !!!!!!!!!!! არასდორს არ დაწეროთ ესე კოდი !!!!!!!!!!!
            //Task.WaitAll(workResult); // !!!!!!!!!!! არასდორს არ დაწეროთ ესე კოდი !!!!!!!!!!!
            //Task.WaitAny(workResult); // !!!!!!!!!!! არასდორს არ დაწეროთ ესე კოდი !!!!!!!!!!!
            


            Console.WriteLine("MAIN END");

            //Thread t1 = new(() => DoWork());
        }

        private static int DoWork(int x, int y)
        {
            Task.Delay(8000);
            return x + y;
        }

    }
}
