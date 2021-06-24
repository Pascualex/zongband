#nullable enable

using UnityEngine;
using System;

using Zongband.Game.Boards;
using Zongband.Game.Entities;
using Zongband.Utils;

namespace Zongband.Game.Actions
{
    public class ProjectileAction : Action
    {
        private Tile Start;
        private Tile Finish;
        private readonly Agent? Caster = null;
        private readonly Agent? Target = null;
        private readonly Parameters Prms;
        private readonly Context Ctx;
        private GameObject? Projectile;

        public ProjectileAction(Tile start, Tile finish, Parameters prms, Context ctx)
        : this(start, finish, null, null, prms, ctx)
        { }

        public ProjectileAction(Agent caster, Tile finish, Parameters prms, Context ctx)
        : this(Tile.Zero, finish, caster, null, prms, ctx)
        { }

        public ProjectileAction(Tile start, Agent target, Parameters prms, Context ctx)
        : this(start, Tile.Zero, null, target, prms, ctx)
        { }

        public ProjectileAction(Agent caster, Agent target, Parameters prms, Context ctx)
        : this(Tile.Zero, Tile.Zero, caster, target, prms, ctx)
        { }

        private ProjectileAction(Tile start, Tile finish, Agent? caster, Agent? target, Parameters prms, Context ctx)
        {
            Start = start;
            Finish = finish;
            Caster = caster;
            Target = target;
            Prms = prms;
            Ctx = ctx;
        }

        protected override bool ExecuteStart()
        {
            if ((Caster != null && !Caster.IsAlive) || (Target != null && !Target.IsAlive))
            {
                Debug.LogWarning(Warnings.AgentNotAlive);
                return true;
            }

            if (Caster != null) Start = Caster.Tile;
            if (Target != null) Finish = Target.Tile;

            if (Prms.Inverted)
            {
                var aux = Start;
                Start = Finish;
                Finish = aux;
            }

            if (!CreateProjectile())
            {
                Debug.LogWarning(Warnings.ParameterIsNull);
                return true;
            }

            return false;
        }

        protected override bool ExecuteUpdate()
        {
            if (!Prms.Inverted && Target != null && Target.IsAlive) Finish = Target.Tile;
            if (Prms.Inverted && Caster != null && Caster.IsAlive) Finish = Caster.Tile;

            var completed = MoveTowardsTarget();
            if (completed) DestroyProjectile();
            return completed;
        }

        private bool CreateProjectile()
        {
            if (Prms.ProjectilePrefab == null) return false;

            var prefab = Prms.ProjectilePrefab;
            var position = Start.ToWorld(Ctx.Board.Scale, Ctx.Board.transform.position);
            var tileDirection = Finish - Start;
            var direction = tileDirection.ToWorld();
            var rotation = Quaternion.LookRotation(direction, Vector3.up);

            Projectile = GameObject.Instantiate(prefab, position, rotation).gameObject;

            return true;
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
            var variableDistance = remainingDistance * Prms.VariableSpeed;
            var distance = (variableDistance + Prms.FixedSpeed) * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, distance);

            return transform.position == targetPosition;
        }

        [Serializable]
        public class Parameters
        {
            public float FixedSpeed = 0f;
            public float VariableSpeed = 0f;
            public bool Inverted = false;
            public GameObject? ProjectilePrefab = null;

            public void OnValidate()
            {
                FixedSpeed = Math.Max(0.1f, FixedSpeed);
                VariableSpeed = Math.Max(0f, VariableSpeed);
            }
        }
    }
}