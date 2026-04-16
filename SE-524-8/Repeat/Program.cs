namespace Repeat
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var x = RomanToInt("IX");
        }



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
    }
}
