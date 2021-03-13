#nullable enable

using UnityEngine;

using Zongband.Game.Actions;
using Zongband.Game.Entities;
using Zongband.Utils;

namespace Zongband.Game.Controllers
{
    public class AIController : Controller
    {
        public override Action? ProduceAction(Agent agent, Action.Context ctx)
        {
            var action = AttackAdjacent(agent, ctx);
            if (action != null) return action;
            return ProduceRandomMovement(agent, ctx);
        }

        private Action? AttackAdjacent(Agent agent, Action.Context ctx)
        {
            var directions = Tile.RandomizedDirections();
            Agent? selectedTarget = null;
            foreach (var direction in directions)
            {
                var target = ctx.Board.GetAgent(agent, direction, true);
                if (target != null && target.IsPlayer != agent.IsPlayer)
                {
                    selectedTarget = target;
                    break;
                }
            }

            if (selectedTarget == null) return null;

            return new AttackAction(agent, selectedTarget, ctx);
        }

        private Action ProduceRandomMovement(Agent agent, Action.Context ctx)
        {
            if (!agent.IsRoamer) return new NullAction();

            var directions = Tile.RandomizedDirections();
            var selectedDirection = Tile.Zero;
            foreach (var direction in directions)
            {
                if (ctx.Board.IsTileAvailable(agent, direction, true))
                {
                    selectedDirection = direction;
                    break;
                }
            }

            if (selectedDirection == Tile.Zero) return new NullAction();

            return new MovementAction(agent, selectedDirection, true, ctx);
        }
    }
}