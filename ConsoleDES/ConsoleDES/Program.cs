using System;
using System.IO;

namespace ConsoleDES
{
    class Program
    {
        static void Main(string[] args)
        {
            DES des = new DES();

            string inFilename = @"D:\Libraries\Music\hello.txt";
            string outFilename = @"D:\Libraries\Music\hello_dec.txt";
            string key = "1111000011110000110011000011001111110000111100001100110000110011";

            FileStream inF = new FileStream(inFilename, FileMode.Open);
            FileStream outF = new FileStream(outFilename, FileMode.Create);

            des.encrypt(inF, outF, key);

            inF.Close();
            outF.Close();

        }
    }
}
