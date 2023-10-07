using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.Graph
{
    public class CityEvacuation
    {

        public static void TestCase()
        {
            string[] files = new string[]
            {
                "D:\\code\\MsftStack\\AlgoDemos\\AlgoDemos\\Graph\\evac_files\\01",
                "D:\\code\\MsftStack\\AlgoDemos\\AlgoDemos\\Graph\\evac_files\\02",
                "D:\\code\\MsftStack\\AlgoDemos\\AlgoDemos\\Graph\\evac_files\\03",
                "D:\\code\\MsftStack\\AlgoDemos\\AlgoDemos\\Graph\\evac_files\\24",
                "D:\\code\\MsftStack\\AlgoDemos\\AlgoDemos\\Graph\\evac_files\\28",

            };

            int[] answers = new int[] { 6, 20000, 0 , 29600 , 59410 };

            FastScanner fs = new FastScanner(files[4]);
            var details = fs.ReadFile();
            var ce = new CityEvacuation(details.Item1, details.Item2, details.Item3);
            var answer = ce.MaxFlow();
            Console.WriteLine($"{answer} should be {answers[4]}");
        }


        FlowGraph _fg;
        int _n;
        int _m;

        public CityEvacuation(int n, int m, FlowGraph fg)
        {
            _n = n;
            _m = m;
            _fg = fg;

        }

        private int DFS(int node, List<int> route, Dictionary<int, bool> visited, int min)
        {
            if(node == _n-1) { return min; }
            if(visited.ContainsKey(node))
            {
                return -1;
            }
            else
            {
                visited[node] = true;
            }
            var eis = _fg.getIds(node);
            foreach (var edgeIndex in eis)
            {
                var edge = _fg.getEdge(edgeIndex);
                if(edge != null && (! edge._flowStarted  || edge._flow != 0))
                {
                    if (visited.ContainsKey(edge._to))
                    {
                        continue;
                    }
                    route.Add(edgeIndex);
                    int minCapacity = DFS(edge._to, route, visited, Math.Min(edge._flow, min));
                    if(minCapacity != -1)
                    {
                        return minCapacity;
                    }
                    route.Remove(edgeIndex);
                }
            }
            return -1;
        }

        private Tuple<List<Edge>, int, List<int>> SourceToSink()
        {
            List<int> route = new List<int>();
            Dictionary<int, bool> visited = new Dictionary<int, bool>();
            int min = Int32.MaxValue;
            int minimum = DFS(0, route, visited, min);
            if(minimum == -1)
            {
                // no s-t path exists
                route.Clear();
            }
            List<Edge> routeEdges = new List<Edge>();
            foreach (var item in route)
            {
                routeEdges.Add(_fg.getEdge(item));
                
            }
            return new Tuple<List<Edge>, int, List<int>>(routeEdges, minimum, route);
        }

        private int MaxFlow()
        {
            var path = SourceToSink();
            
            while ( path.Item2 != -1)
            {
                Console.WriteLine($"s-t path = {String.Join(",", path.Item1.Select(itm => itm._from +"->" + itm._to + ":" + itm._flow ).ToArray())}");
                foreach (var edgeIndex in path.Item3)
                {
                    _fg.addFlow(edgeIndex, path.Item2);
                }
                Console.WriteLine($"s-t path = {String.Join(",", path.Item1.Select(itm => itm._from + "->" + itm._to + ":" + itm._flow ).ToArray())}");
                path = SourceToSink();
            }

            //since no more s-t path exists we get all backward edges at 0 and add them
            var minEdges = _fg.getIds(0);
            int total = 0;
            foreach(var edgeIndex in minEdges)
            {
                total += _fg.getBackwardEdge(edgeIndex)._flow;
            }

            return total;
        }
    }

    public class Edge
    {
        public int _from, _to, _capacity, _flow;
        public bool _flowStarted = false;

        public Edge(int from, int to, int capacity, int currentFlow)
        {
            _from = from;
            _to = to;
            _capacity = capacity;
            _flow = currentFlow;
        }
    }
    public class FlowGraph
    {
        private IList<Edge> _edges; /* List of all - forward and backward - edges */
        private IList<int>[] _adjacencyList; /* These adjacency lists store only indices of edges from the edges list */
        int _n;

        public FlowGraph(int n)
        {
            _adjacencyList =  new List<int>[n];
            _n = n;
            for (int i = 0; i < n; ++i)
                _adjacencyList[i] = new List<int>();
            _edges = new List<Edge>();
        }

        public void addEdge(int from, int to, int capacity)
        {
            /* Note that we first append a forward edge and then a backward edge,
             * so all forward edges are stored at even indices (starting from 0),
             * whereas backward edges are stored at odd indices. */
            Edge forwardEdge = new Edge(from, to, capacity, capacity);
            Edge backwardEdge = new Edge(to, from, 0, 0);
            _adjacencyList[from].Add(_edges.Count()); // here we are adding index of latest would be next created edge number
            _edges.Add(forwardEdge);
            
            _adjacencyList[to].Add(_edges.Count());
            _edges.Add(backwardEdge);
        }

        public int Size()
        {
            return _adjacencyList.Length;
        }

        public IList<int> getIds(int from)
        {
            return _adjacencyList[from];
        }

        public Edge getEdge(int id)
        {
            return _edges[id];
        }

        public Edge getBackwardEdge(int id)
        {
            return _edges[id ^ 1];
        }

        public void addFlow(int id, int flow)
        {
            /* To get a backward edge for a true forward edge (i.e id is even), we should get id + 1
             * due to the described above scheme. On the other hand, when we have to get a "backward"
             * edge for a backward edge (i.e. get a forward edge for backward - id is odd), id - 1
             * should be taken.
             *
             * It turns out that id ^ 1 works for both cases. Think this through! */
            _edges[id]._flow -= flow; // we subtract flow to current edge
            _edges[id]._flowStarted = true;
            // _edges[id]._capacity -= flow;
            _edges[id ^ 1]._flow += flow; // we add flow from backward edge of current edge
            _edges[id ^ 1]._flowStarted = true;
        }
    }

    public class FastScanner
    {
        string _fn;
        public FastScanner(string fileName)
        {
            _fn = fileName;   
        }

        public int[] ReadLine(string line)
        {
            var strs = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray();
            return strs.Select(x => int.Parse(x)).ToArray();

        }

        public Tuple<int, int, FlowGraph> ReadFile()
        {
            string[] strArray = File.ReadAllLines(_fn);
            int[] tmp = ReadLine(strArray[0]);
            int n =  tmp[0];
            int m =  tmp[1];
            FlowGraph fg = new FlowGraph(n);
            for (int i = 1; i < strArray.Length; i++)
            {
                tmp = ReadLine(strArray[i]);
                fg.addEdge(tmp[0]-1, tmp[1]-1, tmp[2]);
            }
            return new Tuple<int, int, FlowGraph>(n, m, fg);
        }

    }
}
