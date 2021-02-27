﻿#nullable enable

using UnityEngine;

using Zongband.Game.Actions;
using Zongband.Game.Entities;
using Zongband.Utils;

namespace Zongband.Game.Controllers
{
    public class PlayerController : Controller
    {
        public PlayerAction? PlayerAction { private get; set; }
        public bool SkipTurn { private get; set; } = false;

        public override Action? ProduceAction(Agent agent, Action.Context context)
        {
            var agentAction = ProduceMovementOrAttack(agent, context);
            if (agentAction == null && SkipTurn) agentAction = new NullAction();

            Clear();
            return agentAction;
        }

        public void Clear()
        {
            PlayerAction = null;
        }

        private Action? ProduceMovementOrAttack(Agent agent, Action.Context context)
        {
            if (PlayerAction == null) return null;

            var tile = PlayerAction.tile;
            var relative = PlayerAction.relative;
            var canAttack = PlayerAction.canAttack;

            var distance = relative ? tile.GetDistance() : tile.GetDistance(agent.tile);
            var instant = distance > 1;
            var isTileAvailable = context.board.IsTileAvailable(agent, tile, relative);
            if (isTileAvailable) return new MovementAction(agent, tile, relative, context, instant);

            var targetAgent = context.board.GetAgent(agent, tile, relative);
            if (canAttack && targetAgent != agent && targetAgent != null)
            {
                return new AttackAction(agent, targetAgent, context);
            }

            return null;
        }
    }
}
