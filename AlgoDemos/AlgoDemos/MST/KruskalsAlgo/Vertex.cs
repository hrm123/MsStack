using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoDemos.MST.KruskalsAlgo
{
    class Vertex
    {
        public string name;
        public int parent;

        public Vertex(string name)
        {
            this.name = name;
        }
    }
}
