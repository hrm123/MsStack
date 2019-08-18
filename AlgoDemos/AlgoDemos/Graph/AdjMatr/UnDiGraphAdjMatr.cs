using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoDemos.Graph
{
    class UnDiGraphAdjMatr
    {
        public int MAX_VERTICES { get; }
        int n; // 0 
        int e; // 0 default
        bool[,] adj; // false default
        Vertex[] vertexList;
        public UnDiGraphAdjMatr()
        {
            MAX_VERTICES = 30; // default
            adj = new bool[MAX_VERTICES, MAX_VERTICES];
            vertexList = new Vertex[MAX_VERTICES];
        }
        
        public int Vertices => n;
        public int Edges => e;

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
                if (s.Equals(vertexList[i].Name))
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
            DiGraphAdjMatr g = new DiGraphAdjMatr();
            int choice;
            String s1, s2;

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1. Display Adjacency Matrix");
                Console.WriteLine("2. Insert a vertex");
                Console.WriteLine("3. Insert an edge");
                Console.WriteLine("4. Delete an edge");
                Console.WriteLine("5. Display indegree and outdegree of vertex");
                Console.WriteLine("6. Check if edge exists between two vertices");
                Console.WriteLine("7. Exit");
                Console.WriteLine("Enter your choice: ");
                choice = Convert.ToInt32(Console.ReadLine());
                if (choice == 7)
                    break;

                switch(choice)
                {
                    case 1:
                        g.Display();
                        Console.WriteLine("Vertices = " + g.Vertices);
                        Console.WriteLine("Edges = " + g.Edges);
                        break;
                    case 2:
                        Console.WriteLine("Insert a Vertex :");
                        s1 = Console.ReadLine();
                        try
                        {
                            g.InsertVertex(s1);
                            Console.WriteLine("Vertex Inserted.");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Vertex Insert failed. Err: " + e.ToString());
                        }
                        break;
                    case 3:
                        Console.WriteLine("Insert start vertex of the Edge :");
                        s1 = Console.ReadLine();
                        Console.WriteLine("Insert end vertex of the Edge :");
                        s2 = Console.ReadLine();
                        try
                        {
                            g.InsertEdge(s1, s2);
                            Console.WriteLine("Edge Inserted.");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Edge Insert failed. err: " + e.ToString());
                        }
                        break;
                    case 4:
                        Console.WriteLine("Insert start vertex of the Edge :");
                        s1 = Console.ReadLine();
                        Console.WriteLine("Insert end vertex of the Edge :");
                        s2 = Console.ReadLine();
                        try
                        {
                            g.DeleteEdge(s1, s2);
                            Console.WriteLine("Edge Deleted.");
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Edge Delete failed. Err:" + e.ToString());
                        }
                        break;
                    case 5:
                        Console.WriteLine("Enter a vertex: ");
                        s1 = Console.ReadLine();
                        Console.WriteLine("Indegree is : " + g.Indegree(s1));
                        Console.WriteLine("Outdegree is : " + g.Outdegree(s1));
                        break;
                    case 6:
                        Console.WriteLine("Enter start vertex of Edge: ");
                        s1 = Console.ReadLine();
                        Console.WriteLine("Enter end vertex of Edge: ");
                        s2 = Console.ReadLine();
                        if (g.EdgeExists(s1, s2))
                        {
                            Console.WriteLine("There is an edge from " + s1 + " to " + s2);
                        }
                        else
                        {
                            Console.WriteLine("There is no edge from " + s1 + " to " + s2);
                        }
                        if (g.EdgeExists(s2, s1))
                        {
                            Console.WriteLine("There is an edge from " + s1 + " to " + s2);
                        }
                        else
                        {
                            Console.WriteLine("There is no edge from " + s1 + " to " + s2);
                        }
                        break;
                    default:
                        Console.WriteLine("Wrong choice.");
                        break;
                }

            }


        }

    }
}
