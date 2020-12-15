using System;
using System.Collections.Generic;
using System.Text;

namespace Huffman
{
    class TreeNode
    {
        public int Sym, Frequency;
        public TreeNode LeftNode;
        public TreeNode RightNode;
        public TreeNode ParentNode;

        public TreeNode (int sym, int freq)
        {
            Sym = sym;
            Frequency = freq;
        }
    }
}
