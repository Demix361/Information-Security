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

            FileStream inF = new FileStream(inFilename, FileMode.Open);
            FileStream outF = new FileStream(outFilename, FileMode.Create);

            des.encrypt(inF, outF);

            inF.Close();
            outF.Close();

        }
    }
}
