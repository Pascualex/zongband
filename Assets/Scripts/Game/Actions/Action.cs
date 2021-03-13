#nullable enable

using UnityEngine;

using Zongband.Game.Turns;
using Zongband.Game.Boards;
using Zongband.Game.Entities;

namespace Zongband.Game.Actions
{
    public abstract class Action
    {
        public bool IsCompleted { get; protected set; } = false;
        
        private bool HasStarted = false;

        public void Process()
        {
            if (IsCompleted) return;
            if (!HasStarted) 
            {
                IsCompleted = ProcessStart();
                HasStarted = true;
            }
            else IsCompleted = ProcessUpdate();
        }

        protected virtual bool ProcessStart()
        {
            return false;
        }

        protected virtual bool ProcessUpdate()
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