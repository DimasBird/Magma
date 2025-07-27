namespace Program
{
    public static class Program
    {
        public static void Main()
        {
            Magma magma = new Magma();

            string file = "text.txt";
            string destination = "encrypted_txt.mgm";
            string destination2 = "decrypted_txt.txt";

            magma.EncryptFile(file, destination);

            magma.DecryptFile(destination, destination2);

            string text = "Hello, World!";
            Console.WriteLine(text = magma.Encrypt(text));
            Console.WriteLine(text = magma.Decrypt(text));
        }
    }
}