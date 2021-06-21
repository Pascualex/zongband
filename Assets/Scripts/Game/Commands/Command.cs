#nullable enable

using UnityEngine;

using Zongband.Game.Turns;
using Zongband.Game.Boards;
using Zongband.Game.Entities;

namespace Zongband.Game.Commands
{
    public abstract class Command
    {
        public bool IsCompleted { get; protected set; } = false;
        
        private bool HasStarted = false;

        public void Execute()
        {
            if (IsCompleted) return;
            if (!HasStarted) 
            {
                IsCompleted = ExecuteStart();
                HasStarted = true;
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