#nullable enable

using UnityEngine;
using System;

using Zongband.Game.Boards;
using Zongband.Utils;

using Random=UnityEngine.Random;

namespace Zongband.Game.Generation
{
    public class Room
    {
        public Tile origin;
        public Size size;
        public Vector2 Center => new Vector2(origin.x + (size.x / 2f), origin.y + (size.y / 2f));
        public bool discarded = false;

        public Room(Tile origin, Size size)
        {
            this.origin = origin;
            this.size = size;
        }

        public bool Collides(Room other)
        {
            if (!Collides(origin.x, size.x, other.origin.x, other.size.x)) return false;
            if (!Collides(origin.y, size.y, other.origin.y, other.size.y)) return false;
            return true;
        }

        public bool MoveAway(Room other, int padding)
        {            
            var left = origin.x + size.x - other.origin.x + padding;
            if (left <= 0) return false;
            var right = other.origin.x + other.size.x - origin.x + padding;
            if (right <= 0) return false;
            var up = other.origin.y + other.size.y - origin.y + padding;
            if (up <= 0) return false;
            var down = origin.y + size.y - other.origin.y + padding;
            if (down <= 0) return false;

            var min = Math.Min(Math.Min(left, right), Math.Min(up, down));

            if (left <= right && left <= min * 4) origin.x--;
            else if (right <= min * 4) origin.x++;
            if (up <= down && up <= min * 4) origin.y++;
            else if (down <= min * 4) origin.y--;

            return true;
        }

        public bool IsOutside(Size dungeonSize, int padding)
        {
            if (origin.x < padding || origin.y < padding) return true;
            if ((origin.x + size.x + 1 - padding) >= dungeonSize.x) return true;
            if ((origin.y + size.y + 1 - padding) >= dungeonSize.y) return true;
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