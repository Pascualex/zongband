#nullable enable

using UnityEngine;

using Zongband.Game.Boards;
using Zongband.Game.Entities;
using Zongband.Utils;

namespace Zongband.Game.Actions
{
    public class CreateAction : Action
    {
        public Entity Entity { get; private set; }

        private readonly Tile Tile;
        private readonly Context Ctx;

        public CreateAction(EntitySO entitySO, Tile tile, Context ctx)
        {
            Ctx = ctx;
            Tile = tile;

            Entity = Spawn(entitySO);
        }

        protected override bool ExecuteStart()
        {
            if (!AddToBoard(Entity))
            {
                GameObject.Destroy(Entity);
                return true;
            }

            MoveToSpawn(Entity);
            Entity.gameObject.SetActive(true);
            Entity.OnSpawn();
            return true;
        }

        private Entity Spawn(EntitySO entitySO)
        {
            var parent = Ctx.TurnManager.transform;
            var entity = GameObject.Instantiate(Ctx.AgentPrefab, parent);
            entity.ApplySO(entitySO);
            entity.gameObject.SetActive(false);
            return entity;
        }

        private bool AddToBoard(Entity entity)
        {
            if (!Ctx.Board.IsTileAvailable(entity, Tile, false)) return false;
            Ctx.Board.Add(entity, Tile);
            return true;
        }

        private void MoveToSpawn(Entity entity)
        {
            var spawnPosition = entity.Tile.ToWorld(Ctx.Board.Scale, Ctx.Board.transform.position);
            entity.transform.position = spawnPosition;
        }
    }
}