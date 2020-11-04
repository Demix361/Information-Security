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
            string filetype = ".docx";
            string inFilename = basepath + "fil" + filetype;
            string outFilename = basepath + "FILE_enc" + filetype;
            string outFilename2 = basepath + "FILE_dec" + filetype;
            string key = "1111000011110000110011000011001111110000111100001100110000110011";

            string key2 = "ad128906ff43a1a0";
            string key_2_bit = des.keyHexToBit(key2);


            des.encrypt(inFilename, outFilename, key_2_bit);

            des.decrypt(outFilename, outFilename2, key_2_bit);

        }
    }
}
