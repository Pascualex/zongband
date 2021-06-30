#nullable enable

using UnityEngine;
using DG.Tweening;
using System;

using Zongband.Game.Entities;
using Zongband.Utils;

namespace Zongband.Game.Actions
{
    public class MoveAction : Action
    {
        private readonly Entity Entity;
        private readonly Tile Tile;
        private readonly bool Relative;
        private readonly Context Ctx;
        private readonly Parameters Prms;
        private Tweener? Tweener = null;

        public MoveAction(Entity entity, Tile tile, bool relative, Parameters prms, Context ctx)
        {
            Entity = entity;
            Tile = tile;
            Relative = relative;
            Ctx = ctx;
            Prms = prms;
        }

        protected override bool ExecuteStart()
        {
            if (!CheckAlive(Entity)) return true;

            var oldTile = Entity.Tile;
            if (!MoveInBoard()) return true;
            FaceTowardsDirection(oldTile);

            if (Prms.Duration <= 0f) MoveToTarget();
            else Tweener = TweenToTarget();

            return Tweener == null;
        }

        protected override bool ExecuteUpdate()
        {
            if (Tweener == null) return true;

            return !Tweener.IsActive();
        }

        private bool MoveInBoard()
        {
            if (!Ctx.Board.IsTileAvailable(Entity, Tile, Relative)) return false;
            Ctx.Board.Move(Entity, Tile, Relative);
            return true;
        }

        private void FaceTowardsDirection(Tile oldTile)
        {
            var tileDirection = Entity.Tile - oldTile;
            var direction = tileDirection.ToWorld();
            Entity.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        private void MoveToTarget()
        {
            var targetPosition = Entity.Tile.ToWorld(Ctx.Board.Scale, Ctx.Board.transform.position);
            Entity.transform.position = targetPosition;
        }

        private Tweener TweenToTarget()
        {
            var targetPosition = Entity.Tile.ToWorld(Ctx.Board.Scale, Ctx.Board.transform.position);
            return Entity.transform.DOMove(targetPosition, Prms.Duration).SetEase(Prms.Ease);
        }

        [Serializable]
        public class Parameters
        {
            public float Duration = 1f;
            public Ease Ease = Ease.Unset;

            public void OnValidate()
            {
                Duration = Math.Max(0f, Duration);
            }
        }
    }
}