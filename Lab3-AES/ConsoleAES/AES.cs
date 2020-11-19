using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ConsoleAES
{
    class AES
    {
        byte[] Sbox;
        byte[] InvSbox;
        byte[] Rcon;

        public AES()
        {
            Sbox = new byte[256]
            {
                0x63, 0x7c, 0x77, 0x7b, 0xf2, 0x6b, 0x6f, 0xc5, 0x30, 0x01, 0x67, 0x2b, 0xfe, 0xd7, 0xab, 0x76,
                0xca, 0x82, 0xc9, 0x7d, 0xfa, 0x59, 0x47, 0xf0, 0xad, 0xd4, 0xa2, 0xaf, 0x9c, 0xa4, 0x72, 0xc0,
                0xb7, 0xfd, 0x93, 0x26, 0x36, 0x3f, 0xf7, 0xcc, 0x34, 0xa5, 0xe5, 0xf1, 0x71, 0xd8, 0x31, 0x15,
                0x04, 0xc7, 0x23, 0xc3, 0x18, 0x96, 0x05, 0x9a, 0x07, 0x12, 0x80, 0xe2, 0xeb, 0x27, 0xb2, 0x75,
                0x09, 0x83, 0x2c, 0x1a, 0x1b, 0x6e, 0x5a, 0xa0, 0x52, 0x3b, 0xd6, 0xb3, 0x29, 0xe3, 0x2f, 0x84,
                0x53, 0xd1, 0x00, 0xed, 0x20, 0xfc, 0xb1, 0x5b, 0x6a, 0xcb, 0xbe, 0x39, 0x4a, 0x4c, 0x58, 0xcf,
                0xd0, 0xef, 0xaa, 0xfb, 0x43, 0x4d, 0x33, 0x85, 0x45, 0xf9, 0x02, 0x7f, 0x50, 0x3c, 0x9f, 0xa8,
                0x51, 0xa3, 0x40, 0x8f, 0x92, 0x9d, 0x38, 0xf5, 0xbc, 0xb6, 0xda, 0x21, 0x10, 0xff, 0xf3, 0xd2,
                0xcd, 0x0c, 0x13, 0xec, 0x5f, 0x97, 0x44, 0x17, 0xc4, 0xa7, 0x7e, 0x3d, 0x64, 0x5d, 0x19, 0x73,
                0x60, 0x81, 0x4f, 0xdc, 0x22, 0x2a, 0x90, 0x88, 0x46, 0xee, 0xb8, 0x14, 0xde, 0x5e, 0x0b, 0xdb,
                0xe0, 0x32, 0x3a, 0x0a, 0x49, 0x06, 0x24, 0x5c, 0xc2, 0xd3, 0xac, 0x62, 0x91, 0x95, 0xe4, 0x79,
                0xe7, 0xc8, 0x37, 0x6d, 0x8d, 0xd5, 0x4e, 0xa9, 0x6c, 0x56, 0xf4, 0xea, 0x65, 0x7a, 0xae, 0x08,
                0xba, 0x78, 0x25, 0x2e, 0x1c, 0xa6, 0xb4, 0xc6, 0xe8, 0xdd, 0x74, 0x1f, 0x4b, 0xbd, 0x8b, 0x8a,
                0x70, 0x3e, 0xb5, 0x66, 0x48, 0x03, 0xf6, 0x0e, 0x61, 0x35, 0x57, 0xb9, 0x86, 0xc1, 0x1d, 0x9e,
                0xe1, 0xf8, 0x98, 0x11, 0x69, 0xd9, 0x8e, 0x94, 0x9b, 0x1e, 0x87, 0xe9, 0xce, 0x55, 0x28, 0xdf,
                0x8c, 0xa1, 0x89, 0x0d, 0xbf, 0xe6, 0x42, 0x68, 0x41, 0x99, 0x2d, 0x0f, 0xb0, 0x54, 0xbb, 0x16
            };

            InvSbox = new byte[256]
            {
                0x52, 0x09, 0x6a, 0xd5, 0x30, 0x36, 0xa5, 0x38, 0xbf, 0x40, 0xa3, 0x9e, 0x81, 0xf3, 0xd7, 0xfb,
                0x7c, 0xe3, 0x39, 0x82, 0x9b, 0x2f, 0xff, 0x87, 0x34, 0x8e, 0x43, 0x44, 0xc4, 0xde, 0xe9, 0xcb,
                0x54, 0x7b, 0x94, 0x32, 0xa6, 0xc2, 0x23, 0x3d, 0xee, 0x4c, 0x95, 0x0b, 0x42, 0xfa, 0xc3, 0x4e,
                0x08, 0x2e, 0xa1, 0x66, 0x28, 0xd9, 0x24, 0xb2, 0x76, 0x5b, 0xa2, 0x49, 0x6d, 0x8b, 0xd1, 0x25,
                0x72, 0xf8, 0xf6, 0x64, 0x86, 0x68, 0x98, 0x16, 0xd4, 0xa4, 0x5c, 0xcc, 0x5d, 0x65, 0xb6, 0x92,
                0x6c, 0x70, 0x48, 0x50, 0xfd, 0xed, 0xb9, 0xda, 0x5e, 0x15, 0x46, 0x57, 0xa7, 0x8d, 0x9d, 0x84,
                0x90, 0xd8, 0xab, 0x00, 0x8c, 0xbc, 0xd3, 0x0a, 0xf7, 0xe4, 0x58, 0x05, 0xb8, 0xb3, 0x45, 0x06,
                0xd0, 0x2c, 0x1e, 0x8f, 0xca, 0x3f, 0x0f, 0x02, 0xc1, 0xaf, 0xbd, 0x03, 0x01, 0x13, 0x8a, 0x6b,
                0x3a, 0x91, 0x11, 0x41, 0x4f, 0x67, 0xdc, 0xea, 0x97, 0xf2, 0xcf, 0xce, 0xf0, 0xb4, 0xe6, 0x73,
                0x96, 0xac, 0x74, 0x22, 0xe7, 0xad, 0x35, 0x85, 0xe2, 0xf9, 0x37, 0xe8, 0x1c, 0x75, 0xdf, 0x6e,
                0x47, 0xf1, 0x1a, 0x71, 0x1d, 0x29, 0xc5, 0x89, 0x6f, 0xb7, 0x62, 0x0e, 0xaa, 0x18, 0xbe, 0x1b,
                0xfc, 0x56, 0x3e, 0x4b, 0xc6, 0xd2, 0x79, 0x20, 0x9a, 0xdb, 0xc0, 0xfe, 0x78, 0xcd, 0x5a, 0xf4,
                0x1f, 0xdd, 0xa8, 0x33, 0x88, 0x07, 0xc7, 0x31, 0xb1, 0x12, 0x10, 0x59, 0x27, 0x80, 0xec, 0x5f,
                0x60, 0x51, 0x7f, 0xa9, 0x19, 0xb5, 0x4a, 0x0d, 0x2d, 0xe5, 0x7a, 0x9f, 0x93, 0xc9, 0x9c, 0xef,
                0xa0, 0xe0, 0x3b, 0x4d, 0xae, 0x2a, 0xf5, 0xb0, 0xc8, 0xeb, 0xbb, 0x3c, 0x83, 0x53, 0x99, 0x61,
                0x17, 0x2b, 0x04, 0x7e, 0xba, 0x77, 0xd6, 0x26, 0xe1, 0x69, 0x14, 0x63, 0x55, 0x21, 0x0c, 0x7d
            };

            Rcon = new byte[44]
            {
                0x00, 0x00, 0x00, 0x00,
                0x01, 0x00, 0x00, 0x00,
                0x02, 0x00, 0x00, 0x00,
                0x04, 0x00, 0x00, 0x00,
                0x08, 0x00, 0x00, 0x00,
                0x10, 0x00, 0x00, 0x00,
                0x20, 0x00, 0x00, 0x00,
                0x40, 0x00, 0x00, 0x00,
                0x80, 0x00, 0x00, 0x00,
                0x1b, 0x00, 0x00, 0x00,
                0x36, 0x00, 0x00, 0x00
            };
        }

        public void SubWord(byte[] word)
        {
            for (int i = 0; i < 4; i++)
                word[i] = Sbox[word[i]];
        }

        // сдвиг байтов на 1 влево
        public void RotWord(byte[] word)
        {
            byte t = word[0];
            for (int i = 0; i < 3; i++)
            {
                word[i] = word[i + 1];
                word[i + 1] = t;
            }
        }

        // returns Nr + 1 round keys (16-byte)
        public byte[] KeyExpansion(byte[] key, int Nk, int Nr)
        {
            byte[] temp = new byte[4];
            byte[] roundKeys = new byte[16 * (Nr + 1)];
            byte[] xor_temp = new byte[4];

            Array.Copy(key, 0, roundKeys, 0, Nk * 4);

            int k = 0;
            byte[] rcon_temp = new byte[4] { 0, 0, 0, 0 };
            for (int i = Nk; i < 4 * (Nr + 1); i++)
            {
                Array.Copy(roundKeys, (i - 1) * 4, temp, 0, 4);

                if (i % Nk == 0)
                {
                    RotWord(temp);
                    SubWord(temp);
                    rcon_temp[0] = Rcon[k * 4];
                    temp = XOR(temp, rcon_temp);
                    k += 1;
                }
                else if (Nk == 8 && i % Nk == 4)
                {
                    SubWord(temp);
                }

                Array.Copy(roundKeys, (i - Nk) * 4, xor_temp, 0, 4);
                Array.Copy(XOR(temp, xor_temp), 0, roundKeys, i * 4, 4);
            }

            return roundKeys;
        }

        public void AddRoundKey(byte[,] state, byte[] RoundKeys, int round)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    state[i, j] = XOR(state[i, j], RoundKeys[round * 16 + i * 4 + j]);
                }
            }
        }

        public byte[] XOR(byte[] a, byte[] b)
        {
            int n = a.Length;
            byte[] c = new byte[n];

            for (int i = 0; i < n; i++)
                c[i] = (byte)(a[i] ^ b[i]);

            return c;
        }

        public byte XOR(byte a, byte b)
        {
            byte c = (byte)(a ^ b);

            return c;
        }

        public int GetNr(int Nk)
        {
            int Nr;
            if (Nk == 4)
                Nr = 10;
            else if (Nk == 6)
                Nr = 12;
            else
                Nr = 14;
            return Nr;
        }

        public void SubBytes(byte[,] state)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    state[i, j] = Sbox[state[i, j]];
        }

        public void InvSubBytes(byte[,] state)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    state[i, j] = InvSbox[state[i, j]];
        }

        public void ShiftRows(byte[,] state)
        {
            byte temp;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < i + 1; j++)
                {
                    temp = state[i + 1, 0];
                    for (int k = 0; k < 3; k++)
                    {
                        state[i + 1, k] = state[i + 1, k + 1];
                        state[i + 1, k + 1] = temp;
                    }
                    
                }
            }
        }

        public void InvShiftRows(byte[,] state)
        {
            byte temp;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < i + 1; j++)
                {
                    temp = state[i + 1, 3];
                    for (int k = 3; k > 0; k--)
                    {
                        state[i + 1, k] = state[i + 1, k - 1];
                        state[i + 1, k - 1] = temp;
                    }

                }
            }
        }

        // Galois Field (256) Multiplication of two Bytes
        private byte GMul(byte a, byte b)
        { 
            byte p = 0;

            for (int counter = 0; counter < 8; counter++)
            {
                if ((b & 1) != 0)
                {
                    p ^= a;
                }

                bool hi_bit_set = (a & 0x80) != 0;
                a <<= 1;
                if (hi_bit_set)
                {
                    a ^= 0x1B; /* x^8 + x^4 + x^3 + x + 1 */
                }
                b >>= 1;
            }

            return p;
        }

        public void MixColumns(byte[,] s)
        {
            byte[,] ss = new byte[4, 4];
            Array.Clear(ss, 0, ss.Length);

            for (int c = 0; c < 4; c++)
            {
                ss[0, c] = (byte)(GMul(0x02, s[0, c]) ^ GMul(0x03, s[1, c]) ^ s[2, c] ^ s[3, c]);
                ss[1, c] = (byte)(s[0, c] ^ GMul(0x02, s[1, c]) ^ GMul(0x03, s[2, c]) ^ s[3, c]);
                ss[2, c] = (byte)(s[0, c] ^ s[1, c] ^ GMul(0x02, s[2, c]) ^ GMul(0x03, s[3, c]));
                ss[3, c] = (byte)(GMul(0x03, s[0, c]) ^ s[1, c] ^ s[2, c] ^ GMul(0x02, s[3, c]));
            }

            Array.Copy(ss, 0, s, 0, 16);
        }

        public void InvMixColumns(byte[,] s)
        {
            byte[,] ss = new byte[4, 4];
            Array.Clear(ss, 0, ss.Length);

            for (int c = 0; c < 4; c++)
            {
                ss[0, c] = (byte)(GMul(0x0e, s[0, c]) ^ GMul(0x0b, s[1, c]) ^ GMul(0x0d, s[2, c]) ^ GMul(0x09, s[3, c]));
                ss[1, c] = (byte)(GMul(0x09, s[0, c]) ^ GMul(0x0e, s[1, c]) ^ GMul(0x0b, s[2, c]) ^ GMul(0x0d, s[3, c]));
                ss[2, c] = (byte)(GMul(0x0d, s[0, c]) ^ GMul(0x09, s[1, c]) ^ GMul(0x0e, s[2, c]) ^ GMul(0x0b, s[3, c]));
                ss[3, c] = (byte)(GMul(0x0b, s[0, c]) ^ GMul(0x0d, s[1, c]) ^ GMul(0x09, s[2, c]) ^ GMul(0x0e, s[3, c]));
            }

            Array.Copy(ss, 0, s, 0, 16);
        }

        public byte[] EncryptBlock(byte[] roundKeys, int Nr, byte[] block)
        {
            byte[,] state = new byte[4, 4];
            byte[] res = new byte[16];
            
            for (int r = 0; r < 4; r++)
                for (int c = 0; c < 4; c++)
                    state[r, c] = block[r + 4 * c];

            AddRoundKey(state, roundKeys, 0);

            for (int round = 1; round < Nr; round++)
            {
                SubBytes(state);
                ShiftRows(state);
                MixColumns(state);
                AddRoundKey(state, roundKeys, round);
            }

            SubBytes(state);
            ShiftRows(state);
            AddRoundKey(state, roundKeys, Nr);

            for (int r = 0; r < 4; r++)
                for (int c = 0; c < 4; c++)
                    res[r + 4 * c] = state[r, c];

            return res;
        }

        public byte[] DecryptBlock(byte[] roundKeys, int Nr, byte[] block)
        {
            byte[,] state = new byte[4, 4];
            byte[] res = new byte[16];

            for (int r = 0; r < 4; r++)
                for (int c = 0; c < 4; c++)
                    state[r, c] = block[r + 4 * c];

            AddRoundKey(state, roundKeys, Nr);

            for (int round = Nr - 1; round > 0; round--)
            {
                InvShiftRows(state);
                InvSubBytes(state);
                AddRoundKey(state, roundKeys, round);
                InvMixColumns(state);
            }
            
            InvShiftRows(state);
            InvSubBytes(state);
            AddRoundKey(state, roundKeys, 0);

            for (int r = 0; r < 4; r++)
                for (int c = 0; c < 4; c++)
                    res[r + 4 * c] = state[r, c];

            return res;
        }

        public void WriteBlock(byte[] block, FileStream FS)
        {
            int n = block.Length;
            for (int i = 0; i < n; i++)
                FS.WriteByte(block[i]);
        }

        public void EncryptFile(string inFilename, string outFilename, byte[] key)
        {
            FileStream inFS = new FileStream(inFilename, FileMode.Open);
            FileStream outFS = new FileStream(outFilename, FileMode.Create);

            int sym;
            byte[] block = new byte[16];
            int i = 0;

            int Nk = key.Length / 4;
            int Nr = GetNr(Nk);
            byte[] roundKeys = KeyExpansion(key, Nk, Nr);


            // Write input file length in 16-byte format to output file
            int inFileLen = (int)new FileInfo(inFilename).Length;
            string inFileLenStr = Convert.ToString(inFileLen, 2).PadLeft(128, '0');
            for (int j = 0; j < 16; j++)
            {
                int inFileLenPart = Convert.ToInt32(inFileLenStr.Substring(j * 8, 8), 2);
                outFS.WriteByte((byte)inFileLenPart);
            }

            while (inFS.CanRead)
            {
                sym = inFS.ReadByte();

                if (sym == -1)
                {
                    if (i != 0)
                    {
                        block = EncryptBlock(roundKeys, Nr, block);
                        WriteBlock(block, outFS);
                    }

                    break;
                }
                else
                {
                    block[i] = (byte)sym;

                    i += 1;
                    if (i == 16)
                    {
                        i = 0;

                        block = EncryptBlock(roundKeys, Nr, block);
                        WriteBlock(block, outFS);

                        Array.Clear(block, 0, 16);
                    }
                }
            }

            inFS.Close();
            outFS.Close();
        }

        public void DecryptFile(string inFilename, string outFilename, byte[] key)
        {
            FileStream inFS = new FileStream(inFilename, FileMode.Open);
            FileStream outFS = new FileStream(outFilename, FileMode.Create);

            int sym;
            byte[] block = new byte[16];

            int Nk = key.Length / 4;
            int Nr = GetNr(Nk);
            byte[] roundKeys = KeyExpansion(key, Nk, Nr);

            // Restore original file length from encoded file
            int inFileLen = (int)new FileInfo(inFilename).Length - 16;
            string outFileLenStr = string.Empty;
            for (int j = 0; j < 16; j++)
            {
                int outFileLenPart = inFS.ReadByte();
                outFileLenStr += Convert.ToString(outFileLenPart, 2).PadLeft(8, '0');
            }
            int outFileLen = Convert.ToInt32(outFileLenStr, 2);
            int n = inFileLen / 16 - 1;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    sym = inFS.ReadByte();
                    block[j] = (byte)sym;
                }

                block = DecryptBlock(roundKeys, Nr, block);
                WriteBlock(block, outFS);
                Array.Clear(block, 0, 16);
            }

            for (int j = 0; j < 16; j++)
            {
                sym = inFS.ReadByte();
                block[j] = (byte)sym;
            }

            block = DecryptBlock(roundKeys, Nr, block);
            if (inFileLen == outFileLen)
            {
                WriteBlock(block, outFS);
            }
            else
            {
                byte[] cutBlock = new byte[inFileLen - outFileLen];
                Array.Copy(block, 0, cutBlock, 0, cutBlock.Length);
                WriteBlock(cutBlock, outFS);
            }

            inFS.Close();
            outFS.Close();
        }
    }
}
