using System;
using System.IO;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Check())
                Console.WriteLine("Hello World!");

            Console.ReadKey();
        }

        static bool Check()
        {
            string filename = "licence.txt";
            if (!File.Exists(filename))
            {
                Console.WriteLine("Installer must be launched first.");
                return false;
            }

            byte[] data = File.ReadAllBytes(filename);
            if (Installer.Program.VerifyCurrentMachine(data))
                return true;
            else
                Console.WriteLine("Error. Different machine.");
            return false;
        }
    }
}
