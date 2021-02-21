#nullable enable

using UnityEngine;
using System;

namespace Zongband.Game.Entities
{
    public class Agent : Entity
    {
        public bool isPlayer = false;
        
        public int TurnCooldown { get; private set; } = 100;
        public int TurnPriority { get; private set; } = 0;
        public int MaxHealth { get; private set; } = 100;
        public bool IsGhost { get; private set; } = false;
        public bool IsRoamer { get; private set; } = true;
        public int CurrentHealth { get; private set; }

        public Agent()
        {
            CurrentHealth = MaxHealth;
        }

        public void ApplySO(AgentSO agentSO)
        {
            base.ApplySO(agentSO);

            TurnCooldown = agentSO.turnCooldown;
            TurnPriority = agentSO.turnPriority;
            MaxHealth = agentSO.maxHealth;
            IsGhost = agentSO.isGhost;
            IsRoamer = agentSO.isRoamer;

            CurrentHealth = MaxHealth;
        }

        public void Damage(int amount)
        {
            if (amount <= 0) return;
            ChangeHealth(-amount);
        }

        public void Heal(int amount)
        {
            if (amount <= 0) return;
            ChangeHealth(amount);
        }

        private void ChangeHealth(int amount)
        {
            CurrentHealth = Mathf.Max(Mathf.Min(CurrentHealth + amount, MaxHealth), 0);
        }
    }
}
