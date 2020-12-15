using System;

namespace DigitalSignature
{
    class Program
    {
        static void Main(string[] args)
        {
            Signer signer = new Signer("SHA512");

            string basepath = @"D:\Libraries\Music\5\";
            string dataFilename = basepath + "1.txt";
            string signatureFilename = basepath + "signature.txt";

            signer.Sign(dataFilename, signatureFilename);
            Console.WriteLine(signer.CheckSignature(dataFilename, signatureFilename));
        }
    }
}
