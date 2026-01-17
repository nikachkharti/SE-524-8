namespace Lecture4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Loop
            //for while do while foreach

            //for (int i = 1; i <= 100; i++)
            //{
            //    Console.WriteLine(i);
            //}

            //foreach (var item in args)
            //{
            //    Console.WriteLine(item);
            //}

            //int i = 1;
            //while (i <= 100)
            //{
            //    Console.WriteLine(i);
            //    i++;
            //}

            //int i = 1;
            //do
            //{
            //    Console.WriteLine(i);
            //    i++;
            //}
            //while (i <= 100);


            //for (int i = 1; i <= 100; i++)
            //{
            //    Console.WriteLine(i);
            //}

            //for (int i = 10; i >= 1; i--)
            //{
            //    if (i == 7)
            //    {
            //        //break; ციკლის გაწყვეტა ხელოვნურად
            //        //continue; ციკლის ამოჭრა კონკრეტულ იტერაციაზე
            //    }

            //    Console.WriteLine(i);
            //}


            //for (int i = 1; i <= 9; i++)
            //{
            //    for (int j = 1; j <= 9; j++)
            //    {
            //        Console.WriteLine($"{i} * {j} = {i * j}");
            //    }
            //}


            //for (int i = 1; i <= 5; i++)
            //{
            //    for (int j = 1; j <= i; j++)
            //    {
            //        Console.Write($"*");
            //    }
            //    Console.WriteLine();
            //}


            //for (int i = 5; i >= 1; i--)
            //{
            //    for (int j = 1; j <= i; j++)
            //    {
            //        Console.Write("*");
            //    }
            //    Console.WriteLine();
            //} 

            //int[] numbers = [10, 20, 30];

            //for (int i = 0; i < numbers.Length; i++)
            //{
            //    Console.WriteLine(numbers[i]);
            //}

            //foreach (int number in numbers)
            //{
            //    Console.WriteLine(number);
            //}




            #endregion


            //მასივი ანუ Array

            //int[] numbers = [10, 20, 30];

            //double[] doubleNumbers = new double[5];
            //double[] doubleNumbers = { 1.5, 2.5, 3.5, 4.5, 5.5 };

            //string[] names = ["John", "Jane", "David"]; //0 ,1, 2
            //names[1] = "Nika";

            //for (int i = 0; i < names.Length; i++)
            //{
            //    Console.WriteLine(names[i]);
            //}


            Random random = new Random(); // შემთხვევითი რიცხვის გენერატორი
            var randomNumber = random.Next(1, 7); // 1-დან 6-მდე შემთხვევითი რიცხვი




        }
    }
}
