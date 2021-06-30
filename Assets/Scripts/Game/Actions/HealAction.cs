#nullable enable

using UnityEngine;
using System;

using Zongband.Game.Entities;

namespace Zongband.Game.Actions
{
    public class HealAction : Action
    {
        private readonly Agent Healer;
        private readonly Agent Target;
        private readonly Parameters Prms;

        public HealAction(Agent healer, Agent target, Parameters prms)
        {
            Healer = healer;
            Target = target;
            Prms = prms;
        }

        protected override bool ExecuteStart()
        {
            if (!CheckAlive(Target)) return true;

            Heal();
            return true;
        }

        private void Heal()
        {
            var heal = (double)Prms.BaseHeal;
            Target.Heal((int)Math.Round(heal));
        }

        [Serializable]
        public class Parameters
        {
            public int BaseHeal = 0;

            public void OnValidate()
            {
                BaseHeal = Math.Max(0, BaseHeal);
            }
        }
    }
}