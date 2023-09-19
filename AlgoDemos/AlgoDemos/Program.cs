using AlgoDemos.bits;
using AlgoDemos.codecamp;
using AlgoDemos.codecamp.arrays;
using AlgoDemos.codecamp.strings;
using AlgoDemos.DijkstrasAlgo;
using AlgoDemos.ExpressionTree;
using AlgoDemos.Graph;
using AlgoDemos.Graph.AdjList;
using AlgoDemos.Graph.Connectivity;
using AlgoDemos.MST.KruskalsAlgo;
using System;
using System.Drawing;

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

            //EditDistance ed = new EditDistance();
            //Console.WriteLine(ed.MinDistance("horse", "ros")); 

            //JumpGame7 jg = new JumpGame7();
            //jg.CanReach(null,1,1);

            // CoinChangeSoln cc = new CoinChangeSoln();
            // int[] coins = new int[] { 411, 412, 413, 414, 415, 416, 417, 418, 419, 420, 421, 422 };
            // int amount = 9864; // expected soln = 24
            // int answer = cc.CoinChange(coins, amount);


            // SegmentTree.testCase();

            //PrimeNumbers.TestCase_GetPrimeDivisors();

            //AhoKorasick.Testcase();

            // BitArrayDemo.TestCase_Xor();

            //MinSwapsAcsArray.testCase();

            // kincreasing.Testcase();

            // MergeSorter sorter = new MergeSorter();
            // sorter.MergeSort(new[] { 10, 9, 2, 5, 3, 7, 101, 18 });

            LIS.TestLIS();

            Console.ReadKey();
        }
    }
}
