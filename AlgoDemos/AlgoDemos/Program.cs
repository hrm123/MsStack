using AlgoDemos.DijkstrasAlgo;
using AlgoDemos.DynamicProg;
using AlgoDemos.ExpressionTree;
using AlgoDemos.Graph;
using AlgoDemos.Graph.AdjList;
using AlgoDemos.Graph.Connectivity;
using AlgoDemos.ints;
using AlgoDemos.Lists;
using AlgoDemos.Miscel;
using AlgoDemos.MST.KruskalsAlgo;
using AlgoDemos.String;
using System;
using System.Diagnostics;

namespace AlgoDemos
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            /*
            var expTree = new Etree();
            string postfix = "45+3/7*42/-";
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

            /*
            UndiWtedGraphKruskal g = new UndiWtedGraphKruskal();
            g.Demo();
            */

            /*
            PalindromeSubSequence ps = new PalindromeSubSequence();
            ps.Demo();
            */
            /*
            AlgoDemos.AStar.AstarAlgo astar = new();
            astar.Demo();
            */

            /*
            Stopwatch sw = new Stopwatch();
            sw.Start();
            AlgoDemos.PrimeTeleportationAlt.Solution.Demo();
            sw.Stop();
            Console.WriteLine($"Time taken: {sw.ElapsedMilliseconds} ms");
            sw.Start();
            AlgoDemos.PrimeTeleportation.Solution.Demo();
            sw.Stop();
            Console.WriteLine($"Time taken: {sw.ElapsedMilliseconds} ms");
            

            PalindromeSubstring.Demo();
            
            JumpGame7.Demo();
            
            AddNumbers.Demo();
            
            ReverseNumbers.Demo();
            
            PhoneCombinations.Demo();
            
            LRUCache.Demo();
            */

            FourSumFailed.Demo();
            Console.Write("Press any key to continue ...");
            Console.Read();
            



        }
    }
}
