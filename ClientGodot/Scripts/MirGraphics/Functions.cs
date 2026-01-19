using System.Drawing; // For Point, Size if needed (or use Godot.Vector2)
using Godot;

namespace ClientGodot.Scripts.MirGraphics
{
    // Need local Functions wrapper if we can't access Shared static properly or for Godot specific helpers
    public static class Functions
    {
        public static bool InRange(Point a, Point b, int i)
        {
            return Math.Abs(a.X - b.X) <= i && Math.Abs(a.Y - b.Y) <= i;
        }

        public static MirDirection DirectionFromPoint(Point source, Point dest)
        {
            if (source.X < dest.X)
            {
                if (source.Y < dest.Y) return MirDirection.DownRight;
                if (source.Y > dest.Y) return MirDirection.UpRight;
                return MirDirection.Right;
            }
            if (source.X > dest.X)
            {
                if (source.Y < dest.Y) return MirDirection.DownLeft;
                if (source.Y > dest.Y) return MirDirection.UpLeft;
                return MirDirection.Left;
            }
            return source.Y < dest.Y ? MirDirection.Down : MirDirection.Up;
        }

        public static Point PointMove(Point p, MirDirection d, int i)
        {
            switch (d)
            {
                case MirDirection.Up: p.Y -= i; break;
                case MirDirection.UpRight: p.X += i; p.Y -= i; break;
                case MirDirection.Right: p.X += i; break;
                case MirDirection.DownRight: p.X += i; p.Y += i; break;
                case MirDirection.Down: p.Y += i; break;
                case MirDirection.DownLeft: p.X -= i; p.Y += i; break;
                case MirDirection.Left: p.X -= i; break;
                case MirDirection.UpLeft: p.X -= i; p.Y -= i; break;
            }
            return p;
        }
    }
}
