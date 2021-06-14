#nullable enable

using UnityEngine;
using System;
using System.Collections.Generic;

using Zongband.Game.Boards;
using Zongband.Utils;

namespace Zongband.Game.Generation
{
    public class DungeonData
    {
        public readonly Size Size;
        public readonly List<Room> Rooms = new List<Room>();
        public readonly List<Tuple<Room, Room>> Connections = new List<Tuple<Room, Room>>();
        public Tile PlayerSpawn = Tile.Zero;
        public readonly List<Tile> EnemiesSpawn = new List<Tile>();

        private readonly TerrainSO Floor;
        private readonly TerrainSO Wall;

        public DungeonData(Size size, TerrainSO floor, TerrainSO wall)
        {
            Size = size;
            Floor = floor;
            Wall = wall;
        }

        public BoardData ToBoardData()
        {
            var boardData = new BoardData(Size, Wall);
            foreach (var room in Rooms) boardData.Fill(room.Origin, room.Size, Floor);
            foreach (var connection in Connections)
                ConnectRooms(boardData, connection.Item1, connection.Item2);
            return boardData;
        }

        private void ConnectRooms(BoardData boardData, Room a, Room b)
        {
            if (ConnectStraight(boardData, a, b)) return;
            var seed = a.Origin.X + a.Origin.Y + b.Origin.X + b.Origin.Y;
            if (seed.GetHashCode() % 2 == 0) ConnectCorner(boardData, a, b);
            else ConnectCorner(boardData, b, a);
        }

        private bool ConnectStraight(BoardData boardData, Room a, Room b)
        {
            if (ConnectHorizontal(boardData, a, b)) return true;
            if (ConnectVertical(boardData, a, b)) return true;
            return false;
        }

        private bool ConnectHorizontal(BoardData boardData, Room a, Room b)
        {
            var minHigh = Math.Min(a.Origin.Y + a.Size.Y, b.Origin.Y + b.Size.Y) - 1;
            var maxLow = Math.Max(a.Origin.Y, b.Origin.Y);

            var center = maxLow + (Math.Max(minHigh - maxLow, 0) / 2);

            if (center < a.Origin.Y) return false;
            if (center < b.Origin.Y) return false;
            if (center >= a.Origin.Y + a.Size.Y) return false;
            if (center >= b.Origin.Y + b.Size.Y) return false;

            var start = Math.Min(a.Origin.X + a.Size.X, b.Origin.X + b.Size.X);
            var end = Math.Max(a.Origin.X, b.Origin.X) - 1;
            for (var i = start; i <= end; i++)
                boardData.Modify(new Tile(i, center), Floor);

            return true;
        }

        private bool ConnectVertical(BoardData boardData, Room a, Room b)
        {
            var minHigh = Math.Min(a.Origin.X + a.Size.X, b.Origin.X + b.Size.X) - 1;
            var maxLow = Math.Max(a.Origin.X, b.Origin.X);

            var center = maxLow + (Math.Max(minHigh - maxLow, 0) / 2);

            if (center < a.Origin.X) return false;
            if (center < b.Origin.X) return false;
            if (center >= a.Origin.X + a.Size.X) return false;
            if (center >= b.Origin.X + b.Size.X) return false;

            var start = Math.Min(a.Origin.Y + a.Size.Y, b.Origin.Y + b.Size.Y);
            var end = Math.Max(a.Origin.Y, b.Origin.Y) - 1;
            for (var i = start; i <= end; i++)
                boardData.Modify(new Tile(center, i), Floor);

            return true;
        }

        private void ConnectCorner(BoardData boardData, Room a, Room b)
        {
            var startX = a.Origin.X < b.Origin.X ? a.Origin.X + a.Size.X : a.Origin.X - 1;
            var endX   = a.Origin.X < b.Origin.X ? b.Origin.X : b.Origin.X + b.Size.X - 1;
            var incX   = a.Origin.X < b.Origin.X ? 1 : -1;

            var startY = a.Origin.Y < b.Origin.Y ? a.Origin.Y + a.Size.Y - 1 : a.Origin.Y;
            var endY   = a.Origin.Y < b.Origin.Y ? b.Origin.Y - 1 : b.Origin.Y + b.Size.Y;
            var incY   = a.Origin.Y < b.Origin.Y ? 1 : -1;

            for (var i = startX; i != endX; i += incX)
                boardData.Modify(new Tile(i, startY), Floor);

            for (var i = startY; i != endY; i += incY)
                boardData.Modify(new Tile(endX, i), Floor);

            boardData.Modify(new Tile(endX, endY), Floor);
        }
    }
}