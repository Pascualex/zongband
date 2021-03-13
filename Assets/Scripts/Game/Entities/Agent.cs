#nullable enable

using UnityEngine;
using System;

namespace Zongband.Game.Entities
{
    public class Agent : Entity
    {
        public bool IsPlayer = false;
        
        public int TurnCooldown { get; private set; } = 100;
        public int TurnPriority { get; private set; } = 0;
        public int MaxHealth { get; private set; } = 100;
        public int Attack { get; private set; } = 10;
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

            TurnCooldown = agentSO.TurnCooldown;
            TurnPriority = agentSO.TurnPriority;
            MaxHealth = agentSO.MaxHealth;
            Attack = agentSO.Attack;
            IsGhost = agentSO.IsGhost;
            IsRoamer = agentSO.IsRoamer;

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
