#nullable enable

using UnityEngine;

using Zongband.Game.Commands;
using Zongband.Game.Entities;
using Zongband.Utils;

namespace Zongband.Game.Controllers
{
    public class PlayerController : Controller
    {
        public PlayerCommand? PlayerCommand { private get; set; }
        public bool SkipTurn { private get; set; } = false;

        public override Command? ProduceCommand(Agent agent, Command.Context ctx)
        {
            var agentCommand = ProduceMovementOrAttack(agent, ctx);
            if (agentCommand == null && SkipTurn) agentCommand = new NullCommand();

            Clear();
            return agentCommand;
        }

        public void Clear()
        {
            PlayerCommand = null;
            SkipTurn = false;
        }

        private Command? ProduceMovementOrAttack(Agent agent, Command.Context ctx)
        {
            if (PlayerCommand == null) return null;

            var tile = PlayerCommand.Tile;
            var relative = PlayerCommand.Relative;
            var canAttack = PlayerCommand.CanAttack;

            var distance = relative ? tile.GetDistance() : tile.GetDistance(agent.Tile);
            var instant = distance > 1;
            var isTileAvailable = ctx.Board.IsTileAvailable(agent, tile, relative);
            if (isTileAvailable) return new MovementCommand(agent, tile, relative, ctx, instant);

            var targetAgent = ctx.Board.GetAgent(agent, tile, relative);
            if (canAttack && targetAgent != agent && targetAgent != null)
            {
                return new AttackCommand(agent, targetAgent, ctx);
            }

            return null;
        }
    }
}
