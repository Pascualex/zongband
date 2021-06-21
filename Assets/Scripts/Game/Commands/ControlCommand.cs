#nullable enable

using UnityEngine;

using Zongband.Game.Entities;
using Zongband.Utils;

namespace Zongband.Game.Commands
{
    public class ControlCommand : Command
    {
        private readonly Agent Agent;
        private readonly bool IsPlayer;

        public ControlCommand(Agent agent, bool isPlayer)
        {
            Agent = agent;
            IsPlayer = isPlayer;
        }

        protected override bool ExecuteStart()
        {
            if (!Agent.IsAlive)
            {
                Debug.LogError(Warnings.AgentNotAlive);
                return true;
            }

            Agent.IsPlayer = IsPlayer;
            return true;
        }
    }
}