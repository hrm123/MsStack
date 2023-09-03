using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp
{
    // another option was to store reversed linked list and then iterate through both comparing current node of both 
    // for constant O(1) memory - have to use recursion stack - recurse one node at a time till you reach end of list. While unwinding you
    // start comparing last element with head of node (which is stored as member variable) and keep moving along original 
    // linked list as the recursion backs each node at time. Both nodes should have same value.
    /*
     * 206ms (beats 92% of c# users). 64MB (beats 24% of c# users)
     */
    public class ListNode
    {
        public int val;
        public ListNode next;
        public ListNode(int val = 0, ListNode next = null)
        {
            this.val = val;
            this.next = next;
        }
    }

    internal class PalindromeLinkedlist
    {
        public bool IsPalindrome(ListNode head)
        {
            Stack<int> firstHalf = new Stack<int>();
            //assumption - no cycles. end node has next  null
            // palindrom can be both even and ood - 12321 (or) 123321
            // get TotalLength of linkedlist
            int ctr = 0;
            ListNode current = head;
            while (current != null)
            {
                ctr++;
                current = current.next;
            }
            int l = ctr;
            int mid = (int)Math.Floor((l - 1) / 2.0);

            ctr = 0;
            current = head;
            while (ctr <= mid)
            {
                if (ctr == mid && l % 2 != 0)
                {
                    current = current.next;
                    break; //if odd dont add mid element to stack
                }
                firstHalf.Push(current.val);
                current = current.next;
                ctr++;
            }


            while (current != null)
            {
                int leftVal = firstHalf.Pop();
                int rightVal = current.val;
                if (leftVal != rightVal) { return false; }
                current = current.next;
            }
            return true;

        }
    }
}
