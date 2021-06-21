#nullable enable

using UnityEngine;

using Zongband.Utils;

namespace Zongband.Game.Controllers
{
    public class PlayerCommand
    {
        public readonly Tile Tile;
        public readonly bool Relative;
        public readonly bool CanAttack;

        public PlayerCommand(Tile tile, bool relative, bool canAttack)
        {
            Tile = tile;
            Relative = relative;
            CanAttack = canAttack;
        }
    }
}
