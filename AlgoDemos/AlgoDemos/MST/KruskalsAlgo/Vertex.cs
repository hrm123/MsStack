using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoDemos.MST.KruskalsAlgo
{
    class Vertex
    {
        public String name;
        public int parent;

        public Vertex(String name)
        {
            this.name = name;
        }
    }
}
