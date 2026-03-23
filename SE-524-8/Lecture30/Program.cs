using System.Text.RegularExpressions;

namespace Lecture30
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //რეგულარული გამოსახულებები [Regex -> Regular Expression]

            /*
                   .   ნებისიმერი სიმბოლო
                   \d   ციფრი (0-9)
                   \w   ასო + ციფრი
                   \s   whitespace
                   +   1 ან მეტი
                   *   0 ან მეტი
                   ^   სტრინგის დასაწყისი
                   $   სტრინგის დასასრული

                   @   გამოიყენება სტრინგის წინ, სწორი ფორმატირებისთვის

             */



            //string onlyNumbersPattern = @"^\d+$"; //მხოლოდ ციფრები
            //string input = "12345";
            //bool isMatch = Regex.IsMatch(input, onlyNumbersPattern);




            //string pattern = @"\d";
            //string input = "My number is 72345";
            //Match match = Regex.Match(input, pattern);
            //Console.WriteLine(match.Value);



            //string pattern = @"\d+";
            //string input = "My number is 72345 10 20 30";
            //MatchCollection matches = Regex.Matches(input, pattern);
            //foreach (Match item in matches)
            //{
            //    Console.WriteLine(item.Value);
            //}



            //string input = "Hello 123 World";
            //string result = Regex.Replace(input, @"\d+", "***");



            //string onlyAlphabetPattern = @"^[a-zA-Z\s]+$";
            //string input = "Hello World";
            //var result = Regex.IsMatch(input, onlyAlphabetPattern);


            //string input = "+995 558 490 645";
            //string geoMobilePattern = @"^(\+995\s?)5\d{2}(\s?\d{3}){2,}$";
            //var result = Regex.IsMatch(input, geoMobilePattern);



            string validEmailPattern = @"";



        }
    }
}
