using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.Lists
{
  
  
    public class ListNode {
        public int val;
        public ListNode next;
        public ListNode(int val=0, ListNode next=null) {
            this.val = val;
            this.next = next;
        }

        public ListNode(LinkedList<int> list)
        {
            if(list.Count == 0)
            {
                return;
            }
            ListNode currentNode = this;
            var enumerator = list.GetEnumerator();
            int indx = 0; 
            int total = list.Count;
            while (enumerator.MoveNext())
            {
                var currentLLNode = enumerator.Current;
                currentNode.val = currentLLNode;
                indx++;
                if(indx == total) 
                {
                    break;
                }
                currentNode.next = new ListNode();
                currentNode = currentNode.next;
            }
        }

        public LinkedList<int> ToLinkedList()
        {
            LinkedList<int> list = new LinkedList<int>();
            ListNode currentNode = this;
            while (currentNode != null)
            {
                list.AddLast(currentNode.val);
                currentNode = currentNode.next;
            }
            return list;
        }

        public bool AreEqual(LinkedList<int> list)
        {
            ListNode currentNode = this;
            var enumerator = list.GetEnumerator();
            while (currentNode != null && enumerator.MoveNext())
            {
                if (currentNode.val != enumerator.Current)
                    return false;
                currentNode = currentNode.next;
            }
            return currentNode == null && !enumerator.MoveNext();
        }

    }
 
    public class AddNumbers
    {
        /// <summary>
        /// 1 ms beats 93.78%. 53.14 MB beats 47.08%
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <returns></returns>
        public ListNode AddTwoNumbers(ListNode l1, ListNode l2)
        {
            int n1 = 0, n2 = 0, total = 0, carry = 0;
            ListNode outputNode = null, headNode = null, tempNode = null;
            while (l1 != null || l2 != null)
            {
                n1 = l1 != null ? l1.val : 0;
                n2 = l2 != null ? l2.val : 0;

                total = n1 + n2 + carry;
                carry = total > 9 ? 1 : 0;
                if (headNode == null)
                { // first node being created
                    tempNode = headNode;
                    headNode = new ListNode(total > 9 ? total - 10 : total);
                    outputNode = headNode;
                }
                else
                {
                    tempNode = outputNode;
                    outputNode = new ListNode(total > 9 ? total - 10 : total);
                    tempNode.next = outputNode;
                }
                l1 = l1?.next;
                l2 = l2?.next;
            }
            if (carry != 0)
            { // no numbers left just the carry is left
                tempNode = outputNode;
                outputNode = new ListNode(carry);
                tempNode.next = outputNode;
                outputNode.next = null;

            }
            return headNode;

        }

        public static void Demo()
        {
            AddNumbers addNumbers = new AddNumbers();
            LinkedList<int> list1 = new LinkedList<int>(new int[] { 9, 9, 9, 9, 9, 9, 9 });
            LinkedList<int> list2 = new LinkedList<int>(new int[] { 9, 9, 9, 9 });
            LinkedList<int> result = new LinkedList<int>(new int[] { 8, 9, 9, 9, 0, 0, 0, 1 });

            var response = addNumbers.AddTwoNumbers(new ListNode(list1), new ListNode(list2));
            Console.WriteLine($"{response.AreEqual(result)} should be True");

        }
    }
}
