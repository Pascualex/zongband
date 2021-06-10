#nullable enable

using UnityEngine;
using System;

using Zongband.Game.Boards;
using Zongband.Utils;

using Random = UnityEngine.Random;

namespace Zongband.Game.Generation
{
    public class Room
    {
        public Tile Origin;
        public Size Size;
        public Vector2 Center => new Vector2(Origin.X + (Size.X / 2f), Origin.Y + (Size.Y / 2f));

        public Room(Tile origin, Size size)
        {
            Origin = origin;
            Size = size;
        }

        public bool Collides(Room other)
        {
            if (!Collides(Origin.X, Size.X, other.Origin.X, other.Size.X)) return false;
            if (!Collides(Origin.Y, Size.Y, other.Origin.Y, other.Size.Y)) return false;
            return true;
        }

        public bool MoveAway(Room other, int padding)
        {
            var left = Origin.X + Size.X - other.Origin.X + padding;
            if (left <= 0) return false;
            var right = other.Origin.X + other.Size.X - Origin.X + padding;
            if (right <= 0) return false;
            var up = other.Origin.Y + other.Size.Y - Origin.Y + padding;
            if (up <= 0) return false;
            var down = Origin.Y + Size.Y - other.Origin.Y + padding;
            if (down <= 0) return false;

            var min = Math.Min(Math.Min(left, right), Math.Min(up, down));

            if (left <= right && left <= min * 4) Origin.X--;
            else if (right <= min * 4) Origin.X++;
            if (up <= down && up <= min * 4) Origin.Y++;
            else if (down <= min * 4) Origin.Y--;

            return true;
        }

        public bool IsOutside(Size dungeonSize, int padding)
        {
            if (Origin.X < padding || Origin.Y < padding) return true;
            if ((Origin.X + Size.X + 1 - padding) >= dungeonSize.X) return true;
            if ((Origin.Y + Size.Y + 1 - padding) >= dungeonSize.Y) return true;
            return false;
        }

        private bool Collides(int originA, int sizeA, int originB, int sizeB)
        {
            if (originA < originB) return (originA + sizeA) >= originB;
            else if (originB < originA) return (originB + sizeB) >= originA;
            else return true;
        }
    }
}