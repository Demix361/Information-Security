using System;

namespace EnigmaConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Enigma machine = new Enigma(3, 256);

            string pathBase = @"D:\Libraries\Music\";
            string fileType = ".txt";
            //string fileType = ".docx";
            string pathCnfg = pathBase + "cnfg.ngm";
            //string pathIn =  pathBase + "TEMY_DOKL_I_REF_ANTIChNAYa_FILOSOFIYa";
            string pathIn = pathBase + "test" + fileType;
            string pathOut = pathBase + "file_enc" + fileType;
            string pathOut2 = pathBase + "file_dec" + fileType;

            machine.SaveConfiguration(pathCnfg);

            machine.FileEncrypt(pathIn, pathOut);

            machine.LoadConfiguration(pathCnfg);

            machine.FileEncrypt(pathOut, pathOut2);
        }
    }
}
