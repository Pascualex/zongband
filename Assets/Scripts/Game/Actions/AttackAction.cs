#nullable enable

using UnityEngine;
using System;

using Zongband.Game.Entities;
using Zongband.Utils;

namespace Zongband.Game.Actions
{
    public class AttackAction : Action
    {
        private readonly Agent Attacker;
        private readonly Agent Target;
        private readonly Parameters Prms;
        private readonly Context Ctx;

        public AttackAction(Agent attacker, Agent target, Parameters prms, Context ctx)
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

            Damage();
            CheckTargetDeath();
            return true;
        }

        private void Damage()
        {
            var damage = (double)Prms.BaseDamage;
            damage += Attacker.Attack * Prms.AttackMultiplier;
            Target.Damage((int)Math.Round(damage));
        }

        private void CheckTargetDeath()
        {
            if (Target.CurrentHealth == 0)
            {
                Ctx.Board.Remove(Target);
                Ctx.TurnManager.Remove(Target);
                GameObject.Destroy(Target.gameObject);
            }
        }

        [Serializable]
        public class Parameters
        {
            public int BaseDamage;
            public float AttackMultiplier;

            public Parameters()
            {
                Clear();
            }

            public void Clear()
            {
                BaseDamage = 0;
                AttackMultiplier = 0f;
            }

            public void OnValidate()
            {
                BaseDamage = Math.Max(0, BaseDamage);
                AttackMultiplier = Math.Max(0f, AttackMultiplier);
            }
        }
    }
}