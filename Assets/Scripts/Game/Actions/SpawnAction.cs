#nullable enable

using UnityEngine;

using Zongband.Game.Turns;
using Zongband.Game.Boards;
using Zongband.Game.Entities;
using Zongband.Utils;

namespace Zongband.Game.Actions
{
    public class SpawnAction : Action
    {
        public Entity? Entity { get; private set; }

        private readonly EntitySO entitySO;
        private readonly Tile tile;
        private readonly Context ctx;
        private readonly bool priority;

        public SpawnAction(EntitySO entitySO, Tile tile, Context ctx)
        : this(entitySO, tile, ctx, false) { }

        public SpawnAction(EntitySO entitySO, Tile tile, Context ctx, bool priority)
        {
            this.entitySO = entitySO;
            this.ctx = ctx;
            this.tile = tile;
            this.priority = priority;
        }

        protected override bool ProcessStart()
        {
            if (entitySO is AgentSO agentSO) Entity = Spawn(agentSO);
            else Entity = Spawn(entitySO);

            if (!AddToBoard(Entity))
            {
                GameObject.Destroy(Entity);
                Entity = null;
                return true;
            }

            if (Entity is Agent agent) AddToTurnManager(agent);
            MoveToSpawnInWorld(Entity);

            return true;
        }

        private Agent Spawn(AgentSO agentSO)
        {
            var parent = ctx.turnManager.transform;
            var agent = GameObject.Instantiate(ctx.agentPrefab, parent);
            agent.ApplySO(agentSO);
            return agent;
        }

        private Entity Spawn(EntitySO entitySO)
        {
            var parent = ctx.board.transform;
            var entity = GameObject.Instantiate(ctx.entityPrefab, parent);
            entity.ApplySO(entitySO);
            return entity;
        }

        private bool AddToBoard(Entity entity)
        {
            if (!ctx.board.IsTileAvailable(entity, tile, false)) return false;
            ctx.board.Add(entity, tile);
            return true;
        }

        private void AddToTurnManager(Agent agent)
        {
            ctx.turnManager.Add(agent, priority);
        }

        private void MoveToSpawnInWorld(Entity entity)
        {
            var spawnPosition = entity.tile.ToWorld(ctx.board.Scale, ctx.board.transform.position);
            entity.transform.position = spawnPosition;
        }
    }
}