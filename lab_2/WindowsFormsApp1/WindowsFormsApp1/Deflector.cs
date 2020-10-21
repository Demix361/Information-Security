using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WindowsFormsApp1
{
    class Deflector
    {
        private int[] alphabet;
        private int[] symbols;

        public int[] Alphabet
        { get; set; }
        public int[] Symbols
        { get; set; }

        // Constructors
        public Deflector(int[] alphabet)
        {
            Alphabet = alphabet;
            int n = Alphabet.Length;

            int[] data = new int[n];
            for (int i = 0; i < n; i++)
            {
                data[n - 1 - i] = Alphabet[i];
            }
            Symbols = data;
        }

        // Methods
        public int process(int symbol)
        {
            return Symbols[Array.IndexOf(Alphabet, symbol)];
        }

    }
}
