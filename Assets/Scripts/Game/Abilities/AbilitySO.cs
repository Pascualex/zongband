#nullable enable

using UnityEngine;

using Zongband.Game.Actions;
using Zongband.Game.Entities;

namespace Zongband.Game.Abilities
{
    [CreateAssetMenu(fileName = "Ability", menuName = "ScriptableObjects/Ability")]
    public class AbilitySO : ScriptableObject
    {
        public int ManaCost = 10;
        public EffectDefinition[] EffectsDefinitions = new EffectDefinition[0];

        public Action CreateAction(Agent caster, Agent target, Action.Context ctx)
        {
            var action = new SequentialAction();

            foreach (var effectDefinition in EffectsDefinitions)
                action.Add(effectDefinition.CreateAction(caster, target, ctx));

            return action;
        }

        private void OnValidate()
        {
            foreach (var effectDefinition in EffectsDefinitions)
            {
                if (effectDefinition.Type == EffectDefinition.EffectType.Attack)
                    effectDefinition.AttackPrms.OnValidate();
                else if (effectDefinition.Type == EffectDefinition.EffectType.Projectile)
                    effectDefinition.ProjectilePrms.OnValidate();
            }
        }
    }
}