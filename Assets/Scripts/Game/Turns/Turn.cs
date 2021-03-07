#nullable enable

using UnityEngine;
using System;

using Zongband.Game.Entities;

namespace Zongband.Game.Turns
{
    public class Turn : IComparable<Turn>
    {
        public readonly Agent agent;
        public readonly int tick;

        public Turn(Agent agent, int tick)
        {
            this.agent = agent;
            this.tick = tick;
        }

        public int CompareTo(Turn other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            if (other.tick != tick) return tick - other.tick;
            var priorityDifference = other.agent.TurnPriority - agent.TurnPriority;
            if (priorityDifference != 0) return priorityDifference;
            return agent.TurnCooldown - other.agent.TurnCooldown;
        }
    }
}