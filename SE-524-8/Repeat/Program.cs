using System.Diagnostics;
namespace Repeat
{
    public static class Program
    {
        private static int _counter = 0;
        //private static object _counterLocker = new();
        private static SemaphoreSlim _counterSemaphore = new(1, 1);

        static async Task Main(string[] args)
        {
            Stopwatch sw = Stopwatch.StartNew();

            #region AWAIT VS CONTINUEWITH
            //var result1 = await GetDataAsync("API 1");
            //var result2 = await GetDataAsync("API 2");
            //var result3 = await GetDataAsync("API 3");

            //GetDataAsync("API 1")
            //    .ContinueWith(t1 =>
            //    {
            //        var result1 = t1.Result;

            //        return GetDataAsync("API 2")
            //            .ContinueWith(t2 =>
            //            {
            //                var result2 = t2.Result;

            //                return GetDataAsync("API 3")
            //                    .ContinueWith(t3 =>
            //                    {
            //                        var result3 = t3.Result;

            //                        // Use results here
            //                        Console.WriteLine(result1);
            //                        Console.WriteLine(result2);
            //                        Console.WriteLine(result3);
            //                    });
            //            }).Unwrap();
            //    }).Unwrap(); 
            #endregion


            #region TASK WHENALL
            //Task<string> task1 = GetDataAsync("API 1");
            //Task<string> task2 = GetDataAsync("API 2");
            //Task<string> task3 = GetDataAsync("API 3");

            //string[] results = await Task.WhenAll(task1, task2, task3); 
            #endregion


            Task<string> task1 = GetDataAsync("API 1");
            Task<string> task2 = GetDataAsync("API 2");
            Task<string> task3 = GetDataAsync("API 3");

            string[] results = await Task.WhenAll(task1, task2, task3);

            Console.WriteLine($"Counter: {_counter}");

            sw.Stop();

            foreach (var result in results)
                Console.WriteLine(result);

            Console.WriteLine($"Total time: {sw.ElapsedMilliseconds} ms");
        }


        static async Task<string> GetDataAsync(string apiName)
        {
            await Task.Delay(2000); // Simulate an slow operation

            for (int i = 0; i < 100000; i++)
            {
                #region LOCK
                //lock (_counterLocker)
                //{
                //_counter++;
                //} 
                #endregion


                #region SEMAPHORE

                await _counterSemaphore.WaitAsync();

                try
                {
                    _counter++;
                }
                finally
                {
                    _counterSemaphore.Release();
                }

                #endregion

            }

            return $"[200] OK Response from service: {apiName}";
        }



        #region გამეორება 1
        public static int RomanToInt(string s)
        {
            var romanPairs = new Dictionary<char, int>
            {
                { 'I', 1 },
                { 'V', 5 },
                { 'X', 10 },
                { 'L', 50 },
                { 'C', 100 },
                { 'D', 500 },
                { 'M', 1000 }
            };

            int result = 0;

            for (int i = 0; i < s.Length; i++)
            {
                int currentValue = romanPairs[s[i]];

                int nextValue = 0;
                if (i + 1 < s.Length)
                {
                    nextValue = romanPairs[s[i + 1]];
                }

                if (currentValue < nextValue)
                    result -= currentValue;
                else
                    result += currentValue;
            }

            return result;
        }
        public static int FirstUniqueCharacter(string text)  //O(n) time complexity
        {
            Dictionary<char, int> frequency = new(); //'H' 0

            //1. Count the frequency of each character in the string
            foreach (var item in text)
            {
                if (frequency.ContainsKey(item))
                    frequency[item]++;
                else
                    frequency[item] = 1;
            }

            //2. Find the first character that has a frequency of 1
            foreach (var item in text)
            {
                if (frequency[item] == 1)
                    return text.IndexOf(item);
            }

            return -1;
        }

        //public static char FirstUniqueCharacter(string text)  O(n^2) time complexity
        //{
        //    for (int i = 0; i < text.Length; i++)
        //    {
        //        bool isUnique = true;

        //        for (int j = 0; j < text.Length; j++)
        //        {
        //            if (i != j && text[i] == text[j])
        //            {
        //                isUnique = false;
        //                break;
        //            }
        //        }

        //        if (isUnique)
        //            return text[i];
        //    }

        //    return default;
        //}

        public static (int age, string name) GetName() //Tuple return type
        {
            return (12, "John");
        }

        public static void IncrementMethod(ref int number)
        {
            number += 1;
        }

        public static void IncrementMethod(ref float number)
        {
            number = 0; // Must assign a value before using an out parameter
            number += 1;
        }
        #endregion
    }
}
