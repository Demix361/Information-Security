using System;
using System.IO;

namespace ConsoleDES
{
    class DES
    {
        int[] IPtable;
        int[] Etable;
        int[,,] Stable;
        int[] Ptable;
        int[] FPtable;
        int[] Ctable;
        int[] Dtable;
        int[] Sitable;
        int[] CPtable;

        public DES()
        {
            IPtable = new int[64]
            {
                58, 50, 42, 34, 26, 18, 10, 2, 60, 52, 44, 36, 28, 20, 12, 4,
                62, 54, 46, 38, 30, 22, 14, 6, 64, 56, 48, 40, 32, 24, 16, 8,
                57, 49, 41, 33, 25, 17, 9, 1, 59, 51, 43, 35, 27, 19, 11, 3,
                61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7
            };

            Etable = new int[48]
            {
                32, 1,  2,  3,  4,  5,
                4,  5,  6,  7,  8,  9,
                8,  9,  10, 11, 12, 13,
                12, 13, 14, 15, 16, 17,
                16, 17, 18, 19, 20, 21,
                20, 21, 22, 23, 24, 25,
                24, 25, 26, 27, 28, 29,
                28, 29, 30, 31, 32, 1
            };

            Stable = new int[8, 4, 16]
            {
                {
                    { 14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7 },
                    { 0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8 },
                    { 4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0 },
                    { 15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13 }
                },

                {
                    { 15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10 },
                    { 3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5 },
                    { 0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15 },
                    { 13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9 }
                },

                {
                    { 10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8 },
                    { 13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1 },
                    { 13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7 },
                    { 1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12 }
                },

                {
                    { 7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15 },
                    { 13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9 },
                    { 10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4 },
                    { 3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14 }
                },

                {
                    { 2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9 },
                    { 14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6 },
                    { 4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14 },
                    { 11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3 }
                },

                {
                    { 12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11 },
                    { 10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8 },
                    { 9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6 },
                    { 4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13 }
                },

                {
                    { 4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1 },
                    { 13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6 },
                    { 1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2 },
                    { 6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12 }
                },

                {
                    { 13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7 },
                    { 1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2 },
                    { 7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8 },
                    { 2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11 }
                }
            };

            Ptable = new int[32]
            {
                16, 7,  20, 21, 29, 12, 28, 17,
                1,  15, 23, 26, 5,  18, 31, 10,
                2,  8,  24, 14, 32, 27, 3,  9,
                19, 13, 30, 6, 22,  11, 4,  25
            };

            FPtable = new int[64]
            {
                40, 8,  48, 16, 56, 24, 64, 32, 39, 7,  47, 15, 55, 23, 63, 31,
                38, 6,  46, 14, 54, 22, 62, 30, 37, 5,  45, 13, 53, 21, 61, 29,
                36, 4,  44, 12, 52, 20, 60, 28, 35, 3,  43, 11, 51, 19, 59, 27,
                34, 2,  42, 10, 50, 18, 58, 26, 33, 1,  41, 9,  49, 17, 57, 25
            };

            Ctable = new int[28]
            {
                57, 49, 41, 33, 25, 17, 9,
                1,  58, 50, 42, 34, 26, 18,
                10, 2,  59, 51, 43, 35, 27,
                19, 11, 3,  60, 52, 44, 36
            };

            Dtable = new int[28]
            {
                63, 55, 47, 39, 31, 23, 15,
                7,  62, 54, 46, 38, 30, 22,
                14, 6,  61, 53, 45, 37, 29,
                21, 13, 5,  28, 20, 12, 4
            };

            Sitable = new int[16]
            {
                1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1
            };

            CPtable = new int[48]
            {
                14, 17,  11,  24,  1,   5,   3,   28,  15,  6,   21,  10,  23,  19,  12,  4,
                26,  8,   16,  7,   27,  20,  13,  2,   41,  52,  31,  37,  47,  55,  30,  40,
                51,  45,  33,  48,  44,  49,  39,  56,  34,  53,  46,  42,  50,  36,  29,  32
            };
        }

        // Permutaion function 64-bit -> 64-bit
        public string IP(string block)
        {
            string newBlock = string.Empty;

            for (int i = 0; i < 64; i++)
                newBlock += block[IPtable[i] - 1];

            return newBlock;
        }

        // Permutaion function 32-bit -> 48-bit
        public string E(string block)
        {
            string newBlock = string.Empty;

            for (int i = 0; i < 48; i++)
                newBlock += block[Etable[i] - 1];

            return newBlock;
        }

        // Permutaion function 48-bit -> 32-bit
        public string P(string block)
        {
            string newBlock = string.Empty;

            for (int i = 0; i < 32; i++)
                newBlock += block[Ptable[i] - 1];

            return newBlock;
        }

        // Permutaion function 64-bit -> 64-bit
        public string FP(string block)
        {
            string newBlock = string.Empty;

            for (int i = 0; i < 64; i++)
                newBlock += block[FPtable[i] - 1];

            return newBlock;
        }

        // Changes every 8th bit in main key so every byte has odd number of "1"
        public string OddKeyBytes(string key)
        {
            string newKey = string.Empty;
            int ones;

            for (int i = 0; i < 8; i++)
            {
                ones = 0;

                for (int j = 0; j < 8; j++)
                {
                    if (j == 7)
                    {
                        if (ones % 2 == 0)
                            newKey += '1';
                        else
                            newKey += '0';
                    }
                    else
                    {
                        newKey += key[i * 8 + j];
                        if (key[i * 8 + j] == '1')
                            ones += 1;
                    }
                }
            }

            return newKey;
        }

        // Generates 16 48-bit round keys from main key
        public string[] RoundKeyGen(string key)
        {
            string newKey = OddKeyBytes(key);
            string[] roundKeys = new string[16];

            int pos = 0;

            for (int i = 0; i < 16; i++)
            {
                pos += Sitable[i];
                string tempKey = string.Empty;
                roundKeys[i] = string.Empty;

                for (int j = 0; j < 56; j++)
                {
                    if (j < 26)
                        tempKey += newKey[Ctable[(j + pos) % 26] - 1];
                    else
                        tempKey += newKey[Dtable[(j - 26 + pos) % 26] - 1];
                }

                for (int j = 0; j < 48; j++)
                    roundKeys[i] += tempKey[CPtable[j] - 1];
            }

            return roundKeys;
        }

        // XOR function gets two equal sized bit-strings
        public string XOR(string a, string b)
        {
            string c = string.Empty;
            int n = a.Length;

            for (int i = 0; i < n; i++)
            {
                if (a[i] == b[i])
                    c += '0';
                else
                    c += '1';
            }

            return c;
        }

        // F function gets 32-bit block, 48-bit round key; returns 32-bit block
        public string F(string block, string key)
        {
            block = E(block);

            string b = XOR(block, key);
            string b_new = string.Empty;

            for (int j = 0; j < 8; j++)
            {
                string bj = b.Substring(j * 6, 6);

                string x_str = bj.Substring(0, 1) + bj.Substring(5, 1);
                string y_str = bj.Substring(1, 4);
                int x_num = Convert.ToInt32(x_str, 2);
                int y_num = Convert.ToInt32(y_str, 2);

                int bj_new_num = Stable[j, x_num, y_num];
                string bj_new = Convert.ToString(bj_new_num, 2).PadLeft(4, '0');

                b_new += bj_new;
            }

            b_new = P(b_new);
            return b_new;
        }

        // Feistel function gets and returns 64-bit block
        public string Feistel(string LRblock, string roundKey)
        {
            string left;
            string right;
            string res = string.Empty;

            left = LRblock.Substring(0, 32);
            right = LRblock.Substring(32, 32);

            res += right;
            res += XOR(left, F(right, roundKey));

            return res;
        }

        // Encrypt 64-bit block using array of round keys
        public string EncryptBlock(string block, string[] roundKeys)
        {
            block = IP(block);

            for (int i = 0; i < 16; i++)
                block = Feistel(block, roundKeys[i]);

            block = block.Substring(32, 32) + block.Substring(0, 32);

            block = FP(block);

            return block;
        }

        // Decrypt 64-bit block using array of round keys
        public string DecryptBlock(string block, string[] roundKeys)
        {
            block = IP(block);

            for (int i = 0; i < 16; i++)
                block = Feistel(block, roundKeys[15 - i]);

            block = block.Substring(32, 32) + block.Substring(0, 32);

            block = FP(block);

            return block;
        }

        // Encrypt input file usuing hexademical key
        public void EncryptFile(string inFilename, string outFilename, string key)
        {
            FileStream inFS = new FileStream(inFilename, FileMode.Open);
            FileStream outFS = new FileStream(outFilename, FileMode.Create);
            key = TransformKey(key);

            int sym;
            string block = string.Empty;
            int i = 0;
            string[] roundKeys = RoundKeyGen(key);

            // Write input file length in 8-byte format to output file
            int inFileLen = (int)new FileInfo(inFilename).Length;
            string inFileLenStr = Convert.ToString(inFileLen, 2).PadLeft(64, '0');
            for (int j = 0; j < 8; j++)
            {
                int inFileLenPart = Convert.ToInt32(inFileLenStr.Substring(j * 8, 8), 2);
                outFS.WriteByte((byte)inFileLenPart);
            }

            while (inFS.CanRead)
            {
                sym = inFS.ReadByte();

                if (sym == -1)
                {
                    if (block.Length != 0)
                    {
                        block = EncryptBlock(block.PadRight(64, '0'), roundKeys);
                        WriteBlock(block, outFS);
                    }

                    break;
                }
                else
                {
                    block += Convert.ToString((byte)sym, 2).PadLeft(8, '0');

                    i += 1;
                    if (i == 8)
                    {
                        i = 0;

                        block = EncryptBlock(block, roundKeys);
                        WriteBlock(block, outFS);

                        block = string.Empty;
                    }
                }
            }

            inFS.Close();
            outFS.Close();
        }

        // Decrypt output file using hexademical key
        public void DecryptFile(string inFilename, string outFilename, string key)
        {
            FileStream inFS = new FileStream(inFilename, FileMode.Open);
            FileStream outFS = new FileStream(outFilename, FileMode.Create);
            key = TransformKey(key);

            int sym;
            string block = string.Empty;
            string[] roundKeys = RoundKeyGen(key);

            // Restore original file length from encoded file
            int inFileLen = (int)new FileInfo(inFilename).Length - 8;
            string outFileLenStr = string.Empty;
            for (int j = 0; j < 8; j++)
            {
                int outFileLenPart = inFS.ReadByte();
                outFileLenStr += Convert.ToString(outFileLenPart, 2).PadLeft(8, '0');
            }
            int outFileLen = Convert.ToInt32(outFileLenStr, 2);
            int n = inFileLen / 8 - 1;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    sym = inFS.ReadByte();
                    block += Convert.ToString((byte)sym, 2).PadLeft(8, '0');
                }

                block = DecryptBlock(block, roundKeys);
                WriteBlock(block, outFS);
                block = string.Empty;
            }

            for (int j = 0; j < 8; j++)
            {
                sym = inFS.ReadByte();
                block += Convert.ToString((byte)sym, 2).PadLeft(8, '0');
            }

            block = DecryptBlock(block, roundKeys);
            WriteBlock(block.Substring(0, 64 - (inFileLen - outFileLen) * 8), outFS);

            inFS.Close();
            outFS.Close();
        }

        // Transforms hexademical key (size = 0..16) to 64 bit key
        public string TransformKey(string key)
        {
            string bit_str = string.Empty;
            int n = key.Length;

            for (int i = 0; i < n; i++)
            {
                int a = Convert.ToInt32(key.Substring(i, 1), 16);
                string b = Convert.ToString(a, 2);
                bit_str += b.PadLeft(4, '0');
            }

            return bit_str.PadLeft(64, '0');
        }

        // Writes 64-bit block to filestream
        public void WriteBlock(string block, FileStream FS)
        {
            int n = block.Length / 8;
            for (int i = 0; i < n; i++)
            {
                string sym_str = block.Substring(i * 8, 8);
                int sym = Convert.ToInt32(sym_str, 2);

                FS.WriteByte((byte)sym);
            }
        }
    }
}
