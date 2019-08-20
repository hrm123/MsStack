using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoDemos.MST.KruskalsAlgo
{
    class UndiWtedGraphKruskal
    {
        public int MAX_VERTICES { get; }
        int n; // 0 
        int e; // 0 default
        int[,] adj; // false default
        Vertex[] vertexList;
        public UndiWtedGraphKruskal()
        {
            MAX_VERTICES = 30; // default
            adj = new int[MAX_VERTICES, MAX_VERTICES];
            vertexList = new Vertex[MAX_VERTICES];
        }
        

        public int Vertices => n;
        public int Edges => e;

        private readonly int TEMPORARY = 1;
        private readonly int PERMANENT = 2;
        private readonly int NIL = -1;
        private readonly int INFINITY = 999999999;

        private int getRootVertex(int v)
        {
            int root = v;
            while (vertexList[root].parent != NIL)
                root = vertexList[root].parent;
            return root;
        }

        public void Kruskals()
        {
            PriorityQueue pq = new PriorityQueue();

            int u, v;
            for (u = 0; u < n; u++)
            {
                for (v = 0; v < u; v++) //TODO : why only to v < u ?
                {
                    if(adj[u,v] != 0)
                    {
                        pq.Insert(new Edge(u, v, adj[u, v]));
                    }
                }
            }

            for(v = 0; v< n; v++)
            {
                vertexList[v].parent = NIL;
            }

            int v1, v2,
                r1 = NIL, //root of tree towhich v1 belongs
                r2 = NIL //root of tree towhich v2 belongs
                ;
            int edgesInTree = 0;
            int wtTree = 0;

            while( !pq.IsEmpty() && edgesInTree < n - 1)
            {
                Edge edge = pq.Delete(); //get an edge from pq
                v1 = edge.u;
                v2 = edge.v;

                r1 = getRootVertex(v1);
                r2 = getRootVertex(v2);

                if(r1 != r2) /* this means v1, v2 belongs to different trees - so Edge (v1, v2) is created so trres get conecnted */
                {
                    edgesInTree++;
                    Console.WriteLine(vertexList[v1].name + "->" + vertexList[v2].name);
                    wtTree += edge.wt;
                    vertexList[r2].parent = r1; // 2 trees are joined
                }
            }
            if(edgesInTree < n - 1)
            {
                throw new InvalidOperationException("Graph is not conencted, no MST created.");
            }

            Console.WriteLine("Weight of MST is " + wtTree);
        }

        public void Display()
        {
            for(int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(adj[i,j]  + " ");
                }
                Console.WriteLine();
            }
        }

        public void InsertVertex(String name)
        {
            vertexList[n++] = new Vertex(name);
        }

        private int GetIndex(String s) // n => row-n and col-n represent that vertex
        {
            for(int i=0;i<n; i++)
            {
                if (s.Equals(vertexList[i].name))
                {
                    return i;
                }
            }
            // String s1, s2;
            // bool resp = EdgeExists(s1,s2)

            // int u, v;
            // bool resp = IsAdjacent(u, v);

            throw new ApplicationException("Invalid index");
        }

        private bool IsAdjacent(int u, int v)
        {
            return (adj[u, v] != 0);
        }

        private bool EdgeExists(string s1, string s2)
        {
            return IsAdjacent(GetIndex(s1), GetIndex(s2));
        }

        public void InsertEdge(String s1, String s2, int wt)
        {
            int u = GetIndex(s1);
            int v = GetIndex(s2);

            if (u == v) throw new InvalidOperationException("Not a valid edge.");

            if(adj[u,v] != 0)
            {
                Console.WriteLine("Edge already exists.");
            }
            else
            {
                adj[u, v] = wt;
                adj[v, u] = wt;
                e++;
            }

        }

        public void DeleteEdge(String s1, String s2)
        {
            int u = GetIndex(s1);
            int v = GetIndex(s2);

            if (u == v) throw new InvalidOperationException("Not a valid edge.");

            if (adj[u, v] == 0)
            {
                Console.WriteLine("Edge not present.");
            }
            else
            {
                adj[u, v] = 0;
                adj[v, u] = 0;
                e--;
            }

        }


        public int Outdegree(String s)
        {
            int u = GetIndex(s);
            int outd = 0;
            for(int v=0; v <n; v++)
            {
                if (adj[u, v] != 0 ) outd++;
            }
            return outd;
        }

        //number of edges coming to a vertex
        public int Indegree(String s)
        {
            int u = GetIndex(s);
            int ind = 0;
            for (int v = 0; v < n; v++)
            {
                if (adj[v,u] != 0 ) ind++;  // iterate on column
            }
            return ind;
        }

        public void Demo()
        {
            UndiWtedGraphKruskal g1 = new UndiWtedGraphKruskal();

            g1.InsertVertex("Zero");
            g1.InsertVertex("One");
            g1.InsertVertex("Two");
            g1.InsertVertex("Three");
            g1.InsertVertex("Four");
            g1.InsertVertex("Five");
            g1.InsertVertex("Six");
            g1.InsertVertex("Seven");
            g1.InsertVertex("Eight");
            g1.InsertVertex("Nine");
            

            g1.InsertEdge("Zero", "One", 19);
            g1.InsertEdge("Zero", "Three", 14);
            g1.InsertEdge("Zero", "Four", 12);
            g1.InsertEdge("One", "Two", 20);
            g1.InsertEdge("One", "Four", 18);
            g1.InsertEdge("Two", "Four", 17);
            g1.InsertEdge("Two", "Five", 15);
            g1.InsertEdge("Two", "Nine", 29);
            g1.InsertEdge("Three", "Four", 13);
            g1.InsertEdge("Three", "Six", 28);
            g1.InsertEdge("Four", "Five", 16);
            g1.InsertEdge("Four", "Six", 21);
            g1.InsertEdge("Four", "Seven", 22);
            g1.InsertEdge("Four", "Eight", 24);
            g1.InsertEdge("Five", "Eight", 26);
            g1.InsertEdge("Five", "Nine", 27);
            g1.InsertEdge("Six", "Seven", 23);
            g1.InsertEdge("Six", "Eight", 30);
            g1.InsertEdge("Eight", "Nine", 35);
            g1.Kruskals();


        }

    }
}
