using System.Security.Cryptography;

namespace Lecture18
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Hashset - უნიკალურ ელემენტებს იმახსოვრებს
            //Stack - LIFO ბოლოს დამახსოვრებული, ამოვარდება პირველი
            //Queue - FIFO რიგითობის დაცვის პრინციპით
            //List, Array []
            //Dictionary []

            //Dictionary<int, string> dictionary = new Dictionary<int, string>()
            //{
            //    { 1, "Erti" },
            //    { 2, "Ori" },
            //    { 3, "Sami" },
            //    { 4, "Otxi" }
            //};


            //dictionary.Add(6, "Eqvsi");
            //dictionary.Remove(6);
            //bool result = dictionary.ContainsKey(5);
            //bool result = dictionary.ContainsValue("Ori");
            //var result = dictionary.GetValueOrDefault(7);
            //dictionary.TryGetValue(6, out var result);
            //dictionary.Clear();
            //Console.WriteLine(dictionary.Count());

            //Dictionary<int, string>.KeyCollection keys = dictionary.Keys;
            //Dictionary<int, string>.ValueCollection values = dictionary.Values;

            //Dictionary<string, decimal> userBalances = new Dictionary<string, decimal>();
            //userBalances.Add("Ana", 200);

            //userBalances["Ana"] += 400;



            //LinkedList<string> names = new LinkedList<string>();
            //names.AddFirst("Elene");
            //names.AddLast("Zura");
            //names.AddBefore(names.Find("Zura"), "Tamari");
            //names.AddAfter(names.Find("Elene"), "Giorgi");
            //names.Remove("Giorgi");
            //names.RemoveFirst();
            //names.RemoveLast();
            //names.First();
            //names.Last();
            //names.Count();

            //                     List vs LinkedList
            // ინდექსით წვდომა     O(1)    O(n)
            // შუაში ჩასმა           ბანძი    მაგარი
            // ზომა                 მაგარი    ბანძი
            // Ranom Access         მაგარი    ბანძი

            //debit card           bad credit
            //Tom Marvolo Riddle   I am Lord Voldemort


        }


        public static bool AreAnagrams(string word1, string word2)
        {
            string normalizedWord1 = word1.Trim().ToLower().Replace(" ", string.Empty);
            string normalizedWord2 = word2.Trim().ToLower().Replace(" ", string.Empty);

            if (normalizedWord1.Length != normalizedWord2.Length)
                return false;

            Dictionary<char, int> result = new();

            //normalizedWord1 დათვლა
            for (int i = 0; i < normalizedWord1.Length; i++)
            {
                if (result.ContainsKey(normalizedWord1[i]))
                    result[normalizedWord1[i]]++;
                else
                    result.Add(normalizedWord1[i], 1);
            }

            //normalizedWord2 გამოკლება
            for (int i = 0; i < normalizedWord2.Length; i++)
            {
                if (!result.ContainsKey(normalizedWord2[i]))
                    return false;

                result[normalizedWord2[i]]--;

                if (result[normalizedWord2[i]] < 0)
                    return false;
            }

            return true;
        }



    }
}
