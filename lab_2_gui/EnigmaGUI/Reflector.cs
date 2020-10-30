using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnigmaGUI
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
        public Reflector(int alphSize, Random randObj)
        {
            Size = alphSize;
            Symbols = new int[Size];

            for (int i = 0; i < Size; i++)
            {
                Symbols[i] = -1;
            }

            for (int i = 0; i < Size / 2; i++)
            {
                while (true)
                {
                    int a = randObj.Next(256);
                    int b = randObj.Next(256);
                    if (a != b && Symbols[a] == -1 && Symbols[b] == -1)
                    {
                        Symbols[a] = b;
                        Symbols[b] = a;
                        break;
                    }
                }

            }

        }
        public Reflector(int alphSize)
        {
            Size = alphSize;
            Symbols = new int[Size];
        }

        // Methods
        public int process(int symbol)
        {
            return Symbols[symbol];
        }
    }
}
