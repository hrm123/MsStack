using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoDemos.DijkstrasAlgo
{
    class Vertex
    {
        public string name { get; set; }
        public int status { get; set; }
        public int predecessor { get; set; }
        public int pathLength { get; set; }


        public Vertex(string name)
        {
            this.name = name;
        }
    }
}
