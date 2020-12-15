using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Huffman
{
    class HuffmanCode
    {
        public int[] FrequencyArr;
        public string[] CodeArr;
        public Tree FrequencyTree;

        public HuffmanCode()
        {
            FrequencyArr = new int[256];
            Array.Clear(FrequencyArr, 0, 256);
            CodeArr = new string[256];
            for (int i = 0; i < 256; i++)
                CodeArr[i] = string.Empty;
            FrequencyTree = new Tree();
        }


        public void FillCodeArr_old()
        {
            int n = 256;

            for (int i = 0; i < n - 1; i++)
            {
                int left = -1, right = -1;
                int min_1 = Int32.MaxValue, min_2 = Int32.MaxValue;

                for (int j = 0; j < n; j++)
                    if (FrequencyArr[j] >= 0 && FrequencyArr[j] <= min_1)
                    {
                        min_1 = FrequencyArr[j];
                        left = j;
                    }

                for (int j = 0; j < n; j++)
                    if (FrequencyArr[j] >= 0 && FrequencyArr[j] <= min_2 && j != left)
                    {
                        min_2 = FrequencyArr[j];
                        right = j;
                    }

                CodeArr[left] += "0";
                for (int j = 0; j < n; j++)
                    if (FrequencyArr[j] == 0 - left)
                        CodeArr[j] += "0";

                CodeArr[right] += "1";
                for (int j = 0; j < n; j++)
                    if (FrequencyArr[j] == 0 - right)
                        CodeArr[j] += "1";

                FrequencyArr[right] += FrequencyArr[left];
                FrequencyArr[left] = 0 - right;
            }

            for (int i = 0; i < n; i++)
            {
                char[] charArray = CodeArr[i].ToCharArray();
                Array.Reverse(charArray);
                CodeArr[i] = new string(charArray);
            }
        }

        // Прямой обход
        public void NLR(TreeNode node, string code)
        {
            if (node != null)
            {
                if (node.Sym != -1)
                {
                    CodeArr[node.Sym] = code;
                }
                NLR(node.LeftNode, code + "0");
                NLR(node.RightNode, code + "1");
            }
        }

        public void FillCodeArr()
        {
            NLR(FrequencyTree.RootNode, "");
        }

        public void FillFrequencyArr(byte[] buffer)
        {
            int n = buffer.Length;

            for (int i = 0; i < n; i++)
                FrequencyArr[buffer[i]] += 1;
        }

        public void BuildTree()
        {
            int nmax = 255 + 256;

            TreeNode[] NodeArr = new TreeNode[nmax];
            for (int i = 0; i < 256; i++)
            {
                NodeArr[i] = new TreeNode(i, FrequencyArr[i]);
            }

            // Добавляем в массив связующие узлы
            for (int i = 256; i < nmax; i++)
            {
                NodeArr[i] = new TreeNode(-1, Int32.MaxValue);
            }

            
            for (int i = 0; i < 255; i++)
            {   
                // Сортируем массив узлов по возрастанию частоты
                Array.Sort(NodeArr, delegate(TreeNode x, TreeNode y) { return x.Frequency.CompareTo(y.Frequency); });

                TreeNode Left = NodeArr[0];
                TreeNode Right = NodeArr[1];
                NodeArr[nmax - 1].LeftNode = Left;
                NodeArr[nmax - 1].RightNode = Right;
                NodeArr[nmax - 1].Frequency = Left.Frequency + Right.Frequency;

                nmax -= 2;
                TreeNode[] NewNodeArr = new TreeNode[nmax];
                Array.Copy(NodeArr, 2, NewNodeArr, 0, nmax);
                NodeArr = NewNodeArr;
            }

            FrequencyTree.RootNode = NodeArr[0];
        }

        public void CompressFile(string inFilename, string outFilename)
        {
            FileStream inFS = new FileStream(inFilename, FileMode.Open);
            FileStream outFS = new FileStream(outFilename, FileMode.Create);
            int sym, i = 0;
            int inFileLen = (int)new FileInfo(inFilename).Length;
            byte[] buffer = new byte[inFileLen];
            string codeBuffer = string.Empty;

            while (inFS.CanRead)
            {
                sym = inFS.ReadByte();

                if (sym == -1)
                {
                   break;
                }
                else
                {
                    buffer[i] = (byte)sym;
                    i += 1;
                }
            }

            FillFrequencyArr(buffer);

            BuildTree();

            FillCodeArr();

            int k = 0;
            for (i = 0; i < 256; i++)
            {
                Console.Write($"{CodeArr[i]} ");
                k += CodeArr[i].Length * FrequencyArr[i];
            }
            Console.WriteLine();
            Console.WriteLine(k);
            Console.WriteLine();

            // Записываем размер изначального файла
            string temp = Convert.ToString(inFileLen, 16).PadLeft(8, '0');
            for (i = 0; i < 4; i++)
            {
                outFS.WriteByte((byte)Convert.ToInt32(temp.Substring(i * 2, 2), 16));
            }

            // Записываем массив частот в начало файла (1024bit)
            string freq;
            for (i = 0; i < 256; i++)
            {
                //Console.WriteLine($"{i} : {FrequencyArr[i]}");
                freq = Convert.ToString(FrequencyArr[i], 16).PadLeft(8, '0');
                for (int j = 0; j < 4; j++)
                {
                    int freq_part = Convert.ToInt32(freq.Substring(j * 2, 2), 16);
                    outFS.WriteByte((byte)freq_part);
                }
            }

            // Заносим двоичные коды в буфер
            for (i = 0; i < inFileLen; i++)
            {
                string code = CodeArr[buffer[i]];
                codeBuffer += code;
            }

            int codeBufferSize = codeBuffer.Length / 8;
            int codeBufferAdd = codeBuffer.Length % 8;

            //Console.WriteLine($"{codeBuffer.Length} {codeBufferSize} {codeBufferAdd}");

            // Дополняем буфер, чтобы он был кратен байту
            if (codeBufferAdd != 0)
            {
                for (i = 0; i < 8 - codeBufferAdd; i++)
                    codeBuffer += "0";
                codeBufferSize += 1;
            }

            // Записываем буфер кода
            for (i = 0; i < codeBufferSize; i++)
            {
                string sym_str = codeBuffer.Substring(i * 8, 8);
                sym = Convert.ToInt32(sym_str, 2);
                outFS.WriteByte((byte)sym);
            }

            inFS.Close();
            outFS.Close();
        }

        public void DecompressFile(string inFilename, string outFilename)
        {
            FileStream inFS = new FileStream(inFilename, FileMode.Open);
            FileStream outFS = new FileStream(outFilename, FileMode.Create);
            int sym;
            string buffer = string.Empty;
            int outFileLen;
            string temp = string.Empty;

            //
            for (int i = 0; i < 4; i++)
                temp += Convert.ToString(inFS.ReadByte(), 16).PadLeft(2, '0');
            outFileLen = Convert.ToInt32(temp, 16);

            // Заполняем массив частот из файла
            for (int i = 0; i < 256; i++)
            {
                temp = string.Empty;
                for (int j = 0; j < 4; j++)
                    temp += Convert.ToString(inFS.ReadByte(), 16).PadLeft(2, '0');
                FrequencyArr[i] = Convert.ToInt32(temp, 16);
            }

            // Заполняем двоичный буфер
            while (inFS.CanRead)
            {
                sym = inFS.ReadByte();

                if (sym == -1)
                    break;
                else
                    buffer += Convert.ToString(sym, 2).PadLeft(8, '0');
            }

            BuildTree();
            FillCodeArr();

            for (int i = 0; i < 256; i++)
                Console.Write($"{CodeArr[i]} ");
            Console.WriteLine();

            int k = 0;          
            TreeNode curNode = FrequencyTree.RootNode;
            for (int i = 0; k < outFileLen; i++)
            {
                if (buffer[i] == '0')
                {
                    if (curNode.LeftNode != null)
                    {
                        curNode = curNode.LeftNode;
                        if (curNode.Sym != -1)
                        {
                            k += 1;
                            outFS.WriteByte((byte)curNode.Sym);
                            curNode = FrequencyTree.RootNode;
                        }
                    }
                }
                else
                {
                    if (curNode.RightNode != null)
                    {
                        curNode = curNode.RightNode;
                        if (curNode.Sym != -1)
                        {
                            k += 1;
                            outFS.WriteByte((byte)curNode.Sym);
                            curNode = FrequencyTree.RootNode;
                        }
                    }
                }
            }

            inFS.Close();
            outFS.Close();
        }
    }
}
