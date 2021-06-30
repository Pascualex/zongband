#nullable enable

using UnityEngine;

using Zongband.Game.Turns;
using Zongband.Game.Boards;
using Zongband.Game.Entities;
using Zongband.Utils;

namespace Zongband.Game.Actions
{
    public abstract class Action
    {
        private bool StartExecuted = false;
        public bool IsCompleted { get; protected set; } = false;

        public void Execute()
        {
            if (!StartExecuted) 
            {
                IsCompleted = ExecuteStart();
                StartExecuted = true;
            }
            else IsCompleted = ExecuteUpdate();
        }

        protected virtual bool ExecuteStart()
        {
            return false;
        }

        protected virtual bool ExecuteUpdate()
        {
            return true;
        }

        protected bool CheckAlive(Entity entity)
        {
            var isAlive = entity.IsAlive;
            if (!isAlive) Debug.LogWarning(Warnings.AgentNotAlive);
            return isAlive;
        }

        public class Context
        {
            public readonly TurnManager TurnManager;
            public readonly Board Board;
            public readonly Agent AgentPrefab;
            public readonly Entity EntityPrefab;

            public Context(TurnManager turnManager, Board board, Agent agentPrefab, Entity entityPrefab)
            {
                TurnManager = turnManager;
                Board = board;
                AgentPrefab = agentPrefab;
                EntityPrefab = entityPrefab;
            }
        }
    }
}