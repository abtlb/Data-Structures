using System;
namespace DataStructures
{
	public class Graph //adjacency list
	{
		private class Edge
		{
			public int to;
			public Edge? next;
			public int weight;
			public Edge()
			{
				next = null;
				weight = 0;
			}
			public Edge(int _to, Edge _next)
			{
				to = _to;
				next = _next;
				weight = 0;
			}
			public Edge(int _to, Edge _next, int _weight)
			{
				to = _to;
				next = _next;
				weight = _weight;
			}
		}

		private struct EdgePair
		{
			public int x;
			public int y;
			public int weight;

			public EdgePair(int _x, int _y, int _weight)
			{
				x = _x;
				y = _y;
				weight = _weight;
			}
		}

		private int nVertices;
		private int nEdges;
		private bool isDirected;
		private bool finished;
		private Edge[] edges;
		private bool[] discovered;
		private bool[] processed;
		private int[] parents;
		private int[] degrees;
		private int currTime;
		private int[] entryTime;
		private int[] exitTime;
		private delegate void ProcessVertex(int node);
		private delegate void ProcessEdge(int x, int y);
		private enum EdgeType { BACK, FORWARD, TREE, CROSS, SELF };

		public Graph(int _nVertices, int _nEdges, bool _isDirected)
		{
			nVertices = _nVertices;
			nEdges = _nEdges;
			isDirected = _isDirected;
			edges = new Edge[nVertices + 1];
			discovered = new bool[nVertices + 1];
			processed = new bool[nVertices + 1];
			parents = new int[nVertices + 1];
			degrees = new int[nVertices + 1];
			entryTime = new int[nVertices + 1];
			exitTime = new int[nVertices + 1];

		}

		public void ReadGraph()
		{
			Console.WriteLine("For each edge, enter vertex1, vertex2.");
			for (int i = 0; i < nEdges; i++)
			{
				Console.WriteLine($"For edge {i + 1}: vertex1: ");
				int x = Convert.ToInt32(Console.ReadLine());
				Console.WriteLine("vertex2: ");
				int y = Convert.ToInt32(Console.ReadLine());
				InsertEdge(x, y, isDirected);
			}

		}

		public void ConstructGraph(int[] edgesArr)// E1V1, E1V2, E2V1, E2V2, ...
		{
			for (int i = 0; i < edgesArr.Length - 1; i += 2)
			{
				InsertEdge(edgesArr[i], edgesArr[i + 1], isDirected);
			}
		}

		public void ConstructGraphWeighted(int[] edgesArr)// E1V1, E1V2, E1Weight, E2V1, E2V2, E2Weight
		{
			for (int i = 0; i < edgesArr.Length - 2; i += 3)
			{
				InsertEdgeWeighted(edgesArr[i], edgesArr[i + 1], isDirected, edgesArr[i + 2]);
			}
		}


		public void PrintGraph()
		{
			for (int i = 0; i < edges.Length; i++)
			{
				Edge? curr = edges[i];
				while (curr != null)
				{
					Console.WriteLine($"From {i} To {curr.to}");
					curr = curr.next;
				}
			}
		}

		public void ConnectedComponents()
		{
			InitializeSearch();
			int c = 1;
			for (int i = 1; i <= nVertices; i++)
			{
				if (!discovered[i])
				{
					Console.WriteLine($"Connected component {c++}: ");
					BFS(i, (x) => { }, (x) => { Console.WriteLine(x); }, (x, y) => { });
				}
			}
		}

		enum Color { UNCOLORED, WHITE, BLACK };
		Color[] colors;
		bool isBipartitie;
		public void Bipartite()
		{
			isBipartitie = true;
			InitializeSearch();
			colors = new Color[nVertices + 1];
			for (int i = 1; i <= nVertices; i++)
			{
				if (!discovered[i])
				{
					colors[i] = Color.WHITE;
					BFS(i, (x) => { }, (x) => { }, CheckBipartite);
					if (isBipartitie == false)
					{
						Console.WriteLine("The graph isn't bipartite.");
						return;
					}
				}
			}
			Console.WriteLine("The graph is bipartite.");

		}

		bool cycleDetected;
		public bool HasCycle()
		{
			cycleDetected = false;
			for (int i = 1; i <= nVertices; i++)
			{
				if (!discovered[i])
				{
					DFS(i, (x) => { }, (x) => { }, CheckCycle);
					if (cycleDetected)
					{
						return true;
					}
				}
			}
			return false;
		}

		public void TopologicalSorting()
		{
			if (!isDirected)
			{
				Console.WriteLine("Cannot perform topological sort on a non-directed graph.");
				return;
			}

			bool detectedCycle = false;
			Stack<int> verteces = new Stack<int>();
			for (int i = 1; i <= nVertices; i++)
			{
				if (discovered[i])
				{
					continue;
				}

				DFS
				(i, (x) => { },
				(x) => { verteces.Push(x); },
				(x, y) =>
				{
					EdgeType t = EdgeIdentification(x, y);
					if (t == EdgeType.BACK)
					{
						Console.WriteLine($"Cycle detected between {x} and {y}. Cannot perform topological sort");
						finished = true;
						detectedCycle = true;
					}
				}
				);

				if (detectedCycle)
				{
					return;
				}
			}

			Console.WriteLine("Toplogical sorting: ");
			while (verteces.Count > 0)
			{
				Console.Write(verteces.Pop());
				if (verteces.Count > 0)
				{
					Console.Write("->");
				}
			}
		}

		public void StronglyConnectedComponents()
		{
			if (!isDirected)
			{
				ConnectedComponents();
				return;
			}

			//topological sorting
			InitializeSearch();
			Stack<int> vertices = new Stack<int>();
			for (int i = 1; i <= nVertices; i++)
			{
				if (!discovered[i])
				{
					DFS
					(
						i,
						(x) => { },
						(x) =>
						{
							vertices.Push(x);
						},

						(x, y) => { }
					);
				}

			}

			//get transpose graph
			Graph gt = Transpose();
			gt.InitializeSearch();
			int compNum = 1;
			while (vertices.Count > 0)
			{
				int ver = vertices.Pop();
				if (!gt.discovered[ver])
				{
					Console.WriteLine($"Component {compNum++}: ");
					gt.DFS
					(
						ver,
						(x) =>
						{
							Console.Write(x + " ");
						}
						,
						(x) => { },
						(x, y) => { }
					);
					Console.Write("\n");//skip a line after printing vertices
				}
			}
		}

		/*
		 *it doesn't matter which vertex we start from since 
		 *all vertices should be included
		 */
		public void MinimumSpanningTree()//Prim's
		{
			//PrimsMinSpanning();
			KruskalsMinSpanning();
		}

		public void ShortestPath(int x, int y)
		{
			/*swap x and y to display path from
			x to y at the end instead of y to x*/
			int tmp = y;
			y = x;
			x = tmp;

			int[] distances = new int[nVertices + 1];
			bool[] inTree = new bool[nVertices + 1];
			for (int i = 1; i <= nVertices; i++)
			{
				distances[i] = int.MaxValue;
				parents[i] = -1;
			}

			int curr = x;
			distances[x] = 0;
			while (!inTree[curr])
			{
				inTree[curr] = true;
				Edge adj = edges[curr];
				while (adj != null)
				{
					int to = adj.to;
					if (distances[to] > distances[curr] + adj.weight)// is better path
					{
						distances[to] = distances[curr] + adj.weight;
						parents[to] = curr;
					}
					adj = adj.next;
				}

				int shortest = int.MaxValue;
				for (int i = 1; i <= nVertices; i++)
				{
					if (!inTree[i] && distances[i] < shortest)
					{
						shortest = distances[i];
						curr = i;
					}
				}
			}


			Console.WriteLine($"The shortest path from {y} to {x}:");
			while (parents[y] != -1)
			{
				Console.WriteLine($"\tEdge {y} to {parents[y]}");
				y = parents[y];
			}
		}




		private void BFS(int start, ProcessVertex ProcessEarly, ProcessVertex ProcessLate, ProcessEdge ProcessEdge)
		{
			Queue<int> queue = new Queue<int>();//next vertices to be processed
			queue.Enqueue(start);
			while (queue.Count > 0)
			{
				int curr = queue.Dequeue();
				discovered[curr] = true;
				ProcessEarly(curr);
				Edge adj = edges[curr];
				while (adj != null)
				{
					if (!discovered[adj.to])
					{
						queue.Enqueue(adj.to);
						discovered[adj.to] = true;
						parents[adj.to] = curr;
					}
					if (!processed[adj.to] || isDirected)
					{
						ProcessEdge(curr, adj.to);
					}
					adj = adj.next;
				}
				processed[curr] = true;
				ProcessLate(curr);
			}
		}

		private void DFS(int start, ProcessVertex ProcessEarly, ProcessVertex ProcessLate, ProcessEdge ProcessEdge)
		{
			discovered[start] = true;
			entryTime[start] = ++currTime;
			ProcessEarly(start);

			if (finished)
			{
				return;
			}

			Edge adj = edges[start];
			while (adj != null)
			{
				if (!discovered[adj.to])
				{
					parents[adj.to] = start;
					ProcessEdge(start, adj.to);
					DFS(adj.to, ProcessEarly, ProcessLate, ProcessEdge);
				}
				else if (isDirected || (parents[start] != adj.to && !processed[adj.to]))
				{
					ProcessEdge(start, adj.to);
				}
				adj = adj.next;
			}

			ProcessLate(start);
			processed[start] = true;
			exitTime[start] = ++currTime;
		}

		private void InsertEdge(int x, int y, bool directed)
		{
			Edge newHead = new Edge(y, edges[x]);
			edges[x] = newHead;
			if (!directed)
			{
				InsertEdge(y, x, true);
			}
		}

		private void InsertEdgeWeighted(int x, int y, bool directed, int weight)//duplication
		{
			Edge newHead = new Edge(y, edges[x], weight);
			edges[x] = newHead;
			if (!directed)
			{
				InsertEdgeWeighted(y, x, true, weight);
			}
		}

		private void InitializeSearch()
		{
			currTime = 0;
			finished = false;
			for (int i = 0; i < nVertices; i++)
			{
				discovered[i] = processed[i] = false;
				entryTime[i] = exitTime[i] = parents[i] = -1;
				degrees[i] = 0;
			}
		}

		private void CheckBipartite(int x, int y)
		{
			if (colors[x] == colors[y])
			{
				isBipartitie = false;
				return;
			}

			//complement x
			if (colors[x] == Color.WHITE)
			{
				colors[y] = Color.BLACK;
			}
			else if (colors[x] == Color.BLACK)
			{
				colors[y] = Color.WHITE;
			}
			else
			{
				colors[y] = Color.UNCOLORED;
			}
		}

		private void CheckCycle(int x, int y)
		{
			if (parents[y] != parents[x])
			{
				Console.WriteLine("Cycle detected. From vertex " + x + " to vertex " + y);
				finished = true;
				cycleDetected = true;
			}
		}

		private EdgeType EdgeIdentification(int x, int y)
		{
			if (parents[y] == x)
			{
				return EdgeType.TREE;
			}
			if (discovered[y] && !processed[y])
			{
				return EdgeType.BACK;
			}
			if (processed[y] && entryTime[x] > entryTime[y])
			{
				return EdgeType.CROSS;
			}
			if (processed[y] && entryTime[x] < entryTime[y])
			{
				return EdgeType.FORWARD;
			}
			return EdgeType.SELF;
		}

		private Graph Transpose()
		{
			Graph g = new Graph(nVertices, nEdges, isDirected);
			for (int i = 0; i < edges.Length; i++)
			{
				Edge? y = edges[i];
				while (y != null)
				{
					g.InsertEdge(y.to, i, isDirected);
					y = y.next;
				}
			}

			return g;
		}

		private void PrimsMinSpanning()
		{
			int weight = 0;
			int start = 1;//could be any vertex in the graph
			bool[] inTree = new bool[nVertices + 1];
			int[] distances = new int[nVertices + 1];

			for (int i = 1; i <= nVertices; i++)
			{
				inTree[i] = false;
				distances[i] = Int32.MaxValue;
				parents[i] = -1;
			}

			distances[start] = 0;
			int curr = start;
			int cheapest = int.MaxValue;
			Console.WriteLine("Minimum spanning tree edges:");
			while (!inTree[curr])
			{
				inTree[curr] = true;
				if (curr != start)
				{
					//print edge
					Console.WriteLine($"\tEdge from {parents[curr]} to {curr}");
					weight += cheapest;
				}

				Edge adj = edges[curr];
				while (adj != null)
				{
					int to = adj.to;
					if (distances[to] > adj.weight)
					{
						distances[to] = adj.weight;
						parents[to] = curr;
					}
					adj = adj.next;
				}

				cheapest = int.MaxValue;
				for (int i = 1; i <= nVertices; i++)
				{
					if (!inTree[i] && distances[i] < cheapest)
					{
						cheapest = distances[i];
						curr = i;
					}
				}
			}
			Console.WriteLine("Minimum spanning tree weight: " + weight);
		}

		private void KruskalsMinSpanning()
		{
			List<EdgePair> edgePairs = ConstructEdgePairs();
			edgePairs.Sort((x, y) => (x.weight - y.weight));//make sure the comparison is right
			UnionFind unionFind = new UnionFind(nVertices);
			int weight = 0;

			Console.WriteLine("Minimum spanning tree edges: ");
			foreach (EdgePair pair in edgePairs)
			{
				if (!unionFind.SameComponent(pair.x, pair.y))
				{
					unionFind.Merge(pair.x, pair.y);
					weight += pair.weight;
					Console.WriteLine($"\tEdge from {pair.x} to {pair.y} ");
				}
			}

			Console.WriteLine("Minimum spanning tree weight: " + weight);
		}

		private List<EdgePair> ConstructEdgePairs()
		{
			List<EdgePair> edgePairs = new List<EdgePair>();
			for (int i = 1; i <= nVertices; i++)
			{
				Edge adj = edges[i];
				while (adj != null)
				{
					int y = adj.to;
					if (y > i)
					{
						edgePairs.Add(new EdgePair(i, y, adj.weight));
					}
					adj = adj.next;
				}
			}

			return edgePairs;
		}

		private int[,] Floyd()
		{
			int[,] weight = new int[nVertices + 1, nVertices + 1];//adjacency matrix
																  //initialize matrix
			for (int i = 1; i <= nVertices; i++)
			{
				for (int j = 1; j <= nVertices; j++)
				{
					weight[i, j] = int.MaxValue;
				}
				weight[i, i] = 0;

				Edge adj = edges[i];
				while (adj != null)
				{
					weight[i, adj.to] = adj.weight;
				}
			}

			for (int k = 1; k <= nVertices; k++)
			{
				for (int i = 1; i <= nVertices; i++)
				{
					for (int j = 1; j <= nVertices; j++)
					{
						weight[i, j] = Math.Min(weight[i, j], weight[i, k] + weight[k, j]);
					}

				}
			}

			return weight;

		}
	}
}

