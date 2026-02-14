using System.Collections;

namespace Lecture16
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<int> ints = new List<int>() { 10, 1, 2, 44, 123, 5, 41, 41, 41, 2, 31 };
            //List<double> doubles = new() { 1.1, 2.33, 12.22, 41, 41 };
            //List<float> floats = new() { 1.1f, 2.3f, 12.22f, 41, 41 };

            //List<int> similarInts = new List<int>() { 1, 1, 15 };
            //List<string> numberAsStrings = new List<string>() { "1", "1", "15" };
            List<string> names = new List<string>() { "Giorgi", "Ana", "Daviti", "Nikoloz", "Elene", "Nikoloz" };

            var x = Algorithms.Sort(names);


        }
    }
}
