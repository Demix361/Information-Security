using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WindowsFormsApp1
{
    class Rotor
    {
        private int[] alphabet;
        private int pos;
        private int[] symbols;
        private int savedPos;

        public int[] Alphabet
        { get; set; }
        public int Pos
        { get; set; }
        public int[] Symbols
        { get; set; }
        public int SavedPos
        { get; set; }

        // Constructors
        public Rotor(int[] alphabet)
        {
            Alphabet = alphabet;
            Pos = 0;
            SavedPos = 0;

            Random randObj = new Random();
            int[] data = new int[Alphabet.Length];
            for (int i = 0; i < Alphabet.Length; i++)
            {
                int j = randObj.Next(i + 1);
                if (j != i)
                {
                    data[i] = data[j];
                }
                data[j] = Alphabet[i];
            }
            Symbols = data;
        }

        // Methods
        public int forward(int symbol)
        {
            return Symbols[(Alphabet.Length + Array.IndexOf(Alphabet, symbol) - Pos) % Alphabet.Length];
        }

        public int backward(int symbol)
        {
            return (Array.IndexOf(Symbols, symbol) + Pos) % Alphabet.Length;
        }

        public void roll()
        {
            Pos += 1;
            if (Pos == Alphabet.Length)
            {
                Pos = 0;
            }
        }

        public void load()
        {
            Pos = SavedPos;
        }

        public void save()
        {
            SavedPos = Pos;
        }

        // !!!!!!!!!!!!
        public void printSymbols()
        {
            for (int i = 0; i < Symbols.Length; i++)
            {
                Console.Write($"{Symbols[i]} ");
            }
        }
    }
}

