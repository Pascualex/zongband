#nullable enable

using UnityEngine;

using Zongband.Game.Actions;
using Zongband.Game.Entities;
using Zongband.Utils;

namespace Zongband.Game.Controllers
{
    public class AIController : Controller
    {
        public override Action? ProduceAction(Agent agent, Action.Context context)
        {
            var action = AttackAdjacent(agent, context);
            if (action != null) return action;
            return ProduceRandomMovement(agent, context);
        }

        private Action? AttackAdjacent(Agent agent, Action.Context context)
        {
            var directions = Tile.RandomizedDirections();
            Agent? selectedTarget = null;
            foreach (var direction in directions)
            {
                var target = context.board.GetAgent(agent, direction);
                if (target != null && target.isPlayer != agent.isPlayer)
                {
                    selectedTarget = target;
                    break;
                }
            }

            if (selectedTarget == null) return null;

            // TODO: change damage calculation
            return new AttackAction(agent, selectedTarget, 10);
        }

        private Action ProduceRandomMovement(Agent agent, Action.Context context)
        {
            if (!agent.IsRoamer) return new NullAction();

            var directions = Tile.RandomizedDirections();
            var selectedDirection = Tile.Zero;
            foreach (var direction in directions)
            {
                if (context.board.IsTileAvailable(agent, direction, true))
                {
                    selectedDirection = direction;
                    break;
                }
            }

            if (selectedDirection == Tile.Zero) return new NullAction();

            return new MovementAction(agent, selectedDirection, true, context);
        }
    }
}