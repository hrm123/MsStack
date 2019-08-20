using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoDemos.DijkstrasAlgo
{
    class DiWtedGraphShortestPath
    {
        public int MAX_VERTICES { get; }
        int n; // 0 
        int e; // 0 default
        int[,] adj; // false default
        Vertex[] vertexList;

        private readonly int TEMPORARY = 1;
        private readonly int PERMANENT = 2;
        private readonly int NIL = -1;
        private readonly int INFINITY = 999999999;

        public DiWtedGraphShortestPath()
        {
            MAX_VERTICES = 30; // default
            adj = new int[MAX_VERTICES, MAX_VERTICES];
            vertexList = new Vertex[MAX_VERTICES];
        }
        
        public int Vertices => n;
        public int Edges => e;


        private void InitializeMatrix(int s)
        {
            int v, c;
            for (v = 0; v < n; v++)
            {
                vertexList[v].status = TEMPORARY;
                vertexList[v].pathLength = INFINITY;
                vertexList[v].predecessor = NIL;
            }

            vertexList[s].pathLength = 0; // source vertex path length is 0
        }

        private void Dijk(int s)
        {
            int v, current;
            InitializeMatrix(s);

            while (true)
            {
                current = TempVertexWithMinPathLength();
                if (current == NIL)
                    return;
                vertexList[current].status = PERMANENT;
                for(v=0; v < n; v++)
                {
                    if(IsAdjacent(current,v) && vertexList[v].status == TEMPORARY)
                    {
                        if(vertexList[current].pathLength + adj[current,v] < vertexList[v].pathLength)
                        {
                            vertexList[v].predecessor = current;
                            vertexList[v].pathLength = vertexList[current].pathLength + adj[current, v];
                        }
                    }
                }
            }

        }

        private int TempVertexWithMinPathLength()
        {
            int min = INFINITY;
            int x = NIL;
            for(int v = 0; v < n; v++)
            {
                if(vertexList[v].status == TEMPORARY && vertexList[v].pathLength < min)
                {
                    min = vertexList[v].pathLength;
                    x = v;
                }
            }
            return x;
        }


        public void FindPaths(String source)
        {
            int s = GetIndex(source);
            Dijk(s);
            Console.WriteLine("Source Vertex : " + source + "\n");
            for (int v = 0; v < n; v++)
            {
                Console.WriteLine("Destination Vertex : " + vertexList[v].name + "\n");
                if (vertexList[v].pathLength == INFINITY)
                {
                    Console.WriteLine("There is no path from " + source  + " to vertex " + vertexList[v].name + "\n");
                }
                else
                {
                    FindPath(s, v);
                }
            }
        }

        private void FindPath(int s, int v)
        {
            int i, u, shortestDistance = 0, numVerticesInSP =0;
            int[] shortestPath = new int[n];

            while (v != s)
            {
                numVerticesInSP++;
                shortestPath[numVerticesInSP] = v;
                u = vertexList[v].predecessor;
                shortestDistance += adj[u, v];
                v = u;
            }
            numVerticesInSP++;
            shortestPath[numVerticesInSP] = s;
            Console.Write("Shortest Path is : ");
            for(i = numVerticesInSP; i >= 1; i--)
            {
                Console.Write(shortestPath[i] + " ");
            }
            Console.WriteLine("\n Shortest distance is : " + shortestDistance + "\n");
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

        public bool IsAdjacent(int u, int v)
        {
            return (adj[u, v] != 0);
        }

        public bool EdgeExists(string s1, string s2)
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
            DiWtedGraphShortestPath g = new DiWtedGraphShortestPath();
            int choice;
            String s1, s2;
            g.InsertVertex("Zero");
            g.InsertVertex("One");
            g.InsertVertex("Two");
            g.InsertVertex("Three");
            g.InsertVertex("Four");
            g.InsertVertex("Five");
            g.InsertVertex("Six");
            g.InsertVertex("Seven");
            g.InsertVertex("Eight");

            g.InsertEdge("Zero", "Three",2);
            g.InsertEdge("Zero", "One", 5);
            g.InsertEdge("Zero", "Four", 8);
            g.InsertEdge("One", "Four", 2);
            g.InsertEdge("Two", "One", 3);
            g.InsertEdge("Two", "Five", 4);
            g.InsertEdge("Three", "Four", 7);
            g.InsertEdge("Three", "Six", 8);
            g.InsertEdge("Four", "Five", 9);
            g.InsertEdge("Four", "Seven", 4);
            g.InsertEdge("Five", "One", 6);
            g.InsertEdge("Six", "Seven", 9);
            g.InsertEdge("Seven", "Three", 5);
            g.InsertEdge("Seven", "Five", 3);
            g.InsertEdge("Seven", "Eight", 5);
            g.InsertEdge("Eight", "Five", 3);

            g.FindPaths("Zero");

        }

    }
}
