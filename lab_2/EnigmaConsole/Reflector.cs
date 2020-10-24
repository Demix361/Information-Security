using System;
using System.Collections.Generic;
using System.Text;

namespace EnigmaConsole
{
    class Reflector
    {
        private int size;
        private int[] symbols;

        // Properties
        public int Size
        { get; set; }
        public int[] Symbols
        { get; set; }

        // Constructors
        public Reflector(int alphSize)
        {
            Size = alphSize;
            Symbols = new int[Size];
            
            for (int i = 0; i < Size; i++)
            {
                Symbols[Size - 1 - i] = i;
            }
        }

        // Methods
        public int process(int symbol)
        {
            return Symbols[symbol];
        }
    }
}
