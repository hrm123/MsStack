using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoDemos.Graph.AdjList
{
    class EdgeNode
    {
        public VertexNode endVertex;
        public EdgeNode nextEdge;

        public EdgeNode(VertexNode v)
        {
            endVertex = v;

        }
    }
}
