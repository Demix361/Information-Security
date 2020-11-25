using System;

namespace ConsoleRSA
{
    class Program
    {
        static void Main(string[] args)
        {
            RSA rsa = new RSA(128);
            rsa.Info();

            string basepath = @"D:\Libraries\Music\rsa\";
            string filetype = ".zip";
            string inFilename = basepath + "arch" + filetype;
            string outFilename = basepath + "FILE_enc" + filetype;
            string outFilename2 = basepath + "FILE_dec" + filetype;

            rsa.EncryptFile(inFilename, outFilename);
            rsa.DecryptFile(outFilename, outFilename2);
        }
    }
}
