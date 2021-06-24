#nullable enable

using UnityEngine;
using System;
using System.Collections.Generic;

using Zongband.Game.Actions;
using Zongband.Game.Entities;

using Action = Zongband.Game.Actions.Action;

namespace Zongband.Game.Abilities
{
    [Serializable]
    public class Effect
    {
        public EffectType Type = EffectType.Attack;
        private EffectType? OldType = null;
        public AttackAction.Parameters AttackPrms = new AttackAction.Parameters();
        public ProjectileAction.Parameters ProjectilePrms = new ProjectileAction.Parameters();
        public List<Effect> Effects = new List<Effect>();

        public Action CreateAction(Agent caster, Agent target, Action.Context ctx)
        {
            if (Type == EffectType.Attack)
                return new AttackAction(caster, target, AttackPrms, ctx);
            else if (Type == EffectType.Projectile)
                return new ProjectileAction(caster, target, ProjectilePrms, ctx);
            else if (Type == EffectType.Sequential || Type == EffectType.Parallel)
            {
                CombinedAction action;
                if (Type == EffectType.Sequential) action = new SequentialAction();
                else action = new ParallelAction();
                foreach (var effect in Effects)
                    action.Add(effect.CreateAction(caster, target, ctx));
                return action;
            }
            return new NullAction();
        }

        public void OnValidate()
        {
            if (Type == EffectType.Attack) AttackPrms.OnValidate();
            else if (Type == EffectType.Projectile) ProjectilePrms.OnValidate();
            else if (Type == EffectType.Sequential || Type == EffectType.Parallel)
                foreach (var effect in Effects) effect.OnValidate();
        }

        public void ClearOld()
        {
            if (Type == EffectType.Sequential || Type == EffectType.Parallel)
                foreach (var effect in Effects) effect.ClearOld();
            if (Type == OldType) return;
            if (OldType != null)
            {
                if (OldType == EffectType.Attack)
                    AttackPrms = new AttackAction.Parameters();
                else if (OldType == EffectType.Projectile)
                    ProjectilePrms = new ProjectileAction.Parameters();
                else if (OldType == EffectType.Sequential && Type != EffectType.Parallel)
                    Effects.Clear();
                else if (OldType == EffectType.Parallel && Type != EffectType.Sequential)
                    Effects.Clear();
            }
            OldType = Type;
        }
    }
}