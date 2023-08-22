using DataStructures;

Graph g1 = new Graph(7, 10, true);
int[] edges1 =
{
    1, 2,
    1, 5,
    2, 3,
    2, 5,
    4, 3,
    5, 4,
    5, 6,
    6, 4,
    7, 6,
    7, 1
};
//g1.ConstructGraph(edges1);
//g1.TopologicalSorting();

Graph g2 = new Graph(7, 10, true);
int[] edges2 =
{
    2,1, 
    5,1, 
    3,2, 
    5,2, 
    3,4, 
    4,5, 
    6,5, 
    4,6, 
    6,7, 
    1,7 
};
//g2.ConstructGraph(edges2);
//g2.TopologicalSorting();

Graph g3 = new Graph(5, 5, true);
int[] edges3 =
{
    2, 1,
    3, 2,
    1, 3,
    1, 4,
    4, 5
};
//g3.ConstructGraph(edges3);
//g3.StronglyConnectedComponents();

Graph g4 = new Graph(7, 10, false);
int[] edges4 =
{
    1, 3, 1,
    1, 4, 4,
    1, 2, 5,
    2, 4, 8,
    2, 6, 6,
    3, 4, 3,
    3, 5, 2,
    4, 6, 8,
    5, 6, 7,
    5, 7, 9
};
//g4.ConstructGraphWeighted(edges4);
//g4.MinimumSpanningTree();

Graph g5 = new Graph(5, 6, false);
int[] edges5 =
{
    1, 2, 4,
    1, 4, 9,
    1, 5, 1,
    2, 4, 4,
    3, 5, 9,
    3, 4, 1
};
g5.ConstructGraphWeighted(edges5);
g5.ShortestPath(1, 4);

Graph g6 = new Graph(6, 6, false);

