using Lecture28.DependencyInversion;
using Lecture28.InterfaceSegregation;
using Lecture28.LiskovSubtitiution;
using System.Drawing;
using Rectangle = Lecture28.LiskovSubtitiution.Rectangle;

namespace Lecture28
{
    //Sinle Responsibility -- კოდის ერთმა ნაწილმა უნდა აკეთოს მხოლოდ ერთი, კონკრეტული საქმე



    //Open Closed Principle -- Software entities should be open for extension, but closed for modification. კოდი ღია უნდა იყოს გაფართოებისთვის მაგრამ დახურული ცვლილებისთვის




    //Liskov Subtitution Principle -- Objects of a base class should be replaceable with objects of a derived class without breaking the program. -- მშობელი კლასის ობიექტები უნდა იყვნენ ჩანაცვლებადი მემკვიდრე ობიექტების მიერ ისე რომ არ დაირღვეს პროგრამის მუშაობა



    //Interface Segragation Principle -- შვილი არ უნდა დაავალდებულო რომ დააიმპლემენტიროს ის ინტერფეისი რომელსაც არ იყენებს




    //Dependency Inversion Principle --
    //1. High-level modules should not depend on low-level modules. Both should depend on abstractions.
    //2. Abstractions should not depend on details. Details should depend on abstractions.
    //ყველაფერი უნდა იყოს დამოკიდებული აბსტრაქტულ ტიპებზე

    internal class Program
    {
        static void Main(string[] args)
        {
            //IShape shape1 = new Square() { Side = 5 };
            //IShape shape2 = new Rectangle() { Width = 5, Height = 5 };

            //Console.WriteLine(shape1.GetArea());
            //Console.WriteLine(shape2.GetArea());

            Notification notification = new(new EmailService());
            notification.Send("I am nika");

        }

    }
}
