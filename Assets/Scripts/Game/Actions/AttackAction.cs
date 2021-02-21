#nullable enable

using UnityEngine;

using Zongband.Game.Entities;

namespace Zongband.Game.Actions
{
    public class AttackAction : Action
    {
        private readonly Agent attacker;
        private readonly Agent target;
        private readonly int damage;
        private EntityAnimator.AnimationState? animationState;
        private bool isDamageDealt = false;

        public AttackAction(Agent attacker, Agent target, int damage)
        {
            this.attacker = attacker;
            this.target = target;
            this.damage = damage;
        }

        protected override bool ProcessStart()
        {
            var tileDirection = target.tile - attacker.tile;
            var direction = tileDirection.ToWorldVector3();
            attacker.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

            var animatorComponent = attacker.GetComponent<EntityAnimator>();
            if (animatorComponent == null)
            {
                target.Damage(damage);
                return true;
            }

            animationState = animatorComponent.Attack();

            return false;
        }

        protected override bool ProcessUpdate()
        {
            if (animationState == null || animationState.isCompleted)
            {
                if (!isDamageDealt) target.Damage(damage);
                return true;
            }

            if (animationState.isReady && !isDamageDealt)
            {
                target.Damage(damage);
                isDamageDealt = true;
            }
            
            return false;
        }
    }
}