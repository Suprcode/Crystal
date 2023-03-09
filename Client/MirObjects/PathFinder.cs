using Client.MirScenes;

namespace Client.MirObjects
{
    public class Heap<T> where T : IHeapItem<T>
    {
        private T[] items;
        private int currentItemCount;

        public Heap(int maxHeapSize)
        {
            items = new T[maxHeapSize];
        }

        public void Add(T item)
        {
            item.HeapIndex = currentItemCount;
            items[currentItemCount] = item;
            SortUp(item);
            currentItemCount++;
        }

        public T RemoveFirst()
        {
            T firstItem = items[0];
            currentItemCount--;

            items[0] = items[currentItemCount];
            items[0].HeapIndex = 0;

            SortDown(items[0]);

            return firstItem;
        }

        public void UpdateItem(T item)
        {
            SortUp(item);
        }

        public int Count { get { return currentItemCount; } }

        public bool Contains(T item)
        {
            return Equals(items[item.HeapIndex], item);
        }

        private void SortDown(T item)
        {
            while (true)
            {
                int childIndexLeft = (item.HeapIndex * 2) + 1;
                int childIndexRight = (item.HeapIndex * 2) + 2;
                int swapIndex = 0;

                if (childIndexLeft < currentItemCount)
                {
                    swapIndex = childIndexLeft;
                    if (childIndexRight < currentItemCount)
                    {
                        if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                        {
                            swapIndex = childIndexRight;
                        }
                    }

                    if (item.CompareTo(items[swapIndex]) < 0)
                    {
                        Swap(item, items[swapIndex]);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        private void SortUp(T item)
        {
            int parentIndex = (item.HeapIndex - 1) / 2;

            while (true)
            {
                T parentItem = items[parentIndex];
                if (item.CompareTo(parentItem) > 0)
                {
                    Swap(item, parentItem);
                }
                else
                    break;

                parentIndex = (item.HeapIndex - 1) / 2;
            }
        }

        private void Swap(T itemA, T itemB)
        {
            items[itemA.HeapIndex] = itemB;
            items[itemB.HeapIndex] = itemA;

            int itemAIndex = itemA.HeapIndex;
            itemA.HeapIndex = itemB.HeapIndex;
            itemB.HeapIndex = itemAIndex;
        }
    }

    public interface IHeapItem<T> : IComparable<T>
    {
        int HeapIndex
        {
            get; set;
        }
    }

    public class Node : IHeapItem<Node>
    {
        public bool Walkable
        {
            get { return Map.EmptyCell(Location); }
        }

        public MapControl Map;
        public Point Location;
        public Node Parent;

        public int GCost, HCost;

        public int FCost
        {
            get { return GCost + HCost; }
        }

        private int _heapIndex;

        public int HeapIndex
        {
            get { return _heapIndex; }
            set { _heapIndex = value; }
        }

        public int CompareTo(Node nodeToCompare)
        {
            int compare = FCost.CompareTo(nodeToCompare.FCost);
            if (compare == 0)
            {
                compare = HCost.CompareTo(nodeToCompare.HCost);
            }
            return -compare;
        }

        public Node(MapControl map, int x, int y)
        {
            Map = map;
            Location = new Point(x, y);
        }
    }

    public class PathFinder
    {
        private Node[,] Grid;

        public MapControl Map;

        public int MaxSize
        {
            get { return Map.Width * Map.Height; }
        }

        public PathFinder(MapControl map)
        {
            Map = map;

            CreateGrid();
        }

        private void CreateGrid()
        {
            Grid = new Node[Map.Width, Map.Height];

            for (int x = 0; x < Map.Width; x++)
            {
                for (int y = 0; y < Map.Height; y++)
                {
                    Grid[x, y] = new Node(Map, x, y);
                }
            }
        }

        public List<Node> FindPath(Point start, Point target, int MaxNodes = 0)
        {
            Node startNode = GetNode(start);
            Node targetNode = GetNode(target);

            Heap<Node> openSet = new Heap<Node>(MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();

                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    var path = RetracePath(startNode, targetNode);
                    return MaxNodes == 0 || path.Count < MaxNodes ? path : null;
                }

                foreach (Node neighbor in GetNeighbours(currentNode))
                {
                    if (!neighbor.Walkable || closedSet.Contains(neighbor))
                        continue;

                    int newMovementCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);
                    if (newMovementCostToNeighbor < neighbor.GCost || !openSet.Contains(neighbor))
                    {
                        neighbor.GCost = newMovementCostToNeighbor;
                        neighbor.HCost = GetDistance(neighbor, targetNode);
                        neighbor.Parent = currentNode;

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                        else
                        {
                            openSet.UpdateItem(neighbor);
                        }
                    }
                }
            }

            return null;
        }

        public List<Node> RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            path.Add(startNode);
            path.Reverse();

            return path;
        }

        private int GetDistance(Node nodeA, Node nodeB)
        {
            int distX = Math.Abs(nodeA.Location.X - nodeB.Location.X);
            int distY = Math.Abs(nodeA.Location.Y - nodeB.Location.Y);

            if (distX > distY)
                return 14 * distY + (10 * (distX - distY));

            return 14 * distX + (10 * (distY - distX));
        }

        private Node GetNode(Point location)
        {
            return Grid[location.X, location.Y];
        }

        private List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;

                    int checkX = node.Location.X + x;
                    int checkY = node.Location.Y + y;

                    if (checkX >= 0 && checkX < Grid.GetLength(0) && checkY >= 0 && checkY < Grid.GetLength(1))
                    {
                        neighbours.Add(Grid[checkX, checkY]);
                    }
                }
            }

            return neighbours;
        }

    }
}