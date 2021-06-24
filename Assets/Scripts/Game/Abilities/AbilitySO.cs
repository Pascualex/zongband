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
        public Effect[] Effects = new Effect[0];

        public Action CreateAction(Agent caster, Agent target, Action.Context ctx)
        {
            var action = new SequentialAction();

            foreach (var effect in Effects)
                action.Add(effect.CreateAction(caster, target, ctx));

            return action;
        }

        private void OnValidate()
        {
            foreach (var effect in Effects)
            {
                effect.ClearOld();
                effect.OnValidate();
            }
        }
    }
}