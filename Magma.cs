namespace Program
{
    public class Magma
    {
        public uint[] Keys = new uint[32];
        private uint[,] TransformTable = new uint[8, 16]
        {
            {12, 4, 6, 2, 10, 5, 11, 9, 14, 8, 13, 7, 0, 3, 15, 1},
            {6, 8, 2, 3, 9, 10, 5, 12, 1, 14, 4, 7, 11, 13, 0, 15},
            {11, 3, 5, 8, 2, 15, 10, 13, 14, 1, 7, 4, 12, 9, 6, 0},
            {12, 8, 2, 1, 13, 4, 15, 6, 7, 0, 10, 5, 3, 14, 9, 11},
            {7, 15, 5, 10, 8, 1, 6, 13, 0, 9, 3, 14, 11, 4, 2, 12},
            {5, 13, 15, 6, 9, 2, 12, 10, 11, 7, 8, 1, 4, 3, 14, 0},
            {8, 14, 2, 5, 6, 9, 1, 12, 15, 4, 11, 0, 13, 10, 3, 7},
            {1, 7, 14, 13, 0, 5, 8, 3, 4, 15, 10, 6, 9, 12, 11, 2}
        };

        public Magma()
        {
            Console.WriteLine("-Объект \"Магма\" создан!");

            Console.WriteLine("  Введите ключи:");
            //Keys[0] = 0xffeeddcc;
            //Keys[1] = 0xbbaa9988;
            //Keys[2] = 0x77665544;
            //Keys[3] = 0x33221100;
            //Keys[4] = 0xf0f1f2f3;
            //Keys[5] = 0xf4f5f6f7;
            //Keys[6] = 0xf8f9fafb;
            //Keys[7] = 0xfcfdfeff;
            for (int i = 0; i < 8; i++)
            {
                Console.Write("  {0}) K = ", i + 1);
                Keys[i] = (uint)Int32.Parse(Console.ReadLine());
                Keys[i + 8] = Keys[i];
                Keys[i + 16] = Keys[i];
                Keys[31 - i] = Keys[i];
            }
        }

        public Number64 EncryptNumberBlock(Number64 block)
        {
            for (int i = 0; i < 32; i++)
            {
                uint current_key = Keys[i];
                block = Round(block, current_key);
            }

            // Поменять местами левую и правую части
            uint temp = block.left;
            block.left = block.right;
            block.right = temp;

            return block;
        }

        public string Encrypt(string input)
        {
            ulong[] numbers = TextHelper.ConvertToLong(input);
            ulong[] numbers2 = new ulong[numbers.Length];
            
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers2[i] = (ulong)EncryptNumberBlock(numbers[i]);
            }

            return TextHelper.ConvertToString(numbers2);
        }

       public void EncryptFile(string file, string destination)
        {
            if (File.Exists(file))
            {
                byte[] file_bytes = File.ReadAllBytes(file);

                string text = System.Text.Encoding.Unicode.GetString(file_bytes);

                string encrypted = Encrypt(text);

                byte[] bytes = System.Text.Encoding.Unicode.GetBytes(encrypted);

                File.WriteAllBytes(destination, bytes);
            }
        }

        public Number64 DecryptNumberBlock(Number64 block)
        {
            for (int i = 31; i >= 0; i--)
            {
                uint current_key = Keys[i];
                block = Round(block, current_key);
            }

            // Поменять местами левую и правую части
            uint temp = block.left;
            block.left = block.right;
            block.right = temp;

            return block;
        }

        public string Decrypt(string input)
        {
            ulong[] numbers = TextHelper.ConvertToLong(input);
            ulong[] numbers2 = new ulong[numbers.Length];

            for (int i = 0; i < numbers.Length; i++)
            {
                numbers2[i] = (ulong)DecryptNumberBlock(numbers[i]);
            }

            return TextHelper.ConvertToString(numbers2);
        }

        public void DecryptFile(string file, string destination)
        {
            if (File.Exists(file))
            {
                byte[] file_bytes = File.ReadAllBytes(file);

                string text = System.Text.Encoding.Unicode.GetString(file_bytes);

                string decrypted = Decrypt(text);

                byte[] bytes = System.Text.Encoding.Unicode.GetBytes(decrypted);

                File.WriteAllBytes(destination, bytes);
            }
        }

        public uint G(uint number, uint key)
        {
            number += key;
            number = T_full(number);
            number = RotateLeft(number);

            return number;
        }

        public uint RotateLeft(uint number)
        {
            return (number << 11) | (number >> (32 - 11));
        }

        public Number64 Round(Number64 number, uint current_key)
        {
            uint left = number.left;
            uint right = number.right;
            uint right_memory = right;

            right = G(right, current_key);
            right ^= left;

            left = right_memory;
            ulong new_number = ((ulong)left << 32) + (ulong)right;
            number.Recreate(new_number);

            return number;
        }

        public uint T_full(uint number)
        {
            int step = 28;
            uint ones = 15;
            uint current;
            for (int i = 0; i < 8; i++)
            {
                current = number >> step;
                current &= 15;              // Взяли последние 4 бита

                current = T(current, 7-i);
                number &= ~(ones << step);    // Занулили единицы, где надо
                number |= current << step;  // Вставили единицы, где надо

                step -= 4;
            }

            return number;
        }

        public uint T(uint number, int row)
        {
            return TransformTable[row, number];
        }
    }
}
