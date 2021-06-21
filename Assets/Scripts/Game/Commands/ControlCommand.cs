#nullable enable

using UnityEngine;

using Zongband.Game.Entities;

namespace Zongband.Game.Commands
{
    public class MakePlayerCommand : Command
    {
        private readonly Agent? Agent;
        private readonly SpawnCommand? SpawnCommand;
        private readonly bool MakePlayer;

        public MakePlayerCommand(Agent agent)
        : this(agent, true) { }

        public MakePlayerCommand(Agent agent, bool makePlayer)
        {
            Agent = agent;
            MakePlayer = makePlayer;
        }

        public MakePlayerCommand(SpawnCommand spawnCommand)
        : this(spawnCommand, true) { }

        public MakePlayerCommand(SpawnCommand spawnCommand, bool makePlayer)
        {
            SpawnCommand = spawnCommand;
            MakePlayer = makePlayer;
        }

        protected override bool ExecuteStart()
        {
            var agent = Agent;
            if (agent == null) agent = InheritAgent();
            if (agent == null || !agent) return true;
            agent.IsPlayer = MakePlayer;
            return true;
        }

        private Agent? InheritAgent()
        {
            if (SpawnCommand == null) return null;
            if (!SpawnCommand.IsCompleted) return null;
            if (SpawnCommand.Entity is Agent agent) return agent;
            return null;
        }
    }
}