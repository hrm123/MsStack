using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoDemos.MST.KruskalsAlgo
{
    class Edge
    {
        public int u; //start vertex
        public int v; //end vertex
        public int wt;

        public Edge(int startV, int endV, int weight)
        {
            u = startV;
            v = endV;
            wt = weight;
        }

    }
}
