using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoDemos.puzzles
{

    public static class LinkedList
    {

        public static void Append(ref Node head, int data)
        {
            if (head != null)
            {
                Node current = head;
                while (current.Next != null)
                {
                    current = current.Next;
                }

                current.Next = new Node();
                current.Next.Data = data;
            }
            else
            {
                head = new Node();
                head.Data = data;
            }
        }

        public static void Print(Node head)
        {
            if (head == null) return;

            Node current = head;
            do
            {
                Console.Write("{0} ", current.Data);
                current = current.Next;
            } while (current != null);
        }

        public static void PrintRecursive(Node head)
        {
            if (head == null)
            {
                Console.WriteLine();
                return;
            }

            Console.Write("{0} ", head.Data);
            PrintRecursive(head.Next);
        }

        public static void Reverse(ref Node head)
        {
            if (head == null) return;

            Node prev = null, current = head, next = null;

            while (current.Next != null)
            {
                next = current.Next;
                current.Next = prev;
                prev = current;
                current = next;
            }

            current.Next = prev;
            head = current;
        }

        public static Node DeleteNthNode(int x, Node head)
        {

            if (x == 1)
            {
                return head.Next;
            }
            Node lag = head;
            Node lead = head.Next;
            //lag Node will point at the node before the Node we want to delete, lead is set to the node
            //set to be deleted
            for (int i = 2; i < x; i++)
            {
                lag = lag.Next;
                lead = lead.Next;
            }
            //Skip the node to be deleted
            lag.Next = lead.Next;
            return head;
        }

        public static Node newHead;
        public static Node AnotherReverseUsingRecursion(Node current)
        {
            if (current.Next == null)
            {
                newHead = current;
                return current;
            }

            Node returned = AnotherReverseUsingRecursion(current.Next);
            returned.Next = current;
            current.Next = null;
            return current;
        }
        public static void ReverseUsingRecursion(Node head)
        {
            if (head == null) return;

            if (head.Next == null)
            {
                newHead = head;
                return;
            }

            ReverseUsingRecursion(head.Next);
            head.Next.Next = head;
            head.Next = null;

        }
    }

    public class Node
    {
        public int Data = 0;
        public Node Next = null;
    }

    public class LinkedListStuff
    {
        public void demo()
        {
            PrintReverse();
        }

        

        private void PrintReverse()
        {
            Node head = null;
            LinkedList.Append(ref head, 25);
            LinkedList.Append(ref head, 5);
            LinkedList.Append(ref head, 18);
            LinkedList.Append(ref head, 7);

            Console.WriteLine("Linked list:");
            LinkedList.Print(head);

            // LinkedList.Reverse(ref head);

            head = LinkedList.AnotherReverseUsingRecursion(head);
            Console.WriteLine();
            Console.WriteLine("Reversed Linked list:");
            LinkedList.Print(LinkedList.newHead);

            Console.WriteLine();

        }

    }

    
}
