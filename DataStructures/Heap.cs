using System;
namespace DataStructures
{//min heap
    public class Heap<T> where T:IComparable
    {
        public int Length
        {
            get
            {
                return length;
            }
            set
            {
                length = value;
            }
        }
        private T[] hArray;// heap array, array is 1-indexed
        private int size;
        private int length;

        public Heap(int _size)
        {
            size = _size;
            length = 0;
            hArray = new T[size + 1];
        }
        

        public void Add(T item)
        {
            if(length >= size)
            {
                Console.WriteLine("Heap capacity reached!");
                return;
            }

            hArray[++length] = item;
            BubbleUp(length);
        }

        public T RemoveMin()
        {
            T min = hArray[1];
            Swap(length--, 1);
            BubbleDown(1);
            return min;
        }

        public T GetMin()
        {
            return hArray[length];
        }

        public void Heapsort(T[] arr)
        {
            length = size = arr.Length;
            hArray = new T[size + 1];
            for(int i = 1; i <= size; i++)
            {
                hArray[i] = arr[i - 1];//hArray is 1 indexed, arr is 0 indexed
            }

            for(int i = size / 2; i >= 1; i--)//refer to Algo Manual Page 122
            {
                BubbleDown(i);
            }

            for(int i = 0; i < size; i++)
            {
                arr[i] = RemoveMin();
            }
        }

        private void BubbleUp(int index)
        {
            //while parent isn't root, parent is bigger than curr
            int parent = GetParentIndex(index), curr = index;
            while(parent >= 1 && hArray[parent].CompareTo(hArray[curr]) == 1)
            {
                Swap(curr, parent);
                curr = parent;
                parent = GetParentIndex(parent);
            }
        }

        private void BubbleDown(int index)
        {
            int smallest = index;//smallest of(index, rightchild, leftchild)

            int leftChild = GetLeftChildIndex(index), rightChild = GetRightChildIndex(index);
            if(leftChild <= length)
            {
                smallest = hArray[smallest].CompareTo(hArray[leftChild]) == 1 ? leftChild : smallest;
            }
            if(rightChild <= length)
            {
                smallest = hArray[smallest].CompareTo(hArray[rightChild]) == 1 ? rightChild : smallest;
            }

            if(smallest != index)
            {
                Swap(smallest, index);
                BubbleDown(smallest);
            }
        }

        private int GetParentIndex(int index)
        {
            return index / 2;
        }

        private int GetLeftChildIndex(int index)
        {
            return index * 2;
        }

        private int GetRightChildIndex(int index)
        {
            return index * 2 + 1;
        }

        private void Swap(int index1, int index2)
        {
            T tmp = hArray[index1];
            hArray[index1] = hArray[index2];
            hArray[index2] = tmp;
        }
    }
}

