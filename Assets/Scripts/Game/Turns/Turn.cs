#nullable enable

using UnityEngine;
using System;

using Zongband.Game.Entities;

namespace Zongband.Game.Turns
{
    public class Turn : IComparable<Turn>
    {
        public readonly Agent Agent;
        public readonly int Tick;

        public Turn(Agent agent, int tick)
        {
            Agent = agent;
            Tick = tick;
        }

        public int CompareTo(Turn other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            if (other.Tick != Tick) return Tick - other.Tick;
            var priorityDifference = other.Agent.TurnPriority - Agent.TurnPriority;
            if (priorityDifference != 0) return priorityDifference;
            return Agent.TurnCooldown - other.Agent.TurnCooldown;
        }
    }
}