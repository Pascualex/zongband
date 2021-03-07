#nullable enable

using UnityEngine;

using Zongband.Game.Entities;
using Zongband.Utils;

namespace Zongband.Game.Boards
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private BoardSO? initialBoardSO;
        [SerializeField] private UnityEngine.Tilemaps.Tilemap? terrainTilemap;

        public Size Size { get; private set; } = Size.Zero;
        public float Scale { get; private set; } = 1.0f;

        private readonly EntityLayer<Agent> agentLayer = new EntityLayer<Agent>();
        private readonly EntityLayer<Entity> entityLayer = new EntityLayer<Entity>();
        private readonly TerrainLayer terrainLayer = new TerrainLayer();

        private void Awake()
        {
            if (initialBoardSO != null) ApplySO(initialBoardSO);
        }

        public void ApplySO(BoardSO boardSO)
        {
            Size = boardSO.size;
            Scale = boardSO.scale;
            agentLayer.ChangeSize(Size);
            entityLayer.ChangeSize(Size);
            terrainLayer.ChangeSize(Size);
        }

        public void Add(Entity entity, Tile at)
        {
            if (!IsTileAvailable(entity, at, false)) throw new NotEmptyTileException(at);

            if (entity is Agent agent) agentLayer.Add(agent, at);
            else entityLayer.Add(entity, at);
        }

        public void Move(Entity entity, Tile to, bool relative)
        {
            if (relative) to += entity.tile;

            if (!IsTileAvailable(entity, to, false)) throw new NotEmptyTileException(to);

            if (entity is Agent agent) agentLayer.Move(agent, to);
            else entityLayer.Move(entity, to);
        }

        public void Remove(Entity entity)
        {
            if (entity is Agent agent) agentLayer.Remove(agent);
            else entityLayer.Remove(entity);
        }

        public void Modify(Tile at, TerrainSO terrainSO)
        {
            if (!IsTileAvailable(terrainSO, at)) throw new NotEmptyTileException(at);

            terrainLayer.Modify(at, terrainSO);
            terrainTilemap?.SetTile(at.ToVector3Int(), terrainSO.tileBase);
        }

        public void Box(Tile from, Tile to, TerrainSO terrainSO)
        {
            var lower = new Tile(Mathf.Min(from.x, to.x), Mathf.Min(from.y, to.y));
            var higher = new Tile(Mathf.Max(from.x, to.x), Mathf.Max(from.y, to.y));
            for (var i = lower.y; i <= higher.y; i++)
            {
                for (var j = lower.x; j <= higher.x; j++)
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
            for (var i = 0; i < boardData.size.y; i++)
            {
                for (var j = 0; j < boardData.size.x; j++)
                {
                    var tile = new Tile(j, i);
                    Modify(origin + tile, boardData.GetTerrain(tile));
                }
            }
        }

        public Agent? GetAgent(Entity entity, Tile at, bool relative)
        {
            if (relative) at += entity.tile;
            return GetAgent(at);
        }

        public Agent? GetAgent(Tile at)
        {
            if (!Size.Contains(at)) return null;
            return agentLayer.Get(at);
        }

        public bool IsTileEmpty(Tile tile)
        {
            if (!Size.Contains(tile)) return false;
            if (!agentLayer.IsTileEmpty(tile)) return false;
            if (!entityLayer.IsTileEmpty(tile)) return false;
            return true;
        }

        public bool IsTileAvailable(Entity entity, Tile tile, bool relative)
        {
            if (relative) tile += entity.tile;
            if (!Size.Contains(tile)) return false;
            /* Add here special interactions in the future */
            if (!agentLayer.IsTileEmpty(tile)) return false;
            var isGhost = (entity is Agent agent) && agent.IsGhost;
            if (!isGhost && !entityLayer.IsTileEmpty(tile)) return false;
            if (!isGhost && terrainLayer.GetTile(tile).BlocksGround) return false;
            return true;
        }

        public bool IsTileAvailable(TerrainSO terrainSO, Tile tile)
        {
            if (!Size.Contains(tile)) return false;
            /* Add here special interactions in the future */
            if (terrainSO.blocksGround && !agentLayer.IsTileEmpty(tile)) return false;
            if (terrainSO.blocksGround && !entityLayer.IsTileEmpty(tile)) return false;
            return true;
        }
    }
}