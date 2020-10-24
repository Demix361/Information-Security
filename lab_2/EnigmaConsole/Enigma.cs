using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Text;

namespace EnigmaConsole
{
    class Enigma
    {
        private int rotorAmount;
        private Rotor[] rotorArray;
        private Reflector refl;
        private int alphSize;
        private int[] rotorPositions;

        // Properties
        public int RotorAmount
        { get; set; }
        public Rotor[] RotorArray
        { get; set; }
        public Reflector Refl
        { get; set; }
        public int AlphSize
        { get; set; }
        public int[] RotorPositions
        { get; set; }

        // Constructors
        public Enigma(int rotorNum, int alphNum)
        {
            AlphSize = alphNum;
            RotorAmount = rotorNum;

            RotorArray = new Rotor[RotorAmount];
            for (int i = 0; i < RotorAmount; i++)
            {
                RotorArray[i] = new Rotor(AlphSize);
            }

            Refl = new Reflector(AlphSize);

            RotorPositions = new int[RotorAmount];
        }

        // Methods
        public void saveRotors()
        {
            for (int i = 0; i < RotorAmount; i++)
            {
                RotorPositions[i] = RotorArray[i].Pos;
            }
        }

        public void loadRotors()
        {
            for (int i = 0; i < RotorAmount; i++)
            {
                RotorArray[i].Pos = RotorPositions[i];
            }
        }

        public int[] ArrayEncrypt(int[] message)
        {
            int[] encryptedMessage = new int[message.Length];
            int encI = 0;

            foreach (int sym in message)
            {
                int newSym = SymEncrypt(sym);

                encryptedMessage[encI] = newSym;
                encI += 1;
            }

            return encryptedMessage;
        }

        public int SymEncrypt(int sym)
        {
            int newSym = sym;
            foreach (Rotor rotor in RotorArray)
            {
                newSym = rotor.forward(newSym);
            }

            newSym = Refl.process(newSym);

            for (int i = RotorAmount - 1; i > -1; i--)
            {
                newSym = RotorArray[i].backward(newSym);
            }

            for (int i = 0; i < RotorAmount; i++)
            {
                if (RotorArray[i].Pos == AlphSize - 1)
                {
                    //RotorArray[i].Pos = 0;
                    RotorArray[i].roll();
                }
                else
                {
                    RotorArray[i].roll();
                    //RotorArray[i].Pos += 1;
                    break;
                }
            }

            return newSym;
        }

        public void SaveConfiguration(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Create);

            //fs.WriteByte((byte)RotorAmount);
            //fs.WriteByte((byte)AlphSize);

            foreach (int sym in Refl.Symbols)
            {
                fs.WriteByte((byte)sym);
            }

            foreach (Rotor r in RotorArray)
            {
                fs.WriteByte((byte)r.Pos);
                foreach (int sym in r.Symbols)
                {
                    fs.WriteByte((byte)sym);
                }
            }

            fs.Close();
        }

        public void LoadConfiguration(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open);

            //RotorAmount = fs.ReadByte();
            //AlphSize = fs.ReadByte();

            for(int i = 0; i < Refl.Size; i++)
            {
                Refl.Symbols[i] = fs.ReadByte();
            }

            foreach (Rotor r in RotorArray)
            {
                r.Pos = fs.ReadByte();
                for (int i = 0; i < r.Size; i++)
                {
                    r.Symbols[i] = fs.ReadByte();
                }
            }

            fs.Close();
        }

        public void FileEncrypt(string inFilename, string outFilename)
        {
            FileStream inF = new FileStream(inFilename, FileMode.Open);
            FileStream outF = new FileStream(outFilename, FileMode.Create);

            int sym;
            while (inF.CanRead)
            {
                sym = inF.ReadByte();
                if (sym == -1)
                    break;
                else
                    outF.WriteByte((byte)SymEncrypt(sym));
                    if (sym > 255)
                        Console.WriteLine($"{sym} ");
            }
            Console.WriteLine();

            inF.Close();
            outF.Close();
        }

        public void PrintRotors()
        {
            for (int i = 0; i < RotorAmount; i++)
            {
                Console.WriteLine($"Rotor {i + 1}: [pos: {RotorArray[i].Pos}] [size: {RotorArray[i].Size}]");
                for (int j = 0; j < RotorArray[i].Size; j++)
                {
                    Console.Write($"{RotorArray[i].Symbols[j]} ");
                }
                Console.WriteLine();
            }
        }

        public void PrintReflector()
        {
            Console.WriteLine($"Deflector: [size: {Refl.Size}]");
            for (int i = 0; i < Refl.Size; i++)
            {
                Console.Write($"{Refl.Symbols[i]} ");
            }
            Console.WriteLine();
        }
    }
}
