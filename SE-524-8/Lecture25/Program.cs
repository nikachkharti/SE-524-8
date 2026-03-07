using System.Diagnostics;
using System.Text;

namespace Lecture25
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath1 = "../../../test_filestream.txt";
            string filePath2 = "../../../test_streamwriter.txt";
            string filePath3 = "../../../test_fileclass.txt";

            string text = "Hello World! This is a text for performance testing.\n";

            // დიდი ტექსტი
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 1000000; i++)
            {
                sb.AppendLine(text);
            }
            string bigText = sb.ToString();

            // =======================
            // TEST 1: FileStream
            // =======================
            Stopwatch sw1 = FileStreamIO(filePath1, bigText);


            // =======================
            // TEST 2: StreamWriter / StreamReader
            // =======================
            Stopwatch sw2 = StreamReaderAndWriterIO(filePath2, bigText);


            // =======================
            // TEST 3: File class (static methods)
            // =======================
            Stopwatch sw3 = FIleClassIO(filePath3, bigText);


            // =======================
            // შედარება
            // =======================
            Console.WriteLine("\n=== Performance Comparison Summary ===");
            Console.WriteLine($"FileStream: {sw1.ElapsedMilliseconds} ms");
            Console.WriteLine($"StreamWriter/Reader: {sw2.ElapsedMilliseconds} ms");
            Console.WriteLine($"File Class: {sw3.ElapsedMilliseconds} ms");

            long minTime = Math.Min(sw1.ElapsedMilliseconds, Math.Min(sw2.ElapsedMilliseconds, sw3.ElapsedMilliseconds));
            if (minTime == sw1.ElapsedMilliseconds)
                Console.WriteLine("\nFileStream was the fastest!");
            else if (minTime == sw2.ElapsedMilliseconds)
                Console.WriteLine("\nStreamWriter/StreamReader was the fastest!");
            else
                Console.WriteLine("\nFile Class was the fastest!");
        }

        private static Stopwatch FIleClassIO(string filePath3, string bigText)
        {
            var sw3 = Stopwatch.StartNew();

            File.WriteAllText(filePath3, bigText);
            string readText3 = File.ReadAllText(filePath3);

            sw3.Stop();
            Console.WriteLine($"[File Class] WriteAllText + ReadAllText: {sw3.ElapsedMilliseconds} ms");
            return sw3;
        }

        private static Stopwatch StreamReaderAndWriterIO(string filePath2, string bigText)
        {
            var sw2 = Stopwatch.StartNew();

            using (StreamWriter writer = new StreamWriter(filePath2))
            {
                writer.Write(bigText);
            }

            using (StreamReader reader = new StreamReader(filePath2))
            {
                string readText = reader.ReadToEnd();
            }

            sw2.Stop();
            Console.WriteLine($"[StreamWriter/Reader] Write + Read: {sw2.ElapsedMilliseconds} ms");
            return sw2;
        }

        private static Stopwatch FileStreamIO(string filePath1, string bigText)
        {
            var sw1 = Stopwatch.StartNew();

            using (FileStream fs = new FileStream(filePath1, FileMode.Create, FileAccess.Write))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(bigText);
                fs.Write(bytes, 0, bytes.Length);
            }

            using (FileStream fs = new FileStream(filePath1, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[fs.Length];
                fs.ReadExactly(buffer);
                string readText = Encoding.UTF8.GetString(buffer);
            }

            sw1.Stop();
            Console.WriteLine($"[FileStream] Write + Read: {sw1.ElapsedMilliseconds} ms");
            return sw1;
        }
    }
}
