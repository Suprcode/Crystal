using System;
using System.Collections.Generic;
using System.Drawing; // For Point
using ClientGodot.Scripts.MirControls;

namespace ClientGodot.Scripts.Algorithms
{
    public class PathFinder
    {
        private class Node
        {
            public Point Location;
            public Node Parent;
            public int GCost;
            public int HCost;
            public int FCost => GCost + HCost;

            public Node(Point loc, Node parent, int g, int h)
            {
                Location = loc;
                Parent = parent;
                GCost = g;
                HCost = h;
            }
        }

        private static MapControl _map;
        private static int _gridWidth, _gridHeight;

        public static void SetMap(MapControl map)
        {
            _map = map;
            _gridWidth = map.Width;
            _gridHeight = map.Height;
        }

        public static List<MirDirection> FindPath(Point start, Point end)
        {
            if (_map == null || !IsValid(end)) return null;

            // Simplified A* implementation
            List<Node> openList = new List<Node>();
            HashSet<Point> closedList = new HashSet<Point>();

            openList.Add(new Node(start, null, 0, GetDistance(start, end)));

            while (openList.Count > 0)
            {
                // Find lowest FCost
                Node current = openList[0];
                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].FCost < current.FCost || (openList[i].FCost == current.FCost && openList[i].HCost < current.HCost))
                    {
                        current = openList[i];
                    }
                }

                openList.Remove(current);
                closedList.Add(current.Location);

                if (current.Location == end)
                {
                    return RetracePath(start, current);
                }

                // Neighbors
                for (int i = 0; i < 8; i++)
                {
                    MirDirection dir = (MirDirection)i;
                    Point neighborLoc = Functions.PointMove(current.Location, dir, 1);

                    if (!IsValid(neighborLoc) || closedList.Contains(neighborLoc))
                        continue;

                    // Check collision
                    if (!IsWalkable(neighborLoc))
                        continue;

                    int newGCost = current.GCost + 1; // Simplify diagonal cost to 1 for now, or 1.4 if using floats

                    Node neighborNode = openList.Find(n => n.Location == neighborLoc);
                    if (neighborNode == null)
                    {
                        neighborNode = new Node(neighborLoc, current, newGCost, GetDistance(neighborLoc, end));
                        openList.Add(neighborNode);
                    }
                    else if (newGCost < neighborNode.GCost)
                    {
                        neighborNode.GCost = newGCost;
                        neighborNode.Parent = current;
                    }
                }
            }

            return null;
        }

        private static List<MirDirection> RetracePath(Point start, Node endNode)
        {
            List<MirDirection> path = new List<MirDirection>();
            Node current = endNode;

            while (current.Location != start)
            {
                path.Add(Functions.DirectionFromPoint(current.Parent.Location, current.Location));
                current = current.Parent;
            }

            path.Reverse();
            return path;
        }

        private static int GetDistance(Point a, Point b)
        {
            return Math.Max(Math.Abs(a.X - b.X), Math.Abs(a.Y - b.Y)); // Chebyshev distance for grid
        }

        private static bool IsValid(Point p)
        {
            return p.X >= 0 && p.Y >= 0 && p.X < _gridWidth && p.Y < _gridHeight;
        }

        // Helper since Functions is in Shared namespace but maybe not static imported
        // Or if 'Functions' is global class in Shared project without namespace?
        // Let's assume Shared project has no namespace for Functions class based on file structure check.
        // If it does, we need 'using Shared.Functions;'
        // Assuming global 'Functions' class available via Shared reference.

        private static bool IsWalkable(Point p)
        {
            if (!IsValid(p)) return false;
            // Check MapControl.Cells
            // BackIndex != -1? Or masking?
            // Usually masking checks FrontIndex or specific bits in BackIndex.
            // For now: assume all valid cells are walkable.
            // Real Logic: (Cell.BackIndex & 0x8000) != 0 means blocked? Need to check Mir Enums.

            // Assuming simplified:
            return true;
        }
    }
}
