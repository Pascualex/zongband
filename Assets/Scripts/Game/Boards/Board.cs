#nullable enable

using UnityEngine;
using System;

using Zongband.Game.Entities;
using Zongband.Utils;

namespace Zongband.Game.Boards
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private BoardSO? InitialBoardSO;
        [SerializeField] private UnityEngine.Tilemaps.Tilemap? TerrainTilemap;

        public Size Size { get; private set; } = Size.Zero;
        public float Scale { get; private set; } = 1f;

        private readonly EntityLayer<Agent> AgentLayer = new EntityLayer<Agent>();
        private readonly EntityLayer<Entity> EntityLayer = new EntityLayer<Entity>();
        private readonly TerrainLayer TerrainLayer = new TerrainLayer();

        private void Awake()
        {
            if (InitialBoardSO != null) ApplySO(InitialBoardSO);
        }

        public void ApplySO(BoardSO boardSO)
        {
            Size = boardSO.Size;
            Scale = boardSO.Scale;
            AgentLayer.ChangeSize(Size);
            EntityLayer.ChangeSize(Size);
            TerrainLayer.ChangeSize(Size);
        }

        public void Add(Entity entity, Tile at)
        {
            if (!IsTileAvailable(entity, at, false)) throw new NotEmptyTileException(at);

            if (entity is Agent agent) AgentLayer.Add(agent, at);
            else EntityLayer.Add(entity, at);
        }

        public void Move(Entity entity, Tile to, bool relative)
        {
            if (relative) to += entity.Tile;

            if (!IsTileAvailable(entity, to, false)) throw new NotEmptyTileException(to);

            if (entity is Agent agent) AgentLayer.Move(agent, to);
            else EntityLayer.Move(entity, to);
        }

        public void Remove(Entity entity)
        {
            if (entity is Agent agent) AgentLayer.Remove(agent);
            else EntityLayer.Remove(entity);
        }

        public void Modify(Tile at, TerrainSO terrainSO)
        {
            if (TerrainTilemap == null) throw new ArgumentNullException(nameof(TerrainTilemap));
            if (!IsTileAvailable(terrainSO, at)) throw new NotEmptyTileException(at);

            TerrainLayer.Modify(at, terrainSO);
            TerrainTilemap.SetTile(at.ToVector3Int(), terrainSO.TileBase);
        }

        public void Box(Tile from, Tile to, TerrainSO terrainSO)
        {
            var lower = new Tile(Mathf.Min(from.X, to.X), Mathf.Min(from.Y, to.Y));
            var higher = new Tile(Mathf.Max(from.X, to.X), Mathf.Max(from.Y, to.Y));
            for (var i = lower.Y; i <= higher.Y; i++)
            {
                for (var j = lower.X; j <= higher.X; j++)
                {
                    Modify(new Tile(j, i), terrainSO);
                }
            }
        }

        public void Apply(BoardData boardData)
        {
            Apply(boardData, Tile.Zero);
        }

        public void Apply(BoardData boardData, Tile origin)
        {
            for (var i = 0; i < boardData.Size.Y; i++)
            {
                for (var j = 0; j < boardData.Size.X; j++)
                {
                    var tile = new Tile(j, i);
                    Modify(origin + tile, boardData.GetTerrain(tile));
                }
            }
        }

        public Agent? GetAgent(Entity entity, Tile at, bool relative)
        {
            if (relative) at += entity.Tile;
            return GetAgent(at);
        }

        public Agent? GetAgent(Tile at)
        {
            if (!Size.Contains(at)) return null;
            return AgentLayer.Get(at);
        }

        public bool IsTileEmpty(Tile tile)
        {
            if (!Size.Contains(tile)) return false;
            if (!AgentLayer.IsTileEmpty(tile)) return false;
            if (!EntityLayer.IsTileEmpty(tile)) return false;
            return true;
        }

        public bool IsTileAvailable(Entity entity, Tile tile, bool relative)
        {
            if (relative) tile += entity.Tile;
            if (!Size.Contains(tile)) return false;
            /* Add here special interactions in the future */
            if (!AgentLayer.IsTileEmpty(tile)) return false;
            var isGhost = (entity is Agent agent) && agent.IsGhost;
            if (!isGhost && !EntityLayer.IsTileEmpty(tile)) return false;
            if (!isGhost && TerrainLayer.GetTile(tile).BlocksGround) return false;
            return true;
        }

        public bool IsTileAvailable(TerrainSO terrainSO, Tile tile)
        {
            if (!Size.Contains(tile)) return false;
            /* Add here special interactions in the future */
            if (terrainSO.BlocksGround && !AgentLayer.IsTileEmpty(tile)) return false;
            if (terrainSO.BlocksGround && !EntityLayer.IsTileEmpty(tile)) return false;
            return true;
        }
    }
}