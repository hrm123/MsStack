using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoDemos.ExpressionTree
{
    class Etree
    {
        public Node Root { get; set; }
        public Etree()
        {
            Root = null;
        }

        static bool IsOperator(char c)
        {
            return (c == '+' ||
                c == '-' ||
                c == '*' ||
                c == '/');

        }

        public void BuildTree(String postfix)
        {
            StackNode stack = new StackNode(30);
            Node currentNode;
            for(int i=0; i< postfix.Length; i++)
            {
                if (IsOperator(postfix[i]))
                {
                    currentNode = new Node(postfix[i]);
                    currentNode.RightChild = stack.Pop();
                    currentNode.LeftChild = stack.Pop();
                    stack.Push(currentNode);
                }
                else // operand
                {
                    currentNode = new Node(postfix[i]);
                    stack.Push(currentNode);
                }
            }
            Root = stack.Pop();
        }

        public void Prefix()
        {
            Preorder(Root);
            Console.WriteLine();
        }

        private void Preorder(Node p)
        {
            if (p == null)
                return;
            Console.Write(p.Info);
            Preorder(p.LeftChild);
            Preorder(p.RightChild);

        }

        public void Postfix()
        {
            Postorder(Root);
            Console.WriteLine();
        }

        private void Postorder(Node p)
        {
            if (p == null)
                return;
            
            Preorder(p.LeftChild);
            Preorder(p.RightChild);
            Console.Write(p.Info);
        }

        public void ParenthesizedInfix()
        {
            Inorder(Root);
            Console.WriteLine();
        }

        private void Inorder(Node p)
        {
            if (p == null)
                return;
            if (IsOperator(p.Info))
            {
                Console.Write("(");
            }
            Inorder(p.LeftChild);
            Console.Write(p.Info);
            Inorder(p.RightChild);
            if (IsOperator(p.Info))
            {
                Console.Write(")");
            }
        }

        public void Display()
        {
            Display(Root, 0);
            Console.WriteLine();
        }

        private void Display(Node p, int level)
        {
            int i;
            if(p == null)
            {
                return;
            }
            Display(p.RightChild, level + 1);
            Console.WriteLine();
            for(i=0; i< level; i++)
                Console.Write("   ");
            Console.Write(p.Info);
            Display(p.LeftChild, level + 1);
        }

        public int Evaluate()
        {
            if(Root == null)
            {
                return 0;
            }

            return Evaluate(Root);
        }

        private int Evaluate(Node p)
        {
            if(!IsOperator(p.Info))
            {
                return Convert.ToInt32(Char.GetNumericValue(p.Info));
            }
            int leftValue = Evaluate(p.LeftChild);
            int rightValue = Evaluate(p.RightChild);
            return (p.Info == '+') ? leftValue + rightValue :
                (p.Info == '-') ? leftValue - rightValue :
                (p.Info == '*') ? leftValue * rightValue :
                leftValue / rightValue;

        }

        public void Demo(String postfix)
        {
            BuildTree(postfix);
            Display();

            Console.Write("prefix expression : ");
            Prefix();

            Console.Write("postfix expression : ");
            Postfix();

            Console.Write("Parenthesized infix expression : ");
            ParenthesizedInfix();

            Console.Write("value of exp tree: ");
            Console.WriteLine(Evaluate());

        }

    }
}
