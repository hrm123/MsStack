using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoDemos.Graph.AdjList
{
    class LinkedDiGraph
    {
        VertexNode start;
        int n;
        int e;

        public int Vertices => n;

        public int Edges => e;

        public void InsertVertex(String s)
        {
            VertexNode temp = new VertexNode(s);
            if (start != null)
            {
                VertexNode p = start;
                while (p.nextVertex != null)
                {
                    if (p.name.Equals(s))
                    {
                        Console.WriteLine("Vertex already present");
                        return;
                    }
                    p = p.nextVertex;
                }
                if (p.name.Equals(s))
                {
                    Console.WriteLine("Vertex already present");
                    return;
                }
                p.nextVertex = temp;
                
            }
            else
            {
                start = temp;
            }
            n++;
        }


        public void AddVertex(String s)
        {
            VertexNode temp = new VertexNode(s);
            if (start != null)
            {
                VertexNode p = start;
                while (p.nextVertex != null)
                {
                    if (p.name.Equals(s))
                    {
                        Console.WriteLine("Vertex already present");
                        return;
                    }
                    p = p.nextVertex;
                }
                if (p.name.Equals(s))
                {
                    Console.WriteLine("Vertex already present");
                    return;
                }
                p.nextVertex = temp;

            }
            else
            {
                start = temp;
            }
            n++;
        }

        public void DeleteVertex(String s)
        {
            DeletefromEdgeLists(s);
            DeletefromVertexList(s);
        }

        private void DeletefromVertexList(String s)
        {
            if(start == null)
            {
                Console.WriteLine("No vertices to be deleted.");
                return;
            }

            if(start.name.Equals(s)) // Vertex to be deleted is first vertex of list
            {
                for (EdgeNode q = start.firstEdge; q != null; q = q.nextEdge) e--;
                start = start.nextVertex;
                n--;
            }
            else
            {
                VertexNode p = start;
                while(p.nextVertex != null)
                {
                    if (p.nextVertex.name.Equals(s)) break;
                    p = p.nextVertex;
                }
                if(p.nextVertex == null)
                {
                    Console.WriteLine("vertex not found.");
                    return;
                }
                else
                {
                    for (EdgeNode q = p.nextVertex.firstEdge; q != null; q = q.nextEdge) e--;
                    p.nextVertex = p.nextVertex.nextVertex;
                    n--;
                }

            }
        }

        private void DeletefromEdgeLists(String s)
        {
            for(VertexNode p = start; p != null; p = p.nextVertex)
            {
                if (p.firstEdge == null)
                    continue;
                if (p.firstEdge.endVertex.name.Equals(s))
                {
                    p.firstEdge = p.firstEdge.nextEdge;
                    e--;
                }
                else
                {
                    EdgeNode q = p.firstEdge;
                    while(q.nextEdge != null)
                    {
                        if (q.nextEdge.endVertex.name.Equals(s))
                        {
                            q.nextEdge = q.nextEdge.nextEdge;
                        }
                    }
                }
            }
        }

        private VertexNode FindVertex(String s)
        {
            VertexNode p = start;
            while(p != null)
            {
                if (p.name.Equals(s)) return p;
                p = p.nextVertex;
            }
            return null;
        }

        public void InsertEdge(String s1, String s2)
        {
            if (s1.Equals(s2))
            {
                Console.WriteLine("Invalid edge - start and end vertices are same");
                return;
            }
            VertexNode u = FindVertex(s1);
            VertexNode v = FindVertex(s2);
            if(u == null || v == null)
            {
                Console.WriteLine("Start or End vertex not present");
                return;
            }

            EdgeNode temp = new EdgeNode(v);
            if(u.firstEdge == null)
            {
                u.firstEdge = temp;
                e++;
            }
            else
            {
                EdgeNode p = u.firstEdge;
                while(p.nextEdge != null)
                {
                    if (p.endVertex.name.Equals(s2))
                    {
                        Console.WriteLine("Edge present.");
                        return;
                    }
                    p = p.nextEdge;
                }
                if (p.endVertex.name.Equals(s2))
                {
                    Console.WriteLine("edge present.");
                    return;
                }
                p.nextEdge = temp;
                e++;
            }
        }

        public void AddEdge(String s1, String s2)
        {
            if (s1.Equals(s2))
            {
                Console.WriteLine("Invalid edge - start and end vertices are same");
                return;
            }
            VertexNode u = FindVertex(s1);
            VertexNode v = FindVertex(s2);
            if (u == null || v == null)
            {
                Console.WriteLine("Start or End vertex not present");
                return;
            }

            EdgeNode temp = new EdgeNode(v);
            if (u.firstEdge == null)
            {
                u.firstEdge = temp;
                e++;
            }
            else
            {
                EdgeNode p = u.firstEdge;
                while (p.nextEdge != null)
                {
                    if (p.endVertex.name.Equals(s2))
                    {
                        Console.WriteLine("Edge present.");
                        return;
                    }
                    p = p.nextEdge;
                }
                if (p.endVertex.name.Equals(s2))
                {
                    Console.WriteLine("edge present.");
                    return;
                }
                p.nextEdge = temp;
                e++;
            }
        }

        public void DeleteEdge(String s1, String s2)
        {
            VertexNode u = FindVertex(s1);
            if(u == null || u.firstEdge == null)
            {
                Console.WriteLine("Start vertex / edge not present.");
                return;
            }

            if (u.firstEdge.endVertex.name.Equals(s2))
            {
                u.firstEdge = u.firstEdge.nextEdge;
                e--;
                return;
            }

            EdgeNode q = u.firstEdge;
            while(q.nextEdge != null)
            {
                if (q.nextEdge.endVertex.name.Equals(s2))
                {
                    q.nextEdge = q.nextEdge.nextEdge;
                    e--;
                    return;
                }
                q = q.nextEdge;
            }
            Console.WriteLine("Edge not found.");
        }

        public void Display()
        {
            
            EdgeNode q;
            for (VertexNode p = start; p != null; p = p.nextVertex)
            {
                Console.Write(p.name + "=>");
                for (q = p.firstEdge; q != null; q = q.nextEdge)
                {
                    Console.Write(" " + q.endVertex.name);
                }
                Console.WriteLine();
            }
        }

        private void FindFullCyclesStartingAtNode(VertexNode startNode, VertexNode currentNode, 
            String pathTillNow, List<string> allFullCycles)
        {
            
            if (currentNode == null)
            {
                startNode.isVisited = true;
                // start of recursion
                for (EdgeNode en = startNode.firstEdge; en != null; en = en.nextEdge)
                {
                    FindFullCyclesStartingAtNode(startNode, en.endVertex, pathTillNow, allFullCycles);
                }
                return;
            }

            pathTillNow += "-" +  currentNode.name;
            if (currentNode.isVisited)
            { // full cycle found
                if (currentNode.name == startNode.name)
                {
                    // full cycle found
                    if (currentNode.isProcessed)
                    { // ignore this path since path that passes through this node already found out
                        return;
                    }
                    allFullCycles.Add(pathTillNow);
                    return;
                } else
                {
                    // this is a sub cycle that does nto end with start node .. so ignore this path
                    return;
                }
            }

            currentNode.isVisited = true;
        
            for (EdgeNode en = currentNode.firstEdge; en != null; en = en.nextEdge)
            {
                    FindFullCyclesStartingAtNode(startNode, en.endVertex, pathTillNow, allFullCycles);
            }

            currentNode.isVisited = false;

        }

        public List<string> FindFullCycles()
        {
            Dictionary<string, bool> visitedNodes = new Dictionary<string, bool>();
            for (VertexNode cur = start; cur != null; cur = cur.nextVertex)
            {
                cur.isProcessed = false;
            }

            List<string> allFullCycles = new List<string>();
            for (VertexNode cur = start; cur != null; cur = cur.nextVertex)
            {
                // dfs for each node with if current node is startnode then full cycle found .. if current 
                // node is null cycle not there
                cur.isProcessed = false;
                
                FindFullCyclesStartingAtNode(cur, null, cur.name, allFullCycles);
                cur.isProcessed = true;
            }
            return allFullCycles;
        }


        public bool EdgeExists(String s1, String s2)
        {
            VertexNode u = FindVertex(s1);
            EdgeNode curEdge = u.firstEdge;
            while(curEdge != null)
            {
                if (curEdge.endVertex.name.Equals(s2)) return true;
                curEdge = curEdge.nextEdge;
            }
            return false;
        }

        public int OutDegree(String s)
        {
            VertexNode u = FindVertex(s);
            if (u == null)
            {
                throw new System.InvalidOperationException("Invalid vertex");
            }
            int outd = 0;
            EdgeNode q = u.firstEdge;
            while (q != null)
            {
                q = q.nextEdge;
                outd++;
            }
            return outd;
        }

        public int InDegree(String s)
        {
            VertexNode u = FindVertex(s);
            if (u == null)
            {
                throw new System.InvalidOperationException("Invalid vertex");
            }
            int ind = 0;
            
            for (VertexNode p = start; p != null; p = p.nextVertex)
            {
                for(EdgeNode q = p.firstEdge; q != null; q = q.nextEdge)
                {
                    if (q.endVertex.name.Equals(s)) ind++;
                }
            }
            return ind;
        }


        public void Demo()
        {
            LinkedDiGraph g = new LinkedDiGraph();
            int choice;
            String s1, s2;

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1. Display Adjacency List");
                Console.WriteLine("2. Insert a vertex");
                Console.WriteLine("3. Insert an edge");
                Console.WriteLine("4. Delete an edge");
                Console.WriteLine("5. Display indegree and outdegree of vertex");
                Console.WriteLine("6. Check if edge exists between two vertices");
                Console.WriteLine("7. Exit");
                Console.WriteLine("8. Delete a vertex");
                Console.WriteLine("Enter your choice: ");
                choice = Convert.ToInt32(Console.ReadLine());
                if (choice == 7)
                    break;

                switch (choice)
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
                        Console.WriteLine("Indegree is : " + g.InDegree(s1));
                        Console.WriteLine("Outdegree is : " + g.OutDegree(s1));
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
                    case 8:
                        Console.WriteLine("Deelete a Vertex :");
                        s1 = Console.ReadLine();
                        try
                        {
                            g.DeleteVertex(s1);
                            Console.WriteLine("Vertex Deleted.");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Vertex Delete failed. Err: " + e.ToString());
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
