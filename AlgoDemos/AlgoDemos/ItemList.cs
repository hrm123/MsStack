using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos
{

    public class Point: ICloneable
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public static void Test()
        {
            Point point1 = new Point(21, 34);
            Point point2 = (Point) point1.Clone();
            point2.X = 55;
            Console.WriteLine(point1.X); //21
            Console.WriteLine(point2.X); //55
        }

        public object Clone()
        {
            return new Point(X, Y);
        }
    }


    public class Item : IItemList.IItem
    {
        string _id = string.Empty;
        string _tag = string.Empty;

        public Item(string id, string tag)
        {
            _id = id;
            _tag = tag;
        }
        public string GetID()
        {
            return _id;
        }

        public string GetTag()
        {
            return _tag;
        }
    }

    public class ItemList : IItemList
    {
        Dictionary<string, IItemList.IItem> _items = new Dictionary<string, IItemList.IItem>(); // O(n) memory
        Dictionary<string, List<string>> _keysWithSameTag = new Dictionary<string, List<string>>(); // O(n) memory


        internal static void TestAddItem()
        {
            ItemList itemStore = new ItemList();
            foreach (var identifier in Enumerable.Range(1, 10000))
            {
                itemStore.Put(new Item($"{identifier}", $"tag{identifier}"));
            }
            if(itemStore.Size() != 10000)
            {
                Console.WriteLine("TestAddItem failed");
            }
            else
            {
                Console.WriteLine("TestAddItem passed");
            }
        }



        internal static void TestFetchByID()
        {
            ItemList itemStore = new ItemList();
            foreach (var identifier in Enumerable.Range(1, 10000))
            {
                itemStore.Put(new Item($"{identifier}", $"tag{identifier}"));
            }
            var itm = itemStore.Get("1111");
            if ( itm != null && itm.GetID()=="1111" && itm.GetTag() == "tag1111")
            {
                Console.WriteLine("TestFetchByID passed");
            }
            else
            {
                Console.WriteLine("TestFetchByID failed");
            }
        }



        internal static void TestDropItems()
        {
            ItemList itemStore = new ItemList();
            foreach (var identifier in Enumerable.Range(1, 1000))
            {
                itemStore.Put(new Item($"{identifier}", $"tag{identifier%100}"));
            }
            itemStore.DropAllByTag("tag0"); // all of 100s and 1000 are exactly divided by 100 - so 10 items should be dropped
            if(itemStore.Size() != 990)
            {
                Console.WriteLine("TestDropItems failed");
            }
            else
            {
                Console.WriteLine("TestDropItems passed");
            }
        }

        public void DropAllByTag(string tag)
        {
            //O(n) worst case
            if (_keysWithSameTag.ContainsKey(tag))
            {
                foreach (var key in _keysWithSameTag[tag])
                {
                    _items.Remove(key);
                }
            }

        }

        public int Size()
        {
            // O(1)
            return _items.Count();
        }

        public IItemList.IItem Get(string id)
        {
            // O(1)
            if (_items.ContainsKey(id)) { 
                return _items[id];
            }
            else
            {
                return null;
            }
        }

        public void Put(IItemList.IItem item)
        {
            // O(1)
            _items.Add(item.GetID(), item);
            var tag = item.GetTag();
            if (! _keysWithSameTag.ContainsKey(tag))
            {
                _keysWithSameTag[tag] = new List<string>();
            }
            _keysWithSameTag[tag].Add(item.GetID());
        }
    }
}
