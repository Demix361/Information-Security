using System;

namespace EnigmaConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Enigma machine = new Enigma(3, 256);

            string pathBase = @"D:\Libraries\Music\enigma\";
            string fileType = ".docx";
            string pathCnfg = pathBase + "cnfg.ngm";
            string pathIn = pathBase + "fil" + fileType;
            string pathOut = pathBase + "file_enc" + fileType;
            string pathOut2 = pathBase + "file_dec" + fileType;

            machine.SaveConfiguration(pathCnfg);

            machine.FileEncrypt(pathIn, pathOut);

            machine.LoadConfiguration(pathCnfg);

            machine.FileEncrypt(pathOut, pathOut2);
        }
    }
}
