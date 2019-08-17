using AlgoDemos.ExpressionTree;
using System;

namespace AlgoDemos
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var expTree = new Etree();
            String postfix = "45+3/7*42/-";
            expTree.Demo(postfix);

            Console.Write("Press any key to continue ...");
            Console.Read();

        }
    }
}
