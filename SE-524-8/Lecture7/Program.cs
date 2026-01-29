namespace Lecture7
{
    internal class Program
    {
        static void Main(string[] args)
        {
        }


        public static int MyLength(string str)
        {
            int length = 0;
            foreach (char c in str)
                length++;
            return length;
        }
        public static bool MyStartsWith(string str, string value)
        {
            if (MyLength(value) > MyLength(str)) return false;

            for (int i = 0; i < MyLength(value); i++)
            {
                if (str[i] != value[i])
                    return false;
            }
            return true;
        }
        public static bool MyEndsWith(string str, string value)
        {
            int strLen = MyLength(str);
            int valLen = MyLength(value);

            if (valLen > strLen) return false;

            for (int i = 0; i < valLen; i++)
            {
                if (str[strLen - valLen + i] != value[i])
                    return false;
            }
            return true;
        }
        public static string MySubstring(string str, int startIndex)
        {
            int len = MyLength(str);
            if (startIndex < 0 || startIndex >= len)
                throw new ArgumentOutOfRangeException();

            char[] result = new char[len - startIndex];
            int j = 0;
            for (int i = startIndex; i < len; i++)
            {
                result[j++] = str[i];
            }
            return new string(result);
        }
        public static string MySubstring(string str, int startIndex, int length)
        {
            int len = MyLength(str);
            if (startIndex < 0 || length < 0 || startIndex + length > len)
                throw new ArgumentOutOfRangeException();

            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = str[startIndex + i];
            }
            return new string(result);
        }
        public static int MyIndexOf(string str, char c)
        {
            int index = 0;
            foreach (char ch in str)
            {
                if (ch == c) return index;
                index++;
            }
            return -1;
        }
        public static int MyIndexOf(string str, string value)
        {
            int strLen = MyLength(str);
            int valLen = MyLength(value);

            if (valLen == 0) return 0;

            for (int i = 0; i <= strLen - valLen; i++)
            {
                bool found = true;
                for (int j = 0; j < valLen; j++)
                {
                    if (str[i + j] != value[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found) return i;
            }
            return -1;
        }
        public static string MyTrim(string str)
        {
            int start = 0;
            int end = MyLength(str) - 1;

            while (start <= end && str[start] == ' ')
                start++;
            while (end >= start && str[end] == ' ')
                end--;

            return MySubstring(str, start, end - start + 1);
        }
        public static string[] MySplit(string str, char separator)
        {
            List<string> parts = new List<string>();
            int start = 0;

            for (int i = 0; i < MyLength(str); i++)
            {
                if (str[i] == separator)
                {
                    parts.Add(MySubstring(str, start, i - start));
                    start = i + 1;
                }
            }
            parts.Add(MySubstring(str, start, MyLength(str) - start));
            return parts.ToArray();
        }

    }
}
