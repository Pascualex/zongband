#nullable enable

using UnityEngine;

using Zongband.Game.Abilities;
using Zongband.Game.Actions;
using Zongband.Game.Entities;

namespace Zongband.Game.Controllers
{
    public class PlayerController : Controller
    {
        public PlayerAction? PlayerAction { private get; set; }
        public bool SkipTurn { private get; set; } = false;
        public AbilitySO? AbilitySO;

        public override Action? ProduceAction(Agent agent, Action.Context ctx)
        {
            var agentAction = ProduceMovementOrAttack(agent, ctx);
            if (agentAction == null && SkipTurn) agentAction = new NullAction();

            Clear();
            return agentAction;
        }

        public void Clear()
        {
            PlayerAction = null;
            SkipTurn = false;
        }

        private Action? ProduceMovementOrAttack(Agent agent, Action.Context ctx)
        {
            if (PlayerAction == null) return null;

            var tile = PlayerAction.Tile;
            var relative = PlayerAction.Relative;
            var canAttack = PlayerAction.CanAttack;

            var distance = relative ? tile.GetDistance() : tile.GetDistance(agent.Tile);
            var instant = distance > 1;
            var isTileAvailable = ctx.Board.IsTileAvailable(agent, tile, relative);
            if (isTileAvailable) return new MoveAction(agent, tile, relative, ctx, instant);

            var targetAgent = ctx.Board.GetAgent(agent, tile, relative);
            if (canAttack && targetAgent != agent && targetAgent != null)
            {
                if (AbilitySO == null) return null;
                return AbilitySO.CreateAction(agent, targetAgent, ctx);
            }

            return null;
        }
    }
}
