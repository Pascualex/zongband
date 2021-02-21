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
        private readonly Context context;
        private readonly bool priority;

        public SpawnAction(EntitySO entitySO, Tile tile, Context context)
        : this(entitySO, tile, context, false) { }

        public SpawnAction(EntitySO entitySO, Tile tile, Context context, bool priority)
        {
            this.entitySO = entitySO;
            this.context = context;
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
            var parent = context.turnManager.transform;
            var agent = GameObject.Instantiate(context.agentPrefab, parent);
            agent.ApplySO(agentSO);
            return agent;
        }

        private Entity Spawn(EntitySO entitySO)
        {
            var parent = context.board.transform;
            var entity = GameObject.Instantiate(context.entityPrefab, parent);
            entity.ApplySO(entitySO);
            return entity;
        }

        private bool AddToBoard(Entity entity)
        {
            if (!context.board.IsTileAvailable(entity, tile, false)) return false;
            context.board.Add(entity, tile);
            return true;
        }

        private void AddToTurnManager(Agent agent)
        {
            context.turnManager.Add(agent, priority);
        }

        private void MoveToSpawnInWorld(Entity entity)
        {
            var board = context.board;
            var spawnPosition = entity.tile.ToWorld(board.Scale, board.transform.position);
            entity.transform.position = spawnPosition;
        }
    }
}