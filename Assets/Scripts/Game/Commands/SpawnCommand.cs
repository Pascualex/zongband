#nullable enable

using UnityEngine;

using Zongband.Game.Turns;
using Zongband.Game.Boards;
using Zongband.Game.Entities;
using Zongband.Utils;

namespace Zongband.Game.Commands
{
    public class SpawnCommand : Command
    {
        public Entity? Entity { get; private set; }

        private readonly EntitySO EntitySO;
        private readonly Tile Tile;
        private readonly Context Ctx;
        private readonly bool Priority;

        public SpawnCommand(EntitySO entitySO, Tile tile, Context ctx)
        : this(entitySO, tile, ctx, false) { }

        public SpawnCommand(EntitySO entitySO, Tile tile, Context ctx, bool priority)
        {
            EntitySO = entitySO;
            Ctx = ctx;
            Tile = tile;
            Priority = priority;
        }

        protected override bool ExecuteStart()
        {
            if (EntitySO is AgentSO agentSO) Entity = Spawn(agentSO);
            else Entity = Spawn(EntitySO);

            if (!AddToBoard(Entity))
            {
                GameObject.Destroy(Entity);
                Entity = null;
                return true;
            }

            if (Entity is Agent agent) AddToTurnManager(agent);
            MoveToSpawn(Entity);

            return true;
        }

        private Agent Spawn(AgentSO agentSO)
        {
            var parent = Ctx.TurnManager.transform;
            var agent = GameObject.Instantiate(Ctx.AgentPrefab, parent);
            agent.ApplySO(agentSO);
            return agent;
        }

        private Entity Spawn(EntitySO entitySO)
        {
            var parent = Ctx.Board.transform;
            var entity = GameObject.Instantiate(Ctx.EntityPrefab, parent);
            entity.ApplySO(entitySO);
            return entity;
        }

        private bool AddToBoard(Entity entity)
        {
            if (!Ctx.Board.IsTileAvailable(entity, Tile, false)) return false;
            Ctx.Board.Add(entity, Tile);
            return true;
        }

        private void AddToTurnManager(Agent agent)
        {
            Ctx.TurnManager.Add(agent, Priority);
        }

        private void MoveToSpawn(Entity entity)
        {
            var spawnPosition = entity.Tile.ToWorld(Ctx.Board.Scale, Ctx.Board.transform.position);
            entity.transform.position = spawnPosition;
        }
    }
}