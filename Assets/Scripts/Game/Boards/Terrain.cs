#nullable enable

using UnityEngine;
using UnityEngine.Tilemaps;

namespace Zongband.Game.Boards
{
    public class Terrain
    {
        public bool BlocksGround { get; private set; } = false;
        public bool BlocksAir { get; private set; } = false;
        public TileBase? TileBase { get; private set; }

        public void ApplySO(TerrainSO terrainSO)
        {
            BlocksGround = terrainSO.BlocksGround;
            BlocksAir = terrainSO.BlocksGround;
            TileBase = terrainSO.TileBase;
        }
    }
}