using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoDemos.DijkstrasAlgo
{
    class Vertex
    {
        public String name { get; set; }
        public int status { get; set; }
        public int predecessor { get; set; }
        public int pathLength { get; set; }


        public Vertex(String name)
        {
            this.name = name;
        }
    }
}
