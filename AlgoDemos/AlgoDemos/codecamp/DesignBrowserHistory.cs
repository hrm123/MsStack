using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    /// <summary>
    /// 1472. leetcode - 1 minute to understand. 20 minutes to code basic. 12 minutes to verify and fix one typo.
    /// 268 ms - beats 62% c# users. 124 MB - beats 6% c# users
    /// </summary>
    public class BrowserHistory
    {
        int MAX_CHUNK = 10000;
        string[] history;
        int empty = 1;
        int current = 0;

        public BrowserHistory(string homepage)
        {
            history = new string[MAX_CHUNK];
            history[0] = homepage;
        }

        public void Visit(string url)
        {
            if (empty >= MAX_CHUNK - 1)
            {
                //reallocate & update MAX_CHUNK
                MAX_CHUNK *= 2;
                string[] historyNew = new string[MAX_CHUNK];
                Array.Copy(history, historyNew, MAX_CHUNK / 2);
                history = historyNew;
            }

            current = current + 1;
            history[current] = url;
            empty = current + 1;
        }

        public string Back(int steps)
        {
            if (current - steps < 0)
            {
                current = 0;
                return history[0];
            }
            else
            {
                current = current - steps;
                return history[current];
            }
        }

        public string Forward(int steps)
        {
            if (current + steps >= empty)
            {
                current = empty - 1;
                return history[empty - 1];
            }
            else
            {
                current = current + steps;
                return history[current];
            }
        }
    }

    /**
     * Your BrowserHistory object will be instantiated and called as such:
     * BrowserHistory obj = new BrowserHistory(homepage);
     * obj.Visit(url);
     * string param_2 = obj.Back(steps);
     * string param_3 = obj.Forward(steps);
     */
}
