using AlgoDemos.ExpressionTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.Miscel
{
    /// <summary>
    /// 40 ms beats 26.6%. 175.56MB beats 20.66%
    /// Improvements - Add dummy head & tail nodes both with key =-1, value =-1. This makes handling edge cases easier since all node addition/removal is same code now.
    /// </summary>
    public class LRUCacheAccepted1
    {
        int _capacity;
        Dictionary<int, Node> _valToIndexMap;
        Node _start = null;
        Node _end = null;
        int _remainingCapacity;

        public LRUCacheAccepted1(int capacity)
        {
            _capacity = capacity;
            _valToIndexMap = new Dictionary<int, Node>();
            _remainingCapacity = _capacity;
        }

        public int Get(int key)
        {
            // if key is not present in _valToIndexMap return -1
            if (!_valToIndexMap.ContainsKey(key))
            {
                return -1;
            }

            Node current = _valToIndexMap[key];
            RemoveNode(current);
            AddAtEnd(current.Key, current.Value);
            return current.Value;
        }

        private void AddAtEnd(int key, int val)
        {
            // add new node at end and change the pointers and the _end
            Node newNode = new Node(key, val);
            if (_end == null) // no nodes yet
            {
                _end = newNode;
                _start = newNode;
                _end.Next = null;
                _start.Prev = null;
            }
            else if (_end == _start) // only one node
            {
                _end = newNode;
                _end.Prev = _start;
                _end.Next = null;
                _start.Next = _end;
            }
            else
            {
                _end.Next = newNode;
                newNode.Prev = _end;
                _end = newNode;
            }
            _valToIndexMap[key] = newNode;
            _remainingCapacity--;
            return;
        }
        private void RemoveAtStart()
        {
            if (_start == null)
            {
                return;
            }
            // change the _start to the next node and change the pointers accordingly
            Node _temp = _start;
            //change the start node
            if (_start.Next != null)
            {
                _start = _start.Next;
                _start.Prev = null;
            }
            if (_start.Next == null) //earlier end node has been made start node
            {
                _end = _start; // ?
            }
            //delete existing start node
            _temp.Next = null;
            _temp.Prev = null;
            _valToIndexMap.Remove(_temp.Key);
            _remainingCapacity++;
        }

        private void RemoveNode(Node node)
        {
            int nodeKey = node.Key;
            Node nextNode = node.Next;
            Node prevNode = node.Prev;
            if (prevNode != null)
            {
                prevNode.Next = nextNode;
            }
            else
            {
                _start = nextNode;
            }
            if (nextNode != null)
            {
                nextNode.Prev = prevNode;
            }
            else
            {
                _end = prevNode;
            }
            _valToIndexMap.Remove(nodeKey);
            _remainingCapacity++;
        }


        public void Put(int key, int value)
        {
            if (_valToIndexMap.ContainsKey(key))
            {
                Node current = _valToIndexMap[key];
                RemoveNode(current);
                AddAtEnd(key, value);
                return;
            }

            // key is not present in _valToIndexMap -
            // if _remainingCapacity!=0
            // (1) create a new Node at end of the _list for this key and put it in approriate place in _list.
            // (2) put the key and the node in _valToIndexMap
            // (3) reduce the _remainingCapacity by 1
            // if _remainingCapacity==0
            // (1) remove the node at start of the list and add new node to end of the _list
            // (2) remove the key corresponding to the node removed from _valToIndexMap and add the new key and node to _valToIndexMap


            if (_remainingCapacity != 0)
            {
                AddAtEnd(key, value);
            }
            else
            {
                RemoveAtStart();
                AddAtEnd(key, value);
            }
        }

        public static List<string> MakeCalls(string[] actions, int[][] vals)
        {
            List<string> responses = new List<string>();
            LRUCacheAccepted1 cache = null;
            for (int i = 0; i < actions.Length; i++)
            {
                string action = actions[i];
                int[] val = vals[i];
                if (action == "LRUCache")
                {
                    cache = new LRUCacheAccepted1(val[0]);
                    responses.Add("null");
                }
                else if (action == "put")
                {
                    cache.Put(val[0], val[1]);
                    responses.Add("null");
                }
                else if (action == "get")
                {
                    int response = cache.Get(val[0]);
                    responses.Add(response.ToString());
                }
            }
            return responses;
        }


        public static void Demo()
        {
            string[] actions = ["LRUCache", "put", "put", "get", "put", "get", "put", "get", "get", "get"];
            int[][] vals = [[2], [1, 1], [2, 2], [1], [3, 3], [2], [4, 4], [1], [3], [4]];
            //// Console.WriteLine($"[{string.Join(",",MakeCalls(actions, vals).ToArray())}] to be [null,null,null,1,null,-1,null,-1,3,4]");
            actions = ["LRUCache", "put", "get"];
            vals = [[1], [2, 1], [1]];
            //// Console.WriteLine($"[{string.Join(",", MakeCalls(actions, vals).ToArray())}] to be [null, null, -1]");

            actions = ["LRUCache", "put", "put", "get", "put", "get", "put", "get", "get", "get"];
            vals = [[2], [1, 0], [2, 2], [1], [3, 3], [2], [4, 4], [1], [3], [4]];
            //// Console.WriteLine($"[{string.Join(",", MakeCalls(actions, vals).ToArray())}] to be [null,null,null,0,null,-1,null,-1,3,4]");

            actions = ["LRUCache", "put", "get", "put", "get", "get"];
            vals = [[1], [2, 1], [2], [3, 2], [2], [3]];
            //// Console.WriteLine($"[{string.Join(",", MakeCalls(actions, vals).ToArray())}] to be [null,null,1,null,-1,-1]");

            actions = ["LRUCache", "put", "put", "put", "put", "get", "get"];
            vals = [[2], [2, 1], [1, 1], [2, 3], [4, 1], [1], [2]];
            //// Console.WriteLine($"[{string.Join(",", MakeCalls(actions, vals).ToArray())}] to be [null,null,null,null,null,-1,3]");

        }
    }
}
