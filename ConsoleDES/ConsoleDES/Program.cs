using System;

namespace ConsoleDES
{
    class Program
    {
        static void Main(string[] args)
        {
            string basepath = @"D:\Libraries\Music\source\";
            string filetype = ".docx";
            string inFilename = basepath + "otchet_po_proizvodstvennoy_praktike" + filetype;
            string outFilename = basepath + "FILE_enc" + filetype;
            string outFilename2 = basepath + "FILE_dec" + filetype;

            // hexademical string, length = [0..16]
            string key = "a9b87c65432d1ef0";

            DES des = new DES();
            
            des.EncryptFile(inFilename, outFilename, key);

            des.DecryptFile(outFilename, outFilename2, key);
        }
    }
}
