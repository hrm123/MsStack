using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoDemos.ExpressionTree
{
    class StackNode
    {
        public Node[] StackArray { get; }
        public int Top { get; set; }

        public int Size => Top + 1;
        public bool IsEmpty => Top == -1;
        public bool IsFull => Top == StackArray.Length - 1;


        public StackNode()
        {
            StackArray = new Node[10];
            Top = -1;
        }

        public StackNode(int maxSize)
        {
            StackArray = new Node[maxSize];
            Top = -1;
        }

        public void Push(Node x)
        {
            if (IsFull)
            {
                Console.WriteLine("stakc overflow due to size \n");
                return;
            }
            Top += 1;
            StackArray[Top] = x;
        }

        public Node Pop()
        {
            Node x;
            if (IsEmpty)
            {
                Console.WriteLine("stakc underflow \n");
                throw new System.InvalidOperationException();
            }
            x = StackArray[Top];
            Top -= 1;
            return x;
        }
    }
}
