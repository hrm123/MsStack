using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoDemos.Graph.Connectivity
{
    class UnDiGraphConComp
    {
        public int MAX_VERTICES { get; }
        int n; // 0 
        int e; // 0 default
        bool[,] adj; // false default
        Vertex[] vertexList;
        public UnDiGraphConComp()
        {
            MAX_VERTICES = 30; // default
            adj = new bool[MAX_VERTICES, MAX_VERTICES];
            vertexList = new Vertex[MAX_VERTICES];
        }
        
        public int Vertices => n;
        public int Edges => e;

        private readonly int INITIAL = 0;
        private readonly int WAITING = 1;
        private readonly int VISITED = 2;

        public bool IsConnected()
        {
            for (int v = 0; v < n; v++)
            {
                vertexList[v].state = INITIAL;
            }
            int cN = 1;
            BfsC(0, cN);

            for (int v = 0; v < n; v++)
            {
                if (vertexList[v].state == INITIAL)
                {
                    cN++;
                    BfsC(v, cN); // any unvisited vertex will be start vertex for new BFS with a new component number
                }
            }

            if(cN == 1) // only one component exists in graph
            {
                Console.WriteLine("Graph is connected \n");
                return true;
            }
            else
            {
                Console.WriteLine("Graph is not connected, it has " + cN + " connected components \n");
                for(int v = 0; v < n; v++)
                {
                    Console.WriteLine(vertexList[v].name + " " + vertexList[v].componentNumber);
                }
                return false;
            }
        }

        private void BfsC(int startVertex, int cN)
        {
            Queue<int> qu = new Queue<int>();
            qu.Enqueue(startVertex);
            vertexList[startVertex].state = WAITING;

            int v;
            while(qu.Count != 0)
            {
                v = qu.Dequeue();
                vertexList[v].state = VISITED;
                vertexList[v].componentNumber = cN;// this step is nto present in original Bfs
                for (int i = 0; i < n; i++)
                {
                    if(IsAdjacent(v,i) && vertexList[i].state == INITIAL )
                    {
                        qu.Enqueue(i);
                        vertexList[i].state = WAITING;
                    }
                }
            }
        }

        public void Display()
        {
            for(int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(adj[i,j] ? "1 " : "0 ");
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
            return adj[u, v];
        }

        public bool EdgeExists(string s1, string s2)
        {
            return IsAdjacent(GetIndex(s1), GetIndex(s2));
        }

        public void InsertEdge(String s1, String s2)
        {
            int u = GetIndex(s1);
            int v = GetIndex(s2);

            if (u == v) throw new InvalidOperationException("Not a valid edge.");

            if(adj[u,v] == true)
            {
                Console.WriteLine("Edge already exists.");
            }
            else
            {
                adj[u, v] = true;
                adj[v, u] = true;
                e++;
            }

        }

        public void DeleteEdge(String s1, String s2)
        {
            int u = GetIndex(s1);
            int v = GetIndex(s2);

            if (u == v) throw new InvalidOperationException("Not a valid edge.");

            if (adj[u, v] == false)
            {
                Console.WriteLine("Edge not present.");
            }
            else
            {
                adj[u, v] = false;
                adj[v, u] = false;
                e--;
            }

        }

        //number of edges coming to a vertex
        public int Degree(String s)
        {
            int u = GetIndex(s);
            int deg = 0;
            for(int v=0; v <n; v++)
            {
                if (adj[u, v]) deg++;
            }
            return deg;
        }
        
        public void Demo()
        {
            UnDiGraphConComp g = new UnDiGraphConComp();

            g.InsertVertex("Zero");
            g.InsertVertex("One");
            g.InsertVertex("Two");
            g.InsertVertex("Three");
            g.InsertVertex("Four");
            g.InsertVertex("Five");
            g.InsertVertex("Six");
            g.InsertVertex("Seven");
            g.InsertVertex("Eight");
            g.InsertVertex("Nine");

            g.InsertEdge("Zero", "One");
            g.InsertEdge("Zero", "Seven");
            g.InsertEdge("One", "Two");
            g.InsertEdge("One", "Four");
            g.InsertEdge("One", "Five");
            g.InsertEdge("Two", "Three");
            g.InsertEdge("Two", "Five");
            g.InsertEdge("Three", "Six");
            g.InsertEdge("Four", "Seven");
            g.InsertEdge("Five", "Six");
            g.InsertEdge("Five", "Seven");
            g.InsertEdge("Five", "Eight");
            g.InsertEdge("Six", "Nine");
            g.InsertEdge("Seven", "Eight");
            g.InsertEdge("Eight", "Nine");

            g.InsertEdge("Zero", "One");

            g.IsConnected();


            UnDiGraphConComp g1 = new UnDiGraphConComp();

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
            g1.InsertVertex("Ten");
            g1.InsertVertex("Eleven");
            g1.InsertVertex("Twelve");
            g1.InsertVertex("Thirteen");
            g1.InsertVertex("Fourteen");
            g1.InsertVertex("Fifteen");
            g1.InsertVertex("Sixteen");

            g1.InsertEdge("Zero", "One");
            g1.InsertEdge("Zero", "Two");
            g1.InsertEdge("Zero", "Three");
            g1.InsertEdge("One", "Three");
            g1.InsertEdge("Two", "Five");
            g1.InsertEdge("Three", "Four");
            g1.InsertEdge("Four", "Five");
            g1.InsertEdge("Six", "Seven");
            g1.InsertEdge("Six", "Eight");
            g1.InsertEdge("Seven", "Ten");
            g1.InsertEdge("Eight", "Nine");
            g1.InsertEdge("Nine", "Ten");
            g1.InsertEdge("Eleven", "Twelve");
            g1.InsertEdge("Eleven", "Fourteen");
            g1.InsertEdge("Eleven", "Fifteen");
            g1.InsertEdge("Twelve", "Thirteen");
            g1.InsertEdge("Thirteen", "Fourteen");
            g1.InsertEdge("Fourteen", "Sixteen");

            g1.IsConnected();
        }

    }
}
