using System;
namespace DataStructures
{
	public class UnionFind
	{
		int[] parents;
		int[] size;//size of subtree given the root.
		int n;

		public UnionFind(int elementsNum)
		{
			n = elementsNum;
			parents = new int[n + 1];
			size = new int[n + 1];
			for(int i = 1; i <= n; i++)
			{
				parents[i] = -1;
				size[i] = 1;
			}
		}

		public bool SameComponent(int elem1, int elem2)
		{
			int r1 = GetRoot(elem1);
            int r2 = GetRoot(elem2);
			return r1 == r2;
        }

		public void Merge(int elem1, int elem2)
		{
            int r1 = GetRoot(elem1);
            int r2 = GetRoot(elem2);

			if(r1 == r2)
			{
				return;
			}

			if (size[r1] >= size[r2])// merging the smaller component into the larger allows for a more balanced tree
			{
				size[r1] += size[r2];
				parents[r2] = r1;
			}
			else
			{
                size[r2] += size[r1];
                parents[r1] = r2;
            }
        }

		private int GetRoot(int elem)
		{
			if (parents[elem] == -1)// element is the root
			{
				return elem;
			}

			return GetRoot(parents[elem]);
        }
	}
}

