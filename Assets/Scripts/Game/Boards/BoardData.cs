#nullable enable

using UnityEngine;
using System;

using Zongband.Utils;

namespace Zongband.Game.Boards
{
    public class BoardData
    {
        public readonly Size size;
        private Tile playerSpawn = Tile.Zero;

        private readonly TerrainSO[][] terrainsSOs;

        public BoardData(Size size, TerrainSO defaultSO)
        {
            this.size = size;
            terrainsSOs = new TerrainSO[size.y][];
            for (var i = 0; i < size.y; i++)
            {
                terrainsSOs[i] = new TerrainSO[size.x];
                for (var j = 0; j < size.x; j++) terrainsSOs[i][j] = defaultSO;
            }
        }

        public TerrainSO GetTerrain(Tile at)
        {
            if (!size.Contains(at)) throw new ArgumentOutOfRangeException();

            return terrainsSOs[at.y][at.x];
        }

        public void Modify(Tile at, TerrainSO terrainSO)
        {
            if (!size.Contains(at)) throw new ArgumentOutOfRangeException();

            terrainsSOs[at.y][at.x] = terrainSO;
        }

        public void Fill(Tile origin, Size size, TerrainSO terrainSO)
        {
            for (var i = origin.y; i < (origin.y + size.y); i++)
            {
                for (var j = origin.x; j < (origin.x + size.x); j++)
                {
                    terrainsSOs[i][j] = terrainSO;
                }
            }
        }

        public void Box(Tile origin, Size size, TerrainSO terrainSO)
        {
            Box(origin, size, terrainSO, 1);
        }

        public void Box(Tile origin, Size size, TerrainSO terrainSO, int width)
        {
            if (width <= 0) throw new ArgumentOutOfRangeException();

            if (width > (size.x / 2)) throw new ArgumentOutOfRangeException();
            if (width > (size.y / 2)) throw new ArgumentOutOfRangeException();

            var lower = origin;
            var higher = origin + new Tile(size.x - 1, size.y - 1);

            for (var i = 0; i < width; i++)
            {
                for (var j = lower.y; j < higher.y; j++) Modify(new Tile(lower.x, j), terrainSO);
                for (var j = lower.x; j < higher.x; j++) Modify(new Tile(j, higher.y), terrainSO);
                for (var j = higher.y; j > lower.y; j--) Modify(new Tile(higher.x, j), terrainSO);
                for (var j = higher.x; j > lower.x; j--) Modify(new Tile(j, lower.y), terrainSO);

                lower += Tile.One;
                higher -= Tile.One;
            }
        }

        public Tile PlayerSpawn
        {
            get => playerSpawn;
            set
            {
                if (!size.Contains(value)) throw new ArgumentOutOfRangeException();
                playerSpawn = value;
            }
        }
    }
}