#nullable enable

using UnityEngine;

using Zongband.Game.Entities;

namespace Zongband.Game.Boards
{
    public class NotInTileException : TileException
    {
        public NotInTileException(Entity entity)
        : base(entity.Tile) { }
    }
}
