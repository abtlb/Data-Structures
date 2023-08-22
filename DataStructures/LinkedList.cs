using System;
using System.Collections;

namespace DataStructures
{
    public class LinkedList<T> : IEnumerable<LinkedList<T>>
    {
        public class Node
        {
            public T val;
            public Node? Next{ get; set; }

            public Node(T _val)
            {
                val = _val;
            }
        }

        public class ListIterator : IEnumerator<T>
        {
            private Node curr;
            private Node head;

            public ListIterator(LinkedList<T> list)
            {
                curr = head = list.head;
            }

            public T Current => curr.val;

            object IEnumerator.Current => throw new NotImplementedException();

            

            public bool MoveNext()
            {
                curr = curr.Next;
                return curr == null;
            }
            

            public void Reset()
            {
                curr = head;
            }

            void IDisposable.Dispose()
            {
                
            }
        }

        public int length;
        private Node? head;
        private Node? tail;

        public void AddFirst(T val)
        {
            Node newHead = new Node(val);
            newHead.Next = head;
            head = newHead;

            if (tail == null)//empty list
            {
                tail = head;
            }

            length++;
        }

        public void AddLast(T val)
        {
            Node newTail = new Node(val);
            if (tail == null)//empty list
            {
                head = tail = newTail;
            }
            else
            {
                tail.Next = newTail;
                tail = newTail;
            }
            length++; 
        }

        public void RemoveFirst()
        {
            if (length <= 1)
            {
                head = tail = null;
                length = 0;
            }
            else
            {
                head = head.Next;
                length--;
            }
        }

        public void RemoveLast()
        {
            if (length <= 1)
            {
                head = tail = null;
                length = 0;
            }
            else
            {
                Node curr = head;
                while(curr.Next != tail)
                {
                    curr = curr.Next;
                }
                curr.Next = null;
                tail = curr;
                length--;
            }
        }

        public void Remove(T val)
        {
            Node prev = FindPrev(val);
            if(prev == null)//length <= 1
            {
                if (head != null && ((IComparable<T>)head.val).CompareTo(val) == 0)
                {
                    RemoveFirst();
                }
            }
            else
            {
                prev.Next = prev.Next.Next;
                length--;
            }
        }

        public bool Contains(T val)
        {
            Node prev = FindPrev(val);
            if(prev == null)
            {
                return (head != null && head != null && ((IComparable<T>)head.val).CompareTo(val) == 0);
            }
            return true;
        }

        public Node Get(T val)
        {
            Node prev = FindPrev(val);
            if (prev == null)
            {
                if (head != null && head != null && ((IComparable<T>)head.val).CompareTo(val) == 0)
                {
                    return head;
                }
            }
            else
            {
                return prev.Next;
            }
            return null;
        }

        public void PrintList()
        {
            Node? curr = head;
            while(curr != null)
            {
                Console.Write(curr.val + ", ");
                curr = curr.Next;
            }
            Console.Write("\n");
        }

        private Node FindPrev(T val)
        {
            Node? curr = head, prev = null;
            while (curr != null)
            {
                if (((IComparable<T>)curr.val).CompareTo(val) == 0)
                {
                    return prev;
                }
                prev = curr;
                curr = curr.Next;
            }
            return null;
        }

        public IEnumerator GetEnumerator()
        {
            return new ListIterator(this);
        }

        IEnumerator<LinkedList<T>> IEnumerable<LinkedList<T>>.GetEnumerator()
        {
            return (IEnumerator<LinkedList<T>>)GetEnumerator(); 
        }
    }
}

