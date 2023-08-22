using System;
namespace DataStructures
{
    public class HashTable<T>// using chaining
    {
        public int Length { get; private set; }
        private int tableSize;
        private LinkedList<T>[] values;


        public HashTable()
        {
            tableSize = 31;
            values = new LinkedList<T>[tableSize];
        }

        public void Add(T val)
        {
            int hash = HashCode(val);
            hash &= 0x7fffffff;//make positive 
            hash %= tableSize;

            if (values[hash] == null)
            {
                values[hash] = new LinkedList<T>();
            }
            values[hash].AddFirst(val);

            if(LoadFactor() >= 0.8f)
            {
                Rehash();
            }
        }

        public void Remove(T val)
        {
            int hash = HashCode(val);
            hash &= 0x7fffffff;//make positive 
            hash %= tableSize;

            if(values[hash] != null)
            {
                values[hash].Remove(val);
            }
        }

        private int HashCode(string s)
        {
            int hash = 17;
            for(int i = 0; i < s.Length; i++)
            {
                hash *= hash * 23 + s[i];
            }

            return hash;
        }

        private int HashCode(Object val)
        {
            return val.GetHashCode();
        }

        private float LoadFactor()
        {
            return (float)Length / tableSize; 
        }

        private void Rehash()
        {

        }
    }
}

