#nullable enable

using UnityEngine;
using DG.Tweening;
using System;

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
        private GameObject? Projectile = null;
        private Tweener? Tweener = null;

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
            if (Caster != null && !CheckAlive(Caster)) return true;
            if (Target != null && !CheckAlive(Target)) return true;

            if (Caster != null) Start = Caster.Tile;
            if (Target != null) Finish = Target.Tile;

            if (Start == Finish) return true;
            if (Prms.Duration <= 0f) return true;

            Projectile = CreateProjectile();
            if (Projectile == null) return true;
            Tweener = TweenToTarget(Projectile);
            return false;
        }

        protected override bool ExecuteUpdate()
        {
            var completed = Tweener == null || !Tweener.IsActive();
            if (completed) DestroyProjectile();
            return completed;
        }

        private GameObject? CreateProjectile()
        {
            if (Prms.ProjectilePrefab == null) 
            {
                Debug.LogWarning(Warnings.ParameterIsNull);
                return null;
            }

            var prefab = Prms.ProjectilePrefab;
            var position = Start.ToWorld(Ctx.Board.Scale, Ctx.Board.transform.position);
            var tileDirection = Finish - Start;
            var direction = tileDirection.ToWorld();
            var rotation = Quaternion.LookRotation(direction, Vector3.up);

            return GameObject.Instantiate(prefab, position, rotation).gameObject;
        }

        private void DestroyProjectile()
        {
            if (Projectile != null) GameObject.Destroy(Projectile);
        }

        private Tweener TweenToTarget(GameObject projectile)
        {
            var targetPosition = Finish.ToWorld(Ctx.Board.Scale, Ctx.Board.transform.position);
            return projectile.transform.DOMove(targetPosition, Prms.Duration)
                                       .SetEase(Prms.Ease)
                                       .SetDelay(Prms.Delay);
        }

        [Serializable]
        public class Parameters
        {
            public float Duration = 1f;
            public Ease Ease = Ease.Unset;
            public float Delay = 0f;
            public GameObject? ProjectilePrefab = null;

            public void OnValidate()
            {
                Duration = Math.Max(0f, Duration);
                Delay = Math.Max(0f, Delay);
            }
        }
    }
}