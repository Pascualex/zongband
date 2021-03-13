#nullable enable

using UnityEngine;

using Zongband.Game.Entities;

namespace Zongband.Game.Actions
{
    public class AttackAction : Action
    {
        private readonly Agent Attacker;
        private readonly Agent Target;
        private readonly Context Ctx;
        private EntityAnimator.AnimationState? AnimationState;
        private bool IsDamageDealt = false;

        public AttackAction(Agent attacker, Agent target, Context ctx)
        {
            Attacker = attacker;
            Target = target;
            Ctx = ctx;
        }

        protected override bool ProcessStart()
        {
            if (!Attacker || !Target) return true;

            var tileDirection = Target.Tile - Attacker.Tile;
            var direction = tileDirection.ToWorldVector3();
            Attacker.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

            var animatorComponent = Attacker.GetComponent<EntityAnimator>();
            if (animatorComponent == null)
            {
                Damage();
                return true;
            }

            AnimationState = animatorComponent.Attack();

            return false;
        }

        protected override bool ProcessUpdate()
        {
            if (!Attacker || !Target) return true;

            if (AnimationState == null || AnimationState.IsCompleted)
            {
                if (!IsDamageDealt) Damage();
                return true;
            }

            if (AnimationState.IsReady && !IsDamageDealt)
            {
                Damage();
                IsDamageDealt = true;
            }

            return false;
        }

        private void Damage()
        {
            Target.Damage(Attacker.Attack);
            if (Target.CurrentHealth == 0)
            {
                Ctx.Board.Remove(Target);
                Ctx.TurnManager.Remove(Target);
                GameObject.Destroy(Target.gameObject);
            }
        }
    }
}