using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoDemos.Graph.Connectivity
{
    class Vertex
    {
        public String name;
        public int state;
        public int componentNumber;

        public Vertex(String name)
        {
            this.name = name;
        }

    }
}
