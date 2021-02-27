#nullable enable

using UnityEngine;

using Zongband.Utils;

namespace Zongband.Game.Controllers
{
    public class PlayerAction
    {
        public readonly Tile tile;
        public readonly bool relative;
        public readonly bool canAttack;
        public PlayerAction(Tile tile, bool relative, bool canAttack)
        {
            this.tile = tile;
            this.relative = relative;
            this.canAttack = canAttack;
        }
    }
}
