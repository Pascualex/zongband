#nullable enable

using UnityEngine;
using System;
using System.Collections.Generic;

using Zongband.Game.Entities;

namespace Zongband.Game.Turns
{
    public class TurnManager : MonoBehaviour
    {
        private readonly LinkedList<Turn> turns = new LinkedList<Turn>();
        private bool hasStarted = false;

        public void Add(Agent agent, bool priority)
        {
            var additionalTicks = priority ? 0 : agent.TurnCooldown;
            var turn = new Turn(agent, GetCurrentTick() + additionalTicks);

            if (!priority)
            {
                for (var node = turns.Last; node != null; node = node.Previous)
                {
                    if (node.Value.CompareTo(turn) <= 0)
                    {
                        turns.AddAfter(node, turn);
                        return;
                    }
                }
            }

            turns.AddFirst(turn);
        }

        public void Remove(Agent agent)
        {
            var node = turns.First;
            while (node != null)
            {
                var next = node.Next;
                if (node.Value.agent == agent) turns.Remove(node);
                node = next;
            }
        }

        public void Next()
        {
            if (turns.Count == 0) return;

            hasStarted = true;

            Add(turns.First.Value.agent, false);
            turns.RemoveFirst();
        }

        public Agent? GetCurrent()
        {
            if (turns.Count == 0) return null;

            return turns.First.Value.agent;
        }

        private int GetCurrentTick()
        {
            return hasStarted ? turns.First.Value.tick : 0;
        }
    }
}