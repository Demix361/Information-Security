using System;
using System.IO;

namespace ConsoleDES
{
    class Program
    {
        static void Main(string[] args)
        {
            DES des = new DES();

            string basepath = @"D:\Libraries\Music\source\";
            string filetype = ".png";
            string inFilename = basepath + "emoji" + filetype;
            string outFilename = basepath + "FILE_enc" + filetype;
            string outFilename2 = basepath + "FILE_dec" + filetype;
            string key = "1111000011110000110011000011001111110000111100001100110000110011";

            FileStream inF = new FileStream(inFilename, FileMode.Open);
            FileStream outF = new FileStream(outFilename, FileMode.Create);

            des.encrypt(inF, outF, key);

            inF.Close();
            outF.Close();

            
            inF = new FileStream(outFilename, FileMode.Open);
            outF = new FileStream(outFilename2, FileMode.Create);

            des.decrypt(inF, outF, key);

            inF.Close();
            outF.Close();
            

        }
    }
}
