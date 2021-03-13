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
        [SerializeField] private TerrainSO? Floor;
        [SerializeField] private TerrainSO? Wall;
        [SerializeField] private int MaxIterations;

        public DungeonData? GenerateDungeon(Size size, int rooms, int minSide, int maxSide, int padding)
        {
            if (Floor == null) throw new ArgumentNullException(nameof(Floor));
            if (Wall == null) throw new ArgumentNullException(nameof(Wall));

            var roomList = GenerateRooms(rooms, size, minSide, maxSide);
            var iterations = 0;
            while (iterations < MaxIterations && ExpandRooms(roomList, size, padding)) iterations++;
            Debug.Log("Dungeon generated in " + (iterations + 1) + " iterations");
            if (iterations >= MaxIterations) Debug.LogWarning("Iteration limit reached");

            var dungeonData = new DungeonData(size, Floor, Wall);
            var playerSpawnPlaced = false;
            foreach (var room in roomList)
            {
                if (room.Discarded) continue;
                dungeonData.Rooms.Add(room);
                if (!playerSpawnPlaced)
                {
                    dungeonData.PlayerSpawn = room.Origin;
                    playerSpawnPlaced = true;
                }
                else dungeonData.EnemiesSpawn.Add(room.Origin);
            }

            return dungeonData;
        }

        public DungeonData GenerateTestDungeon(Size size, int wallWidth)
        {
            if (Floor == null) throw new ArgumentNullException(nameof(Floor));
            if (Wall == null) throw new ArgumentNullException(nameof(Wall));

            var dungeonData = new DungeonData(size, Floor, Wall);

            dungeonData.Rooms.Add(new Room(new Tile(wallWidth), size - new Size(wallWidth * 2)));
            dungeonData.PlayerSpawn = new Tile(5, 5);

            return dungeonData;
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
            var x = (dungeonSize.X / 2f) + (insideUnitCircle.x * dungeonSize.X / 8f) - (size.X / 2f);
            var y = (dungeonSize.Y / 2f) + (insideUnitCircle.y * dungeonSize.Y / 8f) - (size.Y / 2f);
            var origin = new Tile(Convert.ToInt32(x), Convert.ToInt32(y));

            return new Room(origin, size);
        }

        private bool ExpandRooms(Room[] rooms, Size dungeonSize, int padding)
        {
            var collision = false;
            for (var i = 0; i < rooms.Length; i++)
            {
                if (rooms[i].Discarded) continue;
                for (var j = 0; j < rooms.Length; j++)
                {
                    if (i == j) continue;
                    if (rooms[j].Discarded) continue;
                    if (!rooms[i].MoveAway(rooms[j], padding)) continue;
                    if (rooms[i].IsOutside(dungeonSize, padding)) rooms[i].Discarded = true;
                    collision = true;
                    break;
                }
            }
            return collision;
        }
    }
}