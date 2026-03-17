using MiniBank.Repository.Models.Enums;

namespace MiniBank.Repository.Models
{
    public class Customer
    {
        //სავალდებულო
        //დადებითი
        public int Id { get; set; }

        //სავალდებულო
        //მაქს ზომა 50
        public string Name { get; set; }

        //სავალდებულო
        //ზუტად ზომაში 11
        public string IdentityNumber { get; set; }

        //სავალდებულო
        //ზუტად ზომაში 9
        public string PhoneNumber { get; set; }

        //სავალდებულო
        //ჯდებოდეს E-mail ფორმატში
        public string Email { get; set; }
        public CustomerType CustomerType { get; set; }
    }
}
