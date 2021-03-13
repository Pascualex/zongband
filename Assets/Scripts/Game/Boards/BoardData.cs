#nullable enable

using UnityEngine;
using System;
using System.Collections.Generic;

using Zongband.Utils;

namespace Zongband.Game.Boards
{
    public class BoardData
    {
        public readonly Size Size;

        private readonly TerrainSO[][] TerrainsSOs;

        public BoardData(Size size, TerrainSO defaultSO)
        {
            Size = size;
            TerrainsSOs = new TerrainSO[size.Y][];
            for (var i = 0; i < size.Y; i++)
            {
                TerrainsSOs[i] = new TerrainSO[size.X];
                for (var j = 0; j < size.X; j++) TerrainsSOs[i][j] = defaultSO;
            }
        }

        public TerrainSO GetTerrain(Tile at)
        {
            if (!Size.Contains(at)) throw new ArgumentOutOfRangeException();

            return TerrainsSOs[at.Y][at.X];
        }

        public void Modify(Tile at, TerrainSO terrainSO)
        {
            if (!Size.Contains(at)) throw new ArgumentOutOfRangeException();

            TerrainsSOs[at.Y][at.X] = terrainSO;
        }

        public void Fill(Tile origin, Size size, TerrainSO terrainSO)
        {
            for (var i = origin.Y; i < (origin.Y + size.Y); i++)
            {
                for (var j = origin.X; j < (origin.X + size.X); j++)
                {
                    TerrainsSOs[i][j] = terrainSO;
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

            if (width > (size.X / 2)) throw new ArgumentOutOfRangeException();
            if (width > (size.Y / 2)) throw new ArgumentOutOfRangeException();

            var lower = origin;
            var higher = origin + new Tile(size.X - 1, size.Y - 1);

            for (var i = 0; i < width; i++)
            {
                for (var j = lower.Y; j < higher.Y; j++) Modify(new Tile(lower.X, j), terrainSO);
                for (var j = lower.X; j < higher.X; j++) Modify(new Tile(j, higher.Y), terrainSO);
                for (var j = higher.Y; j > lower.Y; j--) Modify(new Tile(higher.X, j), terrainSO);
                for (var j = higher.X; j > lower.X; j--) Modify(new Tile(j, lower.Y), terrainSO);

                lower += Tile.One;
                higher -= Tile.One;
            }
        }
    }
}