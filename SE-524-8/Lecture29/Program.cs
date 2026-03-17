using Lecture29.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Principal;

namespace Lecture29
{
    public class User
    {
        //[MyStringLength(50)]
        public string Name { get; set; }

        [MyRequired]
        public int? Age { get; set; }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            User user = new User();
            user.Name = "Test";
            user.Age = null;

            MyValidator.Validate(user);
        }
    }

}
