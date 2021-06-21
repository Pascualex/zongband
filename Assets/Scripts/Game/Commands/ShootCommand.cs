#nullable enable

using UnityEngine;

using Zongband.Game.Boards;
using Zongband.Game.Entities;
using Zongband.Utils;

namespace Zongband.Game.Commands
{
    public class ShootCommand : Command
    {
        private const float AnimationFixedSpeed = 1f;
        private const float AnimationVariableSpeed = 15f;

        private Tile Start;
        private Tile Finish;
        private readonly Agent? Caster = null;
        private readonly Agent? Target = null;
        private readonly Context Ctx;
        private GameObject? Projectile;

        public ShootCommand(Tile start, Tile finish, Context ctx)
        : this(start, finish, null, null, ctx)
        { }

        public ShootCommand(Agent caster, Tile finish, Context ctx)
        : this(Tile.Zero, finish, caster, null, ctx)
        { }

        public ShootCommand(Tile start, Agent target, Context ctx)
        : this(start, Tile.Zero, null, target, ctx)
        { }

        public ShootCommand(Agent caster, Agent target, Context ctx)
        : this(Tile.Zero, Tile.Zero, caster, target, ctx)
        { }

        private ShootCommand(Tile start, Tile finish, Agent? caster, Agent? target, Context ctx)
        {
            Start = start;
            Finish = finish;
            Caster = caster;
            Target = target;
            Ctx = ctx;
        }

        protected override bool ExecuteStart()
        {
            if ((Caster != null && !Caster.IsAlive) || (Target != null && !Target.IsAlive))
            {
                Debug.LogError(Warnings.AgentNotAlive);
                return true;
            }

            if (Caster != null) Start = Caster.Tile;
            if (Target != null) Finish = Target.Tile;

            CreateProjectile();

            return false;
        }

        protected override bool ExecuteUpdate()
        {
            if (Target != null && !Target.IsAlive)
            {
                Debug.LogError(Warnings.AgentNotAlive);
                return true;
            }

            if (Target != null) Finish = Target.Tile;

            var completed = MoveTowardsTarget();
            if (completed) DestroyProjectile();
            return completed;
        }

        private void CreateProjectile()
        {
            var position = Start.ToWorld(Ctx.Board.Scale, Ctx.Board.transform.position);
            var tileDirection = Finish - Start;
            var direction = tileDirection.ToWorld();
            var rotation = Quaternion.LookRotation(direction, Vector3.up);

            Projectile = GameObject.Instantiate(Ctx.EntityPrefab, position, rotation).gameObject;
        }

        private void DestroyProjectile()
        {
            if (Projectile != null) GameObject.Destroy(Projectile);
        }

        private bool MoveTowardsTarget()
        {
            if (Projectile == null) return true;

            var targetPosition = Finish.ToWorld(Ctx.Board.Scale, Ctx.Board.transform.position);

            var transform = Projectile.transform;
            var remainingDistance = Vector3.Distance(transform.position, targetPosition);
            var variableDistance = remainingDistance * AnimationVariableSpeed;
            var distance = (variableDistance + AnimationFixedSpeed) * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, distance);

            return transform.position == targetPosition;
        }
    }
}