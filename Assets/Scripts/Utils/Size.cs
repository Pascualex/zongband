#nullable enable

using UnityEngine;

namespace Zongband.Utils
{
    [System.Serializable]
    public struct Size
    {
        public static Size Zero { get; } = new Size(0, 0);
        public static Size One { get; } = new Size(1, 1);

        public int x;
        public int y;

        public Size(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Contains(Location coordinates)
        {
            return Checker.Range(coordinates.x, x) && Checker.Range(coordinates.y, y);
        }

        public static Size operator +(Size a, Size b)
        {
            return new Size(a.x + b.x, a.y + b.y);
        }

        public static Size operator -(Size a, Size b)
        {
            return new Size(a.x - b.x, a.y - b.y);
        }
    }
}