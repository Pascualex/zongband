#nullable enable

using UnityEngine;

using Zongband.Game.Entities;

namespace Zongband.Game.Actions
{
    public class AttackAction : Action
    {
        private readonly Agent attacker;
        private readonly Agent target;
        private readonly Context ctx;
        private EntityAnimator.AnimationState? animationState;
        private bool isDamageDealt = false;

        public AttackAction(Agent attacker, Agent target, Context ctx)
        {
            this.attacker = attacker;
            this.target = target;
            this.ctx = ctx;
        }

        protected override bool ProcessStart()
        {
            if (!attacker || !target) return true;

            var tileDirection = target.tile - attacker.tile;
            var direction = tileDirection.ToWorldVector3();
            attacker.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

            var animatorComponent = attacker.GetComponent<EntityAnimator>();
            if (animatorComponent == null)
            {
                Damage();
                return true;
            }

            animationState = animatorComponent.Attack();

            return false;
        }

        protected override bool ProcessUpdate()
        {
            if (!attacker || !target) return true;

            if (animationState == null || animationState.isCompleted)
            {
                if (!isDamageDealt) Damage();
                return true;
            }

            if (animationState.isReady && !isDamageDealt)
            {
                Damage();
                isDamageDealt = true;
            }

            return false;
        }

        private void Damage()
        {
            target.Damage(attacker.Attack);
            if (target.CurrentHealth == 0)
            {
                ctx.board.Remove(target);
                ctx.turnManager.Remove(target);
                GameObject.Destroy(target.gameObject);
            }
        }
    }
}