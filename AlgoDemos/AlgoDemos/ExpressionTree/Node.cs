using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoDemos.ExpressionTree
{
    public class Node
    {
        public Node LeftChild { get; set; }
        public char Info { get; set; }
        public Node RightChild { get; set; }

        public Node(char c)
        {
            Info = c;
            LeftChild = null;
            RightChild = null;
                
        }
    }
}
