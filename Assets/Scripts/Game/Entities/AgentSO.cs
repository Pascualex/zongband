﻿using UnityEngine;

namespace Zongband.Game.Entities
{
    [CreateAssetMenu(fileName = "Agent", menuName = "ScriptableObjects/Agent")]
    public class AgentSO : EntitySO
    {
        public int turnCooldown = 100;
        public int turnPriority = 0;
        public int maxHealth = 100;

        private void OnValidate()
        {
            turnCooldown = Mathf.Max(turnCooldown, 1);
            turnPriority = Mathf.Max(turnPriority, 0);
            maxHealth = Mathf.Max(maxHealth, 1);
        }
    }
}
