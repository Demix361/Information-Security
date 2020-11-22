using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.IO;

namespace ConsoleRSA
{
    class RSA
    {
        BigInteger P;
        BigInteger Q;
        BigInteger N;
        BigInteger Phi;
        BigInteger E;
        BigInteger D;
        Random RandObj;

        public RSA(int keyLength)
        {
            RandObj = new Random();
            P = GenPrime(keyLength / 2);
            Q = GenPrime(keyLength / 2);
            N = P * Q;
            Phi = (P - 1) * (Q - 1);
            E = GenE();
            D = ExtendedEuclideanAlgorithm(E, Phi);

            Console.WriteLine(Phi);
            Console.WriteLine(E);
            Console.WriteLine(D);
            Console.WriteLine((E * D) % Phi);

        }

        // methods
        public BigInteger GenE()
        {
            while (true)
            {
                BigInteger num = RandomBigInteger(Phi - 1) + 1; // [2, Phi - 1]

                if (MillerRabinTest(num, 100))
                    return num;
            }
        }

        public BigInteger GenPrime(int byteSize)
        {
            while (true)
            {
                BigInteger num = GenBigInt(byteSize);

                if (MillerRabinTest(num, 100))
                    return num;
            }
        }

        // returns positive BigInteger of given byte length
        public BigInteger GenBigInt(int byteSize)
        {
            byte[] byteNum = new byte[byteSize + 1];

            for (int i = 0; i < byteSize; i++)
            {
                byteNum[i] = (byte)RandObj.Next(256);
            }
            byteNum[byteSize] = 0;

            BigInteger num = new BigInteger(byteNum);
                
            return num;
        }

        // returns a random BigInteger [1, N-1].
        public BigInteger RandomBigInteger(BigInteger N)
        {
            BigInteger result = 0;

            do
            {
                int length = (int)Math.Ceiling(BigInteger.Log(N, 2));
                int numBytes = (int)Math.Ceiling(length / 8.0);
                byte[] data = new byte[numBytes];
                RandObj.NextBytes(data);
                result = new BigInteger(data);
            } while (result >= N || result <= 0);

            return result;
        }

        public bool MillerRabinTest(BigInteger num, int rounds)
        {
            if (num.IsEven)
                return false;

            BigInteger s = new BigInteger(0);
            BigInteger d = num - 1;

            while (d.IsEven)
            {
                s += 1;
                d /= 2;
            }

            /*
            for (int i = 0; i < rounds; i++)
            {
                BigInteger a = RandomBigInteger(num - 2) + 1; // [2, n-2]
                BigInteger x = BigInteger.ModPow(a, d, num);

                if (x != 1 && x != num - 1)
                {
                    if (s - 1 < 1)
                    {
                        Console.WriteLine($"{i + 1} [1]");
                        return false;
                    }

                    for (int j = 0; j < s - 1; j++)
                    {
                        x = BigInteger.ModPow(x, 2, num);

                        if (x == num - 1)
                        {
                            break;
                        }
                        else if (j == s - 2)
                        {
                            Console.WriteLine($"{i + 1}  [2] (j={j})");
                            return false;
                        }
                    }
                }
            }

            Console.WriteLine(rounds);
            return true;
            */

            for (int i = 0; i < rounds; i++)
            {
                BigInteger a = RandomBigInteger(num - 2) + 1; // [2, n-2]
                BigInteger x = BigInteger.ModPow(a, d, num);

                if (x == 1 || x == num - 1)
                    continue;

                for (int j = 0; j < s - 1; j++)
                {
                    x = BigInteger.ModPow(x, 2, num);

                    if (x == 1)
                    {
                        //Console.WriteLine($"{i + 1} [1]");
                        return false;
                    }
                    if (x == num - 1)
                    {
                        break;
                    }
                }

                if (x != num - 1)
                {
                    //Console.WriteLine($"{i + 1} [2]");
                    return false;
                }
            }

            return true;
        }

        // a > b => a=phi, b=e, t -> d
        public BigInteger ExtendedEuclideanAlgorithm(BigInteger a, BigInteger b)
        {
            BigInteger x, y, m, q;
            x = 0;
            y = 1;
            m = b;

            while (true)
            {
                if (a == 1)
                    return y;
                if (a == 0)
                {
                    Console.WriteLine("Doesnt exist");
                    return b;
                }

                q = b / a;
                b -= a * q;
                x += q * y;

                if (b == 1)
                    return m - x;
                if (b == 0)
                {
                    Console.WriteLine("Doesnt exist");
                    return a;
                }

                q = a / b;

                a -= b * q;
                y += q * x;
            }

        }

        public byte[] EncryptBlock(byte[] block, int outSize)
        {
            int blockSize = block.Length;
            //BigInteger num = BytearrToBigint(block);
            BigInteger num = new BigInteger(block, isUnsigned: true, isBigEndian: true);

            num = BigInteger.ModPow(num, E, N);

            byte[] temp = num.ToByteArray(isUnsigned: true, isBigEndian: true);
            byte[] byteCoded = new byte[outSize];
            Array.Copy(temp, 0, byteCoded, outSize - temp.Length, temp.Length);

            Console.WriteLine($"byteCoded len: {temp.Length} ({blockSize})");

            return byteCoded;
        }

        public void EncryptFile(string inFilename, string outFilename)
        {
            FileStream inFS = new FileStream(inFilename, FileMode.Open);
            FileStream outFS = new FileStream(outFilename, FileMode.Create);

            int cutBytes = 1;
            int blockSize = N.GetByteCount(isUnsigned: true) - cutBytes;
            byte[] block = new byte[blockSize];
            byte[] blockCut;
            int sym, i = 0, k = 0;

            //write to the top of file amount of cut bytes
            int inFileLen = (int)new FileInfo(inFilename).Length;
            string inFileLenStr = Convert.ToString(inFileLen, 16).PadLeft((blockSize + cutBytes) * 2, '0');
            for (int j = 0; j < blockSize + cutBytes; j++)
            {
                int inFileLenPart = Convert.ToInt32(inFileLenStr.Substring(j * 2, 2), 16);
                outFS.WriteByte((byte)inFileLenPart);
            }

            while (inFS.CanRead)
            {
                sym = inFS.ReadByte();

                if (sym == -1)
                {
                    if (i != 0)
                    {
                        k += 1;
                        Console.Write($"[{k}] ");

                        blockCut = new byte[i];
                        Array.Copy(block, blockCut, i);
                        block = EncryptBlock(blockCut, blockSize + cutBytes);
                        WriteBlock(block, outFS);
                    }

                    break;
                }
                else
                {
                    block[i] = (byte)sym;
                    i += 1;

                    if (i == blockSize)
                    {
                        i = 0;

                        k += 1;
                        Console.Write($"[{k}] ");

                        block = EncryptBlock(block, blockSize + cutBytes);
                        WriteBlock(block, outFS);

                        block = new byte[blockSize];
                    }
                }
            }

            inFS.Close();
            outFS.Close();
        }



        public byte[] DecryptBlock(byte[] block, int outSize)
        {
            int blockSize = block.Length;
            BigInteger num = new BigInteger(block, isUnsigned: true, isBigEndian: true);

            num = BigInteger.ModPow(num, D, N);

            byte[] temp = num.ToByteArray(isUnsigned: true, isBigEndian: true);
            byte[] byteCoded = new byte[outSize];
            Array.Copy(temp, 0, byteCoded, outSize - temp.Length, temp.Length);

            Console.WriteLine($"byteCoded len: {temp.Length} ({blockSize})");

            return byteCoded;
        }


        public void DecryptFile(string inFilename, string outFilename)
        {
            FileStream inFS = new FileStream(inFilename, FileMode.Open);
            FileStream outFS = new FileStream(outFilename, FileMode.Create);

            int cutBytes = 1;
            int blockSize = N.GetByteCount(isUnsigned: true);
            byte[] block = new byte[blockSize];
            int sym, k = 0;

            //
            string outFileLenStr = string.Empty;
            for (int j = 0; j < blockSize; j++)
            {
                int outFileLenPart = inFS.ReadByte();
                outFileLenStr += Convert.ToString(outFileLenPart, 16).PadLeft(2, '0');
            }
            int ogFileLen = Convert.ToInt32(outFileLenStr, 16);
            int cutBlockSize = ogFileLen % (blockSize - cutBytes);
            int blocksAmount = ogFileLen / (blockSize - cutBytes);

            Console.WriteLine(blocksAmount);
            Console.WriteLine(cutBlockSize);

            for (int j = 0; j < blocksAmount; j++)
            {
                for (int i = 0; i < blockSize; i++)
                {
                    sym = inFS.ReadByte();
                    block[i] = (byte)sym;   
                }

                k += 1;
                Console.Write($"[{k}] ");

                block = DecryptBlock(block, blockSize - cutBytes);
                WriteBlock(block, outFS);

                block = new byte[blockSize];
            }

            if (cutBlockSize != 0)
            {
                for (int i = 0; i < blockSize; i++)
                {
                    sym = inFS.ReadByte();
                    block[i] = (byte)sym;
                }

                k += 1;
                Console.Write($"[{k}] ");

                block = DecryptBlock(block, cutBlockSize);
                WriteBlock(block, outFS);
            }

            inFS.Close();
            outFS.Close();
        }


        public void WriteBlock(byte[] block, FileStream FS)
        {
            int n = block.Length;
            for (int i = 0; i < n; i++)
                FS.WriteByte(block[i]);
        }
    }



    
}
