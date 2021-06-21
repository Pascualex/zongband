#nullable enable

using UnityEngine;

using Zongband.Game.Commands;
using Zongband.Game.Entities;
using Zongband.Utils;

namespace Zongband.Game.Controllers
{
    public class AIController : Controller
    {
        public override Command? ProduceCommand(Agent agent, Command.Context ctx)
        {
            var command = AttackAdjacent(agent, ctx);
            if (command != null) return command;
            return ProduceRandomMovement(agent, ctx);
        }

        private Command? AttackAdjacent(Agent agent, Command.Context ctx)
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

            return new AttackCommand(agent, selectedTarget, ctx);
        }

        private Command ProduceRandomMovement(Agent agent, Command.Context ctx)
        {
            if (!agent.IsRoamer) return new NullCommand();

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

            if (selectedDirection == Tile.Zero) return new NullCommand();

            return new MoveCommand(agent, selectedDirection, true, ctx);
        }
    }
}