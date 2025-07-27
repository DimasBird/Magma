using System.Text;

namespace Program
{
    public static class TextHelper
    {
        public static ulong[] ConvertToLong(string text)
        {
            int length = text.Length;
            int ulongCount = (length + 3) / 4;
            ulong[] numbers = new ulong[ulongCount];
            int currentIndex = 0;
            int shift = 48;

            foreach (char c in text)
            {
                numbers[currentIndex] |= (ulong)c << shift;
                shift -= 16;

                if (shift < 0)
                {
                    currentIndex++;
                    shift = 48;
                }
            }

            return numbers;
        }

        public static string ConvertToString(ulong[] numbers)
        {
            StringBuilder sb = new StringBuilder();

            foreach (ulong number in numbers)
            {
                for (int shift = 48; shift >= 0; shift -= 16)
                {
                    ushort charValue = (ushort)((number >> shift) & 0xFFFF);
                    sb.Append((char)charValue);
                }
            }

            // Удаляем нули в конце строки
            string result = sb.ToString();
            result = result.TrimEnd('\0'); // Удаляем все нулевые символы в конце

            return result;
        }

        public static string OpenFile(string path)
        {
            if (File.Exists(path))
            {
                Console.WriteLine("File found!");
                return File.ReadAllText(path);
            }
            else
            {
                Console.WriteLine("No file found");
                return null;
            }
        }
    }
}