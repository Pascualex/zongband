#nullable enable

using UnityEngine;

using Zongband.Utils;

namespace Zongband.Game.Controllers
{
    public class PlayerAction
    {
        public readonly Tile Tile;
        public readonly bool Relative;
        public readonly bool CanAttack;
        public PlayerAction(Tile tile, bool relative, bool canAttack)
        {
            Tile = tile;
            Relative = relative;
            CanAttack = canAttack;
        }
    }
}
