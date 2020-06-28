using AlgoDemos.Graph.AdjList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgoDemos.puzzles
{
    /*
     * N buildings are there ids from 0 to N-1. Several employees have requested cubical move in the buildings. 
     * A request from building X to building Y is executed only if someone in building Y moves away 
     * to create a cube there (since all buildings are full).
     */
    public class OptimalTransfers
    {
        public void Demo()
        {
            List<Request> requests = new List<Request>();
            requests.Add(new Request
            {
                EmployeeName = "e1",
                FromBuilding = "11",
                ToBuilding = "22"
            });
            requests.Add(new Request
            {
                EmployeeName = "e2",
                FromBuilding = "22",
                ToBuilding = "33"
            });
            requests.Add(new Request
            {
                EmployeeName = "e3",
                FromBuilding = "33",
                ToBuilding = "44"
            });
            requests.Add(new Request
            {
                EmployeeName = "e4",
                FromBuilding = "44",
                ToBuilding = "11"
            });
            requests.Add(new Request
            {
                EmployeeName = "e5",
                FromBuilding = "22",
                ToBuilding = "45"
            });
            requests.Add(new Request
            {
                EmployeeName = "e6",
                FromBuilding = "45",
                ToBuilding = "22"
            });
            FindTransfers(requests);
        }

        public List<List<string>> FindTransfers(List<Request> requests)
        {
            // solution (logic based):
            //1. create graph (nodes are buildings, edges are transfers)
            //2. find cycles in the graph start at each node and should end at same node (for all nodes of the graph)
            //3. find largest cycle and store that and eliminate those edges of this large cycle from List<Requests>
            //4. repeat steps 1 to 3 till no more edges exist or no  cycles are created
            bool cyclesPresent = true;

            while (cyclesPresent)
            {
                LinkedDiGraph g = new LinkedDiGraph();
                var buildingNames = Enumerable.Range(0, 99).Select(iter => iter + "").ToList();
                buildingNames.ForEach(bName => g.AddVertex(bName));
                requests.ForEach(req =>
                {
                    g.AddEdge(req.FromBuilding, req.ToBuilding);
                });
                // g.Display();
                List<String> allFullCycles = g.FindFullCycles();
                Console.WriteLine("Full cycles :");
                allFullCycles.ForEach(cycle => Console.WriteLine(cycle));
                cyclesPresent = false;
            }
            List<List<string>> transfers = new List<List<string>>();



            return transfers;

        }
        public List<List<string>> FindTransfersRulebased(List<Request> requests)
        {
            // solution (rule based):

            List<List<string>> transfers = new List<List<string>>();
            return transfers;

        }


    }


    public class Request
    {
        public string EmployeeName { get; set; }
        public string FromBuilding { get; set; }
        public string ToBuilding { get; set; }
    }
}
