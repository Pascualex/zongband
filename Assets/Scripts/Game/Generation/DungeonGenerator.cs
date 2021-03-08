#nullable enable

using UnityEngine;
using System;

using Zongband.Game.Boards;
using Zongband.Utils;

namespace Zongband.Game.Generation
{
    public class DungeonGenerator : MonoBehaviour
    {
        [SerializeField] private TerrainSO? floor;
        [SerializeField] private TerrainSO? wall;

        public BoardData? GenerateDungeon(Size size)
        {
            if (floor == null) throw new ArgumentNullException(nameof(floor));
            if (wall == null) throw new ArgumentNullException(nameof(wall));

            var boardData = new BoardData(size, floor);

            boardData.Box(Tile.Zero, new Tile(size.x - 1, size.y - 1), wall, 2);
            boardData.PlayerSpawn = new Tile(5, 5);

            return boardData;
        }
    }
}