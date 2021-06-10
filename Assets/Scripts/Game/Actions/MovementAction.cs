#nullable enable

using UnityEngine;

using Zongband.Game.Boards;
using Zongband.Game.Entities;
using Zongband.Utils;

namespace Zongband.Game.Actions
{
    public class MovementAction : Action
    {
        private const float AnimationFixedSpeed = 1f;
        private const float AnimationVariableSpeed = 15f;

        private readonly Entity Entity;
        private readonly Tile Tile;
        private readonly bool Relative;
        private readonly Context Ctx;
        private readonly bool Instant;

        public MovementAction(Entity entity, Tile tile, bool relative, Context ctx)
        : this(entity, tile, relative, ctx, false) { }

        public MovementAction(Entity entity, Tile tile, bool relative, Context ctx, bool instant)
        {
            Entity = entity;
            Tile = tile;
            Relative = relative;
            Ctx = ctx;
            Instant = instant;
        }

        protected override bool ProcessStart()
        {
            if (!Entity) return true;

            var oldTile = Entity.Tile;

            if (!MoveInBoard()) return true;
            FaceTowardsDirection(oldTile);

            if (Instant)
            {
                MoveToTargetInWorld();
                return true;
            }

            return false;
        }

        protected override bool ProcessUpdate()
        {
            if (!Entity) return true;

            return MoveTowardsTarget();
        }

        private void FaceTowardsDirection(Tile oldTile)
        {
            var tileDirection = Entity.Tile - oldTile;
            var direction = tileDirection.ToWorld();
            Entity.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        private bool MoveInBoard()
        {
            if (!Ctx.Board.IsTileAvailable(Entity, Tile, Relative)) return false;
            Ctx.Board.Move(Entity, Tile, Relative);
            return true;
        }

        private void MoveToTargetInWorld()
        {
            var targetPosition = Entity.Tile.ToWorld(Ctx.Board.Scale, Ctx.Board.transform.position);
            Entity.transform.position = targetPosition;
        }

        private bool MoveTowardsTarget()
        {
            var targetPosition = Entity.Tile.ToWorld(Ctx.Board.Scale, Ctx.Board.transform.position);

            var transform = Entity.transform;
            var remainingDistance = Vector3.Distance(transform.position, targetPosition);
            var variableDistance = remainingDistance * AnimationVariableSpeed;
            var distance = (variableDistance + AnimationFixedSpeed) * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, distance);

            return transform.position == targetPosition;
        }
    }
}