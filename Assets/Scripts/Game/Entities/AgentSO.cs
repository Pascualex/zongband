#nullable enable

using UnityEngine;

namespace Zongband.Game.Entities
{
    [CreateAssetMenu(fileName = "Agent", menuName = "ScriptableObjects/Agent")]
    public class AgentSO : EntitySO
    {
        public string Name = "Example name";
        public int TurnCooldown = 100;
        public int TurnPriority = 0;
        public int MaxHealth = 100;
        public int Attack = 10;
        public bool IsGhost = false;
        public bool IsRoamer = true;

        private void OnValidate()
        {
            Name = Name.Length <= 16 ? Name : Name.Substring(0, 16);
            TurnCooldown = Mathf.Max(TurnCooldown, 1);
            TurnPriority = Mathf.Max(TurnPriority, 0);
            MaxHealth = Mathf.Max(MaxHealth, 1);
            Attack = Mathf.Max(Attack, 0);
        }
    }
}
