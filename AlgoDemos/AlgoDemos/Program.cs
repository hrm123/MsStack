using AlgoDemos.codecamp;
using AlgoDemos.DijkstrasAlgo;
using AlgoDemos.ExpressionTree;
using AlgoDemos.Graph;
using AlgoDemos.Graph.AdjList;
using AlgoDemos.Graph.Connectivity;
using AlgoDemos.MST.KruskalsAlgo;
using System;

namespace AlgoDemos
{
    class Program
    {
        static void Main(string[] args)
        {
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
            

            UndiWtedGraphKruskal g = new UndiWtedGraphKruskal();
            g.Demo();

            Console.Write("Press any key to continue ...");
            Console.Read();
            */

            /*

            TwoSum ts = new TwoSum();
            ts.Run();

            */

            EditDistance ed = new EditDistance();
            Console.WriteLine(ed.MinDistance("horse", "ros")); 
        }
    }
}
