using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoDemos.Graph.AdjList
{
    class VertexNode
    {
        public String name;
        public VertexNode nextVertex;
        public EdgeNode firstEdge;
        public bool isVisited;
        public bool isProcessed;
        public VertexNode(String s)
        {
            name = s;
        }
    }
}
