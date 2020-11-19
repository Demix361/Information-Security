using System;

namespace ConsoleAES
{
    class Program
    {
        static void Main(string[] args)
        {
            AES aes = new AES();

            byte[] key = new byte[16] { 1, 234, 22, 12, 99, 45, 172, 100, 18, 24, 212, 92, 199, 145, 19, 107 };
            byte[] block = new byte[16] { 123, 234, 22, 12, 99, 172, 100, 18, 24, 212, 2, 199, 145, 19, 9, 145 };

            string basepath = @"D:\Libraries\Music\aes\";
            string filetype = ".docx";
            string inFilename = basepath + "otchet" + filetype;
            string outFilename = basepath + "FILE_enc" + filetype;
            string outFilename2 = basepath + "FILE_dec" + filetype;

            aes.EncryptFile(inFilename, outFilename, key);

            aes.DecryptFile(outFilename, outFilename2, key);

        }
    }
}
