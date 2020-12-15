using System;

namespace Huffman
{
    class Program
    {
        static void Main(string[] args)
        {
            HuffmanCode huff = new HuffmanCode();

            string basepath = @"D:\Libraries\Music\6\";
            string filetype = ".txt";
            string inFilename = basepath + "3" + filetype;
            string outFilename = basepath + "FILE_enc" + filetype;
            string outFilename2 = basepath + "FILE_dec" + filetype;

            huff.CompressFile(inFilename, outFilename);

            huff = new HuffmanCode();

            huff.DecompressFile(outFilename, outFilename2);
        }
    }
}
