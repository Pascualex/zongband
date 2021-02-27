#nullable enable

using UnityEngine;

namespace Zongband.Utils
{
    public struct Tile
    {
        public static Tile Zero { get; } = new Tile(0, 0);
        public static Tile One { get; } = new Tile(1, 1);
        public static Tile MinusOne { get; } = new Tile(-1, -1);
        public static Tile Up { get; } = new Tile(0, 1);
        public static Tile Right { get; } = new Tile(1, 0);
        public static Tile Down { get; } = new Tile(0, -1);
        public static Tile Left { get; } = new Tile(-1, 0);
        public static Tile UpRight { get; } = new Tile(1, 1);
        public static Tile DownRight { get; } = new Tile(1, -1);
        public static Tile DownLeft { get; } = new Tile(-1, -1);
        public static Tile UpLeft { get; } = new Tile(-1, 1);

        public int x;
        public int y;

        public Tile(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int GetDistance()
        {
            return GetDistance(Zero);
        }

        public int GetDistance(Tile tile)
        {
            return Mathf.Max(Mathf.Abs(x - tile.x), Mathf.Abs(y - tile.y));
        }

        public Vector3Int ToVector3Int()
        {
            return new Vector3Int(x, y, 0);
        }

        public Vector3 ToWorldVector3()
        {
            return new Vector3(x, 0, y);
        }

        public Vector3 ToWorld(float scale, Vector3 origin)
        {
            return origin + (new Vector3(x + 0.5f, 0, y + 0.5f) * scale);
        }

        public override bool Equals(object o)
        {
            if (o is Tile tile)
            {
                return (tile.x == x) && (tile.y == y);
            }
            else return false;
        }

        public override int GetHashCode()
        {
            return x ^ y;
        }

        public static Tile operator +(Tile a, Tile b)
        {
            return new Tile(a.x + b.x, a.y + b.y);
        }

        public static Tile operator -(Tile a, Tile b)
        {
            return new Tile(a.x - b.x, a.y - b.y);
        }

        public static bool operator ==(Tile a, Tile b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(Tile a, Tile b)
        {
            return !Equals(a, b);
        }

        public static Tile[] RandomizedDirections()
        {
            var directions = new Tile[] {
                Up,
                Right,
                Down,
                Left,
                UpRight,
                DownRight,
                DownLeft,
                UpLeft
            };

            Shuffler.Shuffle(directions);

            return directions;
        }

        public override string ToString()
        {
            return "(" + x + ", " + y + ")";
        }
    }
}