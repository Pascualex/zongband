#nullable enable

using UnityEngine;

using Zongband.Game.Entities;
using Zongband.Utils;

namespace Zongband.Game.Commands
{
    public class AttackCommand : Command
    {
        private readonly Agent Attacker;
        private readonly Agent Target;
        private readonly Context Ctx;
        private EntityAnimator.AnimationState? AnimationState;
        private bool IsDamageDealt = false;

        public AttackCommand(Agent attacker, Agent target, Context ctx)
        {
            Attacker = attacker;
            Target = target;
            Ctx = ctx;
        }

        protected override bool ExecuteStart()
        {
            if (!Attacker.IsAlive || !Target.IsAlive)
            {
                Debug.LogError(Warnings.AgentNotAlive);
                return true;
            }

            var tileDirection = Target.Tile - Attacker.Tile;
            var direction = tileDirection.ToWorld();
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

        protected override bool ExecuteUpdate()
        {
            if (!Attacker.IsAlive || (!Target.IsAlive && !IsDamageDealt))
            {
                Debug.LogError(Warnings.AgentNotAlive);
                return true;
            }

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