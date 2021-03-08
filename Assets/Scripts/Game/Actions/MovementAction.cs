#nullable enable

using UnityEngine;

using Zongband.Game.Boards;
using Zongband.Game.Entities;
using Zongband.Utils;

namespace Zongband.Game.Actions
{
    public class MovementAction : Action
    {
        private const float animationFixedSpeed = 1f;
        private const float animationVariableSpeed = 15f;

        private readonly Entity entity;
        private readonly Tile tile;
        private readonly bool relative;
        private readonly Context ctx;
        private readonly bool instant;

        public MovementAction(Entity entity, Tile tile, bool relative, Context ctx)
        : this(entity, tile, relative, ctx, false) { }

        public MovementAction(Entity entity, Tile tile, bool relative, Context ctx, bool instant)
        {
            this.entity = entity;
            this.tile = tile;
            this.relative = relative;
            this.ctx = ctx;
            this.instant = instant;
        }

        protected override bool ProcessStart()
        {
            if (!entity) return true;

            var oldTile = entity.tile;

            if (!MoveInBoard()) return true;
            FaceTowardsDirection(oldTile);

            if (instant)
            {
                MoveToTargetInWorld();
                return true;
            }

            return false;
        }

        protected override bool ProcessUpdate()
        {
            if (!entity) return true;

            return MoveTowardsTarget();
        }

        private void FaceTowardsDirection(Tile oldTile)
        {
            var tileDirection = entity.tile - oldTile;
            var direction = tileDirection.ToWorldVector3();
            entity.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        private bool MoveInBoard()
        {
            if (!ctx.board.IsTileAvailable(entity, tile, relative)) return false;
            ctx.board.Move(entity, tile, relative);
            return true;
        }

        private void MoveToTargetInWorld()
        {
            var targetPosition = entity.tile.ToWorld(ctx.board.Scale, ctx.board.transform.position);
            entity.transform.position = targetPosition;
        }

        private bool MoveTowardsTarget()
        {
            var targetPosition = entity.tile.ToWorld(ctx.board.Scale, ctx.board.transform.position);

            var transform = entity.transform;
            var remainingDistance = Vector3.Distance(transform.position, targetPosition);
            var variableDistance = remainingDistance * animationVariableSpeed;
            var distance = (variableDistance + animationFixedSpeed) * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, distance);

            return transform.position == targetPosition;
        }
    }
}