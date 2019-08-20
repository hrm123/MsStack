using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoDemos.MST.KruskalsAlgo
{
    /// <summary>
    /// Stores the edge obejcts based on weight of edge as priority
    /// </summary>
    class PriorityQueue
    {
        private QueueNode front;

        public PriorityQueue()
        {
            front = null;
        }

        public void Insert(Edge e)
        {
            QueueNode temp, p;
            temp = new QueueNode(e);
            if (IsEmpty() || e.wt < front.info.wt)
            {
                temp.link = front;
                front = temp;
            }
            else
            {
                p = front;
                while (p.link != null && p.link.info.wt <= e.wt)
                {
                    p = p.link;
                }
                temp.link = p.link;
                p.link = temp;

            }
        }
    
        public Edge Delete()
        {
            Edge e;
            if (IsEmpty())
                throw new InvalidOperationException("Queue Underflow");
            else
            {
                e = front.info;
                front = front.link;
            }
            return e;
        }

        public bool IsEmpty()
        {
            return front == null;
        }
    }
}
