using System.Globalization;
using System.Text;

namespace Lecture6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region STRINGBUILDER
            //StringBuilder sb = new StringBuilder();
            //sb.Append("Hello");
            //sb.Append(" ");
            //sb.Append("World");
            //sb.AppendLine();
            //sb.Insert(0, "Start");
            ////sb.Remove();
            //sb.Replace("Wo", "Mo");
            ////sb.Clear();


            //string content = sb.ToString();
            #endregion

            /*
             
            string name = "Nikoloz"; // name.Length = 7
            int age = 30;

            //string a = "Hello";
            //string b = "hello";

            Console.WriteLine($"Hello {name}"); //Hello Nikoloz
            Console.WriteLine("Hello" + " " + name); //Hello Nikoloz
            Console.WriteLine("Hello {0} {1}", name, age); //Hello Nikoloz 30

            //var result = a.Equals(b, StringComparison.OrdinalIgnoreCase);

            //var result = a.ToLower() == b.ToLower();

            Console.ReadLine();



            //string[] fruits = ["Apple", "Banana", "Orange"];
            // "Apple,Banana,Orange"

            //string joinedFruits = string.Join(", ", fruits);




            //string[] fruitsArray = fruits.Split(',', 'a');



            //text2 = text2.Replace("Javascript", "C#");


            //name = name.Substring(0,4); // Niko
            //name = name.Substring(0, name.Length);

            //int indexOfN = name.IndexOf('N');
            //int lastIndexOfN = name.LastIndexOf('N');




            //var result = name.StartsWith("Ni");
            //var result2 = name.EndsWith("zi");

            //var result = name.Contains("ni", StringComparison.OrdinalIgnoreCase);

            //string upperName = name.ToUpper();
            //string lowerName = name.ToLower();

            //name = name.TrimStart();
            //name = name.TrimEnd();
            //name = name.Trim();




            //string nameEmpty = string.Empty;
            //string nameEmpty = "";
            //string newName = new string('A', 5); !!!

            //char firstLetter = name[0];
            //char lastLetter = name[name.Length - 1];

            string x = 12.ToString();

             */


            string text = "Hello World !";  //l --> მომიძებნეთ ამ ასოს პირველივე ინდექსი
            var firstIdx = text.IndexOf('l');


        }
    }
}
