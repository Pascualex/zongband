#nullable enable

using UnityEngine;
using System;

using Zongband.Utils;

namespace Zongband.Game.Boards
{
    public class TerrainLayer : Layer
    {
        private Terrain[][] Terrains = new Terrain[0][];

        public override void ChangeSize(Size size)
        {
            base.ChangeSize(size);

            Terrains = new Terrain[Size.Y][];
            for (var i = 0; i < Size.Y; i++)
            {
                Terrains[i] = new Terrain[Size.X];
                for (var j = 0; j < Size.X; j++)
                {
                    Terrains[i][j] = new Terrain();
                }
            }
        }

        public void Modify(Tile at, TerrainSO terrainSO)
        {
            if (!Size.Contains(at)) throw new ArgumentOutOfRangeException();

            Terrains[at.Y][at.X].ApplySO(terrainSO);
        }

        public Terrain GetTile(Tile at)
        {
            if (!Size.Contains(at)) throw new ArgumentOutOfRangeException();

            return Terrains[at.Y][at.X];
        }
    }
}
