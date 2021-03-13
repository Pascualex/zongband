#nullable enable

using UnityEngine;

namespace Zongband.Utils
{
    public static class Warnings
    {
        public const string MissingEntityTransformer =
            "Entity was moved but doesn't have an  EntityTransformer attached";

        public static string TileWarning(Tile tile)
        {
            return "In tile at tile " + tile;
        }
    }
}
