using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace AlgoDemos.puzzles
{
    /// <summary>
    /// Given N persons N tasks and C(a,b) which is cost of assigning person a to task b
    /// calculate optimal assignment (minimum cost).
    /// </summary>
    public class PersonTaskAssigment
    {
        int _num;

        bool[] personBusy = null;
        bool[] taskAssigned = null;
        Dictionary<string, int> pathCostMap = new Dictionary<string, int>();
        const int FREE = 0;
        const int BUSY = 1;
        const int ASSIGNED = 0;
        const int UNASSIGNED = 1;
        int minCost;
        string minPath = "";
        int[][] costMatrix = null;

        public PersonTaskAssigment(int N, int[][] costsMatrix)
        {
            _num = N;
            personBusy = new bool[N];
            taskAssigned = new bool[N];
            for (int iter = 0; iter < _num; iter++)
            {
                personBusy[iter] = false;
                taskAssigned[iter] = false;
            }

            if(costsMatrix == null)
            {
                // TODO add random costs
            }
            else
            {
                costMatrix = costsMatrix;
            }
        }


        /// <summary>
        /// recursive function to brute force cal optimal assignment
        /// </summary>
        /// <param name="level">start with 0 level</param>
        /// <param name="path">start with empty</param>
        /// <param name="cost">start with 0 cost </param>
        public void PersonTaskAssigmentBruteForce(int level, string path, int cost)
        {
            if(level == _num - 1)
            {
                return; // recursion termination
            }

            if (cost > minCost) // no need to pursue further along this path
            {
                return; // recursion termination
            }

            for (int person = 0; person < _num; person++) // for all persons
            {
                // current person is now busy
                personBusy[person] = true;
                for (int task = 0; task < _num; task++) // for all tasks
                {
                    if (taskAssigned[task] == false)
                    {
                        //assign unassigned task  to person
                        taskAssigned[task] = true;

                        //allow next task for next person
                        PersonTaskAssigmentBruteForce(level++, path + (person + task), cost + costMatrix[person][task]);

                        taskAssigned[task] = false; // release this task and move to next task
                    }
                }
                // no more tasks for current person
                personBusy[person] = false;
            }


            // get min cost from all the stored paths

        }

        int getCost(int person, int task)
        {
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>returns -1 if not unassigned tasks else returns location of the task in array</returns>
        int getNextUnassignedTask() 
        {
            return -1;
        }

    }
}
