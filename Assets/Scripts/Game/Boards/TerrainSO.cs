#nullable enable

using UnityEngine;
using UnityEngine.Tilemaps;

namespace Zongband.Game.Boards
{
    [CreateAssetMenu(fileName = "Terrain", menuName = "ScriptableObjects/Terrain")]
    public class TerrainSO : ScriptableObject
    {
        public bool BlocksGround = false;
        public bool BlocksAir = false;
        public TileBase? TileBase;
    }
}
