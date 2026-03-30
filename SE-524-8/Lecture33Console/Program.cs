namespace Lecture33Console
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            int result = await Task.Run(() => Sum(10, 10));
            Console.WriteLine(result);
            Test();


            //int finalResult = workResult.Result; // !!!!!!!!!!! არასდორს არ დაწეროთ ესე კოდი !!!!!!!!!!!
            //workResult.Wait(); // !!!!!!!!!!! არასდორს არ დაწეროთ ესე კოდი !!!!!!!!!!!
            //Task.WaitAll(workResult); // !!!!!!!!!!! არასდორს არ დაწეროთ ესე კოდი !!!!!!!!!!!
            //Task.WaitAny(workResult); // !!!!!!!!!!! არასდორს არ დაწეროთ ესე კოდი !!!!!!!!!!!


            //Thread t1 = new(() => Sum());
        }


        private static void Test()
        {
            Console.WriteLine("Hello World");
        }

        private static int Sum(int x, int y)
        {
            Thread.Sleep(10000);
            return x + y;
        }

    }
}
