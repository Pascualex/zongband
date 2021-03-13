#nullable enable

using UnityEngine;

using Zongband.Game.Entities;

namespace Zongband.Game.Actions
{
    public class MakePlayerAction : Action
    {
        private readonly Agent? Agent;
        private readonly SpawnAction? SpawnAction;
        private readonly bool MakePlayer;

        public MakePlayerAction(Agent agent)
        : this(agent, true) { }

        public MakePlayerAction(Agent agent, bool makePlayer)
        {
            Agent = agent;
            MakePlayer = makePlayer;
        }

        public MakePlayerAction(SpawnAction spawnAction)
        : this(spawnAction, true) { }

        public MakePlayerAction(SpawnAction spawnAction, bool makePlayer)
        {
            SpawnAction = spawnAction;
            MakePlayer = makePlayer;
        }

        protected override bool ProcessStart()
        {
            var agent = Agent;
            if (agent == null) agent = InheritAgent();
            if (agent == null || !agent) return true;
            agent.IsPlayer = MakePlayer;
            return true;
        }

        private Agent? InheritAgent()
        {
            if (SpawnAction == null) return null;
            if (!SpawnAction.IsCompleted) return null;
            if (SpawnAction.Entity is Agent agent) return agent;
            return null;
        }
    }
}