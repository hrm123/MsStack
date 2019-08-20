using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoDemos.MST.KruskalsAlgo
{
    class QueueNode
    {
        public Edge info;
        public QueueNode link;

        public QueueNode(Edge e)
        {
            info = e;
            link = null;
        }
    }
}
