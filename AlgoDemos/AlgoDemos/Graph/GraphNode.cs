using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoDemos.Graph
{
    public class GraphNode<T>
    {
        public int Level { get; private set; }
        public string nodeIdentifier {get; private set; }
        public GraphNode(T data, int level)
        {
            this.Data = data;
            this.Level = level;
            nodeIdentifier = data.ToString() + level.ToString();
        }
        public T Data { get; private set; }
        public List<GraphNode<T>> ChildNodes { get; set; }
    }
}
