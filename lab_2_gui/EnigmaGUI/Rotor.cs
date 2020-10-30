using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnigmaGUI
{
    class Rotor
    {
        private int pos;
        private int[] symbols;
        private int size;

        // Properties
        public int Pos
        { get; set; }
        public int[] Symbols
        { get; set; }
        public int Size
        { get; set; }

        // Constructors
        public Rotor(int alphSize, Random randObj)
        {
            Size = alphSize;
            Pos = randObj.Next(Size - 1);
            Symbols = new int[Size];

            for (int i = 0; i < Size; i++)
            {
                int j = randObj.Next(i + 1);
                if (j != i)
                {
                    Symbols[i] = Symbols[j];
                }
                Symbols[j] = i;
            }
        }

        public Rotor(int alphSize)
        {
            Size = alphSize;
            Symbols = new int[Size];
        }

        // Methods
        public int forward(int symbol)
        {
            return Symbols[(Size + symbol - Pos) % Size];
        }

        public int backward(int symbol)
        {
            return (Array.IndexOf(Symbols, symbol) + Pos) % Size;
        }

        public void roll()
        {
            Pos += 1;
            if (Pos == Size)
            {
                Pos = 0;
            }
        }
    }
}
