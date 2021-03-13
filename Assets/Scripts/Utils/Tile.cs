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

        public int X;
        public int Y;

        public Tile(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Tile(int xy)
        : this(xy, xy) { }

        public Tile(Vector2 vector2)
        : this((int)vector2.x, (int)vector2.y) { }

        public int GetDistance()
        {
            return GetDistance(Zero);
        }

        public int GetDistance(Tile tile)
        {
            return Mathf.Max(Mathf.Abs(X - tile.X), Mathf.Abs(Y - tile.Y));
        }

        public Vector3Int ToVector3Int()
        {
            return new Vector3Int(X, Y, 0);
        }

        public Vector3 ToWorldVector3()
        {
            return new Vector3(X, 0, Y);
        }

        public Vector3 ToWorld(float scale, Vector3 origin)
        {
            return origin + (new Vector3(X + 0.5f, 0, Y + 0.5f) * scale);
        }

        public override bool Equals(object o)
        {
            if (o is Tile tile)
            {
                return (tile.X == X) && (tile.Y == Y);
            }
            else return false;
        }

        public override int GetHashCode()
        {
            return X ^ Y;
        }

        public static Tile operator +(Tile a, Tile b)
        {
            return new Tile(a.X + b.X, a.Y + b.Y);
        }

        public static Tile operator -(Tile a, Tile b)
        {
            return new Tile(a.X - b.X, a.Y - b.Y);
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
            return "(" + X + ", " + Y + ")";
        }
    }
}