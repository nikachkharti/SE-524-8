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


            //Random random = new Random(); // შემთხვევითი რიცხვის გენერატორი
            //var randomNumber = random.Next(1, 7); // 1-დან 6-მდე შემთხვევითი რიცხვი



            #region DICE GAME
            const int rollsCount = 10;

            int[] player1Rolls = new int[rollsCount];
            int[] player2Rolls = new int[rollsCount];

            int player1Score = 0;
            int player2Score = 0;

            Random random = new Random();

            Console.WriteLine("=== Dice Game Started ===\n");

            for (int i = 0; i < rollsCount; i++)
            {
                // Player 1
                int roll1 = random.Next(1, 7);
                player1Rolls[i] = roll1;
                int score1 = roll1 == 6 ? 10 : roll1;
                player1Score += score1;

                // Player 2
                int roll2 = random.Next(1, 7);
                player2Rolls[i] = roll2;
                int score2 = roll2 == 6 ? 10 : roll2;
                player2Score += score2;

                // თითოეული რაუნდის შედეგი
                Console.WriteLine(
                    $"Round {i + 1}: " +
                    $"Player 1 rolled {roll1} (+{score1}), " +
                    $"Player 2 rolled {roll2} (+{score2})"
                );
            }

            Console.WriteLine("\n=== Rolls ===");

            Console.Write("Player 1 rolls: ");
            for (int i = 0; i < rollsCount; i++)
            {
                Console.Write(player1Rolls[i] + " ");
            }

            Console.WriteLine();

            Console.Write("Player 2 rolls: ");
            for (int i = 0; i < rollsCount; i++)
            {
                Console.Write(player2Rolls[i] + " ");
            }

            Console.WriteLine("\n\n=== Final Scores ===");
            Console.WriteLine($"Player 1 total score: {player1Score}");
            Console.WriteLine($"Player 2 total score: {player2Score}");

            Console.WriteLine("\n=== Result ===");
            if (player1Score > player2Score)
            {
                Console.WriteLine("Winner: Player 1");
                Console.WriteLine("Loser: Player 2");
            }
            else if (player2Score > player1Score)
            {
                Console.WriteLine("Winner: Player 2");
                Console.WriteLine("Loser: Player 1");
            }
            else
            {
                Console.WriteLine("Result: Draw");
            }
            #endregion






        }
    }
}
