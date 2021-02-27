#nullable enable

using UnityEngine;

using Zongband.Game.Entities;

namespace Zongband.Game.Actions
{
    public class MakePlayerAction : Action
    {
        private readonly Agent? agent;
        private readonly SpawnAction? spawnAction;
        private readonly bool makePlayer;

        public MakePlayerAction(Agent agent)
        : this(agent, true) { }

        public MakePlayerAction(Agent agent, bool makePlayer)
        {
            this.agent = agent;
            this.makePlayer = makePlayer;
        }

        public MakePlayerAction(SpawnAction spawnAction)
        : this(spawnAction, true) { }

        public MakePlayerAction(SpawnAction spawnAction, bool makePlayer)
        {
            this.spawnAction = spawnAction;
            this.makePlayer = makePlayer;
        }

        protected override bool ProcessStart()
        {
            var agent = this.agent;
            if (agent == null) agent = InheritAgent();
            if (agent == null || !agent) return true;
            agent.isPlayer = makePlayer;
            return true;
        }

        private Agent? InheritAgent()
        {
            if (spawnAction == null) return null;
            if (!spawnAction.IsCompleted) return null;
            if (spawnAction.Entity is Agent agent) return agent;
            return null;
        }
    }
}