#nullable enable

using UnityEngine;

namespace Zongband.Utils
{
    [System.Serializable]
    public struct Size
    {
        public static Size Zero { get; } = new Size(0, 0);
        public static Size One { get; } = new Size(1, 1);

        public int X;
        public int Y;

        public Size(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Size(int xy)
        : this(xy, xy) { }

        public bool Contains(Tile tile)
        {
            return Checker.Range(tile.X, X) && Checker.Range(tile.Y, Y);
        }

        public static Size operator +(Size a, Size b)
        {
            return new Size(a.X + b.X, a.Y + b.Y);
        }

        public static Size operator -(Size a, Size b)
        {
            return new Size(a.X - b.X, a.Y - b.Y);
        }
    }
}