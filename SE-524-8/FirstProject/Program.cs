//char charVariable = 'A';
//string stringVariable = "Hello, World!";


//byte byteVariable = 0 - 255;
//sbyte sbyteVariable = -128;
//short shortVariable = 0;
//ushort ushortVariable = 0;
//int intVariable = 0;   --DEFAULT
//uint uintVariable = 0;
//long longVariable = 0;
//ulong ulongVariable = 0;


//float f = 0.0f;
//double d = 0.0; ---DEFAULT
//decimal dec = 0.0m;


//bool boolVariable = true; or false

//object objVariable = new object();




Console.WriteLine("First number :");
int firstNumber = int.Parse(Console.ReadLine()); // Prompt

Console.WriteLine("Second number :");
int secondNumber = Convert.ToInt32(Console.ReadLine()); // Prompt

int result = firstNumber + secondNumber;

Console.WriteLine(result);


Console.ReadKey();


/*
 
დავალება 1 
1.კონსოლის ფანჯრიდან შემოიყვანეთ ციფრი, და Convert.ToDouble ფუნქციით გადააკონვერტირეთ double-ში 

და შეინახეთ ცვლადში სახელად number1. 
2.კონსოლის ფანჯრიდან შემოიყვანეთ მეორე ციფრი, ესეც დააკონვერტირეთ double-ში და შეინახეთ 
ცვლადში სახელად number2. 

3.შექმენით ცვლადი sum  და შეინახეთ მასში  number1 მიმატებული number2 ანუ მათი ჯამი და დაბეჭდეთ.
 
 */