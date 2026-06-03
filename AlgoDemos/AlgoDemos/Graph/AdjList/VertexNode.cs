using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoDemos.Graph.AdjList
{
    class VertexNode
    {
        public string name;
        public VertexNode nextVertex;
        public EdgeNode firstEdge;

        public VertexNode(string s)
        {
            name = s;
        }
    }
}
