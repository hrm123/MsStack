using AlgoDemos.ExpressionTree;
using AlgoDemos.Graph;

using System;

namespace AlgoDemos
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            /*
            var expTree = new Etree();
            String postfix = "45+3/7*42/-";
            expTree.Demo(postfix);
            */

            DiGraphAdjMatr adjMatrixDemo = new DiGraphAdjMatr();
            adjMatrixDemo.Demo();

            

            Console.Write("Press any key to continue ...");
            Console.Read();

        }
    }
}
