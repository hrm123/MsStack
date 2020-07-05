using AlgoDemos.DijkstrasAlgo;
using AlgoDemos.ExpressionTree;
using AlgoDemos.Graph;
using AlgoDemos.Graph.AdjList;
using AlgoDemos.Graph.Connectivity;
using AlgoDemos.MST.KruskalsAlgo;
using AlgoDemos.puzzles;
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

            /*
            DiGraphAdjMatr adjMatrixDemo = new DiGraphAdjMatr();
            adjMatrixDemo.Demo();
            */

            /*
            LinkedDiGraph linkedDiGraph = new LinkedDiGraph();
            linkedDiGraph.Demo();
            */

            /*
            DiWtedGraphShortestPath dijkDemo = new DiWtedGraphShortestPath();
            dijkDemo.Demo();
            */

            /*
            UnDiGraphConComp g = new UnDiGraphConComp();
            g.Demo();
            */

            //UndiWtedGraphKruskal g = new UndiWtedGraphKruskal();
            // g.Demo();

            // OptimalTransfers optTrans = new OptimalTransfers();
            // optTrans.Demo();

            /*
            AllPalindromes palindromes = new AllPalindromes();
            palindromes.Demo();
            */

            /*
            NumberPuzzles numberPuzzles = new NumberPuzzles();
            numberPuzzles.demo();
            */

            LinkedListStuff lls = new LinkedListStuff();
            lls.demo();

            Console.Write("Press any key to continue ...");
            Console.Read();

        }
    }
}
