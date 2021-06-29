#nullable enable

using UnityEngine;
using System;

using Zongband.Game.Entities;
using Zongband.Utils;

namespace Zongband.Game.Actions
{
    public class HealAction : Action
    {
        private readonly Agent Attacker;
        private readonly Agent Target;
        private readonly Parameters Prms;
        private readonly Context Ctx;

        public HealAction(Agent attacker, Agent target, Parameters prms, Context ctx)
        {
            Attacker = attacker;
            Target = target;
            Prms = prms;
            Ctx = ctx;
        }

        protected override bool ExecuteStart()
        {
            if (!Target.IsAlive)
            {
                Debug.LogWarning(Warnings.AgentNotAlive);
                return true;
            }

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