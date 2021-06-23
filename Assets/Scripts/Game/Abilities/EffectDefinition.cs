#nullable enable

using UnityEngine;
using System;

using Zongband.Game.Actions;
using Zongband.Game.Entities;

using Action = Zongband.Game.Actions.Action;

namespace Zongband.Game.Abilities
{
    [Serializable]
    public class EffectDefinition
    {
        public EffectType Type;
        public AttackAction.Parameters AttackPrms = new AttackAction.Parameters();
        public ProjectileAction.Parameters ProjectilePrms = new ProjectileAction.Parameters();

        public Action CreateAction(Agent caster, Agent target, Action.Context ctx)
        {
            if (Type == EffectType.Attack)
                return new AttackAction(caster, target, AttackPrms, ctx);
            else if (Type == EffectType.Projectile)
                return new ProjectileAction(caster, target, ProjectilePrms, ctx);
            return new NullAction();
        }

        public enum EffectType
        {
            Attack,
            Projectile,
        }
    }
}