using AlgoDemos.ExpressionTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.Miscel
{
    public class Node
    {
        public Node Next { get; set; }
        public Node Prev { get; set; }
        public int Value { get;  set; }
        public int Key { get; set; }

        public Node(int key, int val, Node nxt = null, Node prv = null) 
        { 
            Value = val;
            Next = nxt;
            Prev = prv;
            Key = key;
        }
    }
    public class LRUCache
    {
        int _capacity;
        Dictionary<int,Node> _valToIndexMap;
        Node _start = null;
        Node _end = null;
        int _remainingCapacity;

        public LRUCache(int capacity)
        {
            _capacity = capacity;
            _valToIndexMap = new Dictionary<int,Node>();
            _remainingCapacity = _capacity;
        }

        public int Get(int key)
        {
            // if key is not present in _valToIndexMap return -1
            if (!_valToIndexMap.ContainsKey(key))
            {
                // Console.WriteLine($"{key} is not present in dictionary");
                return -1;
            }

            Node current = _valToIndexMap[key];
            int valueReturn = current.Value;
            RemoveNode(current);
            AddAtEnd(current.Key, current.Value);
            // Console.WriteLine($"After getting key ={current.Key},value ={current.Value}");
            // PrintCache();
            return valueReturn;
        }

        /*
        private void AddAtStart(int val)
        {
            // Make sure that val does not exist in list anywhere else
            // else if val already exists in list at start then return
            if (_valToIndexMap.ContainsKey(val))
            {
                if (_valToIndexMap[val] != _start)
                {
                    throw new Exception("value being added at start is alrady present elsewhere");
                }
                else
                {
                    return; // nothing to change
                }
            }
            Node newNode = new Node(val);
            _valToIndexMap[val] = newNode;
            Node temp = _start;
            _start = newNode;
            _start.Next = temp.Next;
            _start.Prev = null;
            if (temp.Next != null)
            {
                temp.Next.Prev = _start;
            }
            _valToIndexMap.Remove(temp.Value);
            _remainingCapacity--;
        }
        */

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
            if(_start == null)
            {
                // Console.WriteLine($"Removing at start - start is null");
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
            if(_start.Next == null) //earlier end node has been made start node
            {
                _end = _start; // ?
            }
            //delete existing start node
            _temp.Next = null;
            _temp.Prev = null;
            _valToIndexMap.Remove(_temp.Key);
            _remainingCapacity++;
            // Console.WriteLine($"After removing at start ");
            // PrintCache();
        }

        /*
        private void RemoveAtEnd()
        {
            Node _temp = _end;
            //change the end node
            _end = _end.Prev;
            _end.Next = null;
            //delete existing end node
            _temp.Next = null;
            _temp.Prev = null;
            _valToIndexMap.Remove(_temp.Value);
            _remainingCapacity++;
        }
        */

        private void RemoveNode(Node node)
        {
            int nodeKey = node.Key;
            Node nextNode = node.Next;
            Node prevNode = node.Prev;
            if(prevNode != null)
            {
                prevNode.Next = nextNode;
            }
            else
            {
                _start = nextNode;
            }
            if(nextNode != null)
            {
                nextNode.Prev = prevNode;
            }
            else
            {
                _end = prevNode;
            }
            _valToIndexMap.Remove(nodeKey);
            _remainingCapacity++;
            // Console.WriteLine($"After removing (key={node.Key},value={node.Value}); ");
            // PrintCache();
        }

        /*
        void PrintCache()
        {
            Node temp = _start;
            List<string> tempList = new List<string>();
            if (temp == null)
            {
                // Console.WriteLine("Cache is empty");
                return;
            }
            while (temp != null)
            {
                tempList.Add($"[{temp.Key},{temp.Value}]");
                temp = temp.Next;               
            }
            // Console.WriteLine(string.Join(" ", tempList));
        }
        */

        public void Put(int key, int value)
        {
            if (_valToIndexMap.ContainsKey(key))
            {
                Node current = _valToIndexMap[key];

                // if value has not changed still we update the position in list
                
                RemoveNode(current);
                AddAtEnd(key,value);
                
                // Console.WriteLine($"After modifying (key={key},value={value}); ");
                // PrintCache();
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
            // Console.WriteLine($"After putting (key={key},value={value}); " );
            // PrintCache();
        }

        public static List<string> MakeCalls(string[] actions, int[][] vals)
        {
            List<string> responses = new List<string>();
            LRUCache cache = null;
            for (int i = 0; i < actions.Length; i++)
            {
                string action = actions[i];
                int[] val = vals[i];
                if (action == "LRUCache")
                {
                    cache = new LRUCache(val[0]);
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
