using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoDemos.Graph.Connectivity
{
    class Vertex
    {
        public string name;
        public int state;
        public int componentNumber;

        public Vertex(string name)
        {
            this.name = name;
        }

    }
}
