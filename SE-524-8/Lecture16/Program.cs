using System.Collections;

namespace Lecture16
{

    public class MyClass : ICollection
    {
        public int Count { get; }
        public bool IsSynchronized { get; }
        public object SyncRoot { get; }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            List<int> ints = new List<int>() { 10, 1, 2, 44, 123, 5, 41, 41, 41, 2, 31 };
            //List<double> doubles = new() { 1.1, 2.33, 12.22, 41, 41 };
            //List<float> floats = new() { 1.1f, 2.3f, 12.22f, 41, 41 };

            //List<int> similarInts = new List<int>() { 1, 1, 15 };
            //List<string> numberAsStrings = new List<string>() { "1", "1", "15" };
            string[] ar = ["1", "2", "3", "2", "4", "1", "-1", "2"];
            List<string> names = new List<string>() { "Ana", "Giorgi", "Ana", "Daviti", "Nikoloz", "Elene", "Nikoloz" };


            var r = Algorithms.NikasSelect(ar, IntConverter);

        }

        private static int IntConverter(string input)
        {
            int.TryParse(input, out var result);
            return result;
        }

        private static bool IsAna(string name) => name == "Ana";
        private static bool IsEven(int number) => number % 2 == 0;
        private static bool IsOdd(int number) => number % 2 != 0;
        private static bool IsPositive(int number) => number > 0;
        private static bool IsNegative(int number) => number < 0;

    }
}
