#nullable enable

using UnityEngine;
using System;

using Zongband.Game.Boards;
using Zongband.Utils;

using Random = UnityEngine.Random;

namespace Zongband.Game.Generation
{
    public class DungeonGenerator : MonoBehaviour
    {
        [SerializeField] private TerrainSO? floor;
        [SerializeField] private TerrainSO? wall;

        private int maxIterations = 10000;

        public BoardData? GenerateDungeon(Size size, int rooms, int minSide, int maxSide, int padding)
        {
            if (floor == null) throw new ArgumentNullException(nameof(floor));
            if (wall == null) throw new ArgumentNullException(nameof(wall));

            var boardData = new BoardData(size, wall);

            var roomList = GenerateRooms(rooms, size, minSide, maxSide);
            var iterations = 0;
            while (iterations < maxIterations && ExpandRooms(roomList, size, padding)) iterations++;
            Debug.Log("Dungeon generated in " + (iterations + 1) + " iterations");
            if (iterations >= maxIterations) Debug.LogWarning("Iteration limit reached");

            var playerSpawnPlaced = false;
            foreach (var room in roomList)
            {
                if (room.discarded) continue;
                boardData.Fill(room.origin, room.size, floor);
                if (!playerSpawnPlaced)
                {
                    boardData.PlayerSpawn = room.origin;
                    playerSpawnPlaced = true;
                }
            }

            return boardData;
        }

        public BoardData? GenerateTestDungeon(Size size, int wallWidth)
        {
            if (floor == null) throw new ArgumentNullException(nameof(floor));
            if (wall == null) throw new ArgumentNullException(nameof(wall));

            var boardData = new BoardData(size, floor);

            boardData.Box(Tile.Zero, size, wall, wallWidth);
            boardData.PlayerSpawn = new Tile(5, 5);

            return boardData;
        }

        private Room[] GenerateRooms(int quantity, Size dungeonSize, int minSide, int maxSide)
        {
            var rooms = new Room[quantity];
            for (var i = 0; i < rooms.Length; i++) rooms[i] = GenerateRoom(dungeonSize, minSide, maxSide);
            return rooms;
        }

        private Room GenerateRoom(Size dungeonSize, int minSide, int maxSide)
        {
            var sideX = Random.Range(minSide, maxSide + 1);
            var sideY = Random.Range(minSide, maxSide + 1);
            var size = new Size(sideX, sideY);

            var insideUnitCircle = Random.insideUnitCircle;
            var x = (dungeonSize.x / 2f) + (insideUnitCircle.x * dungeonSize.x / 8f) - (size.x / 2f);
            var y = (dungeonSize.y / 2f) + (insideUnitCircle.y * dungeonSize.y / 8f) - (size.y / 2f);
            var origin = new Tile(Convert.ToInt32(x), Convert.ToInt32(y));

            return new Room(origin, size);
        }

        private bool ExpandRooms(Room[] rooms, Size dungeonSize, int padding)
        {
            var collision = false;

            for (var i = 0; i < rooms.Length; i++)
            {
                if (rooms[i].discarded) continue;
                for (var j = 0; j < rooms.Length; j++)
                {
                    if (i == j) continue;
                    if (rooms[j].discarded) continue;
                    if (!rooms[i].MoveAway(rooms[j], padding)) continue;
                    if (rooms[i].IsOutside(dungeonSize, padding)) rooms[i].discarded = true;
                    collision = true;
                    break;
                }
            }
            return collision;
        }
    }
}