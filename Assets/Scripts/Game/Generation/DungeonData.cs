#nullable enable

using UnityEngine;
using System;
using System.Collections.Generic;

using Zongband.Game.Boards;
using Zongband.Utils;

using Random = UnityEngine.Random;

namespace Zongband.Game.Generation
{
    public class DungeonData
    {
        public readonly Size Size;
        public Tile playerSpawn = Tile.Zero;
        public readonly List<Tile> enemiesSpawn = new List<Tile>();
        public readonly List<Room> Rooms = new List<Room>();

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
            foreach (var room in Rooms) boardData.Fill(room.origin, room.size, Floor);
            return boardData;
        }
    }
}