#nullable enable

using UnityEngine;

namespace Zongband.Utils
{
    public static class Warnings
    {
        public const string MissingEntityTransformer =
            "Entity was moved but doesn't have an EntityTransformer attached";
        public const string AgentNotAlive =
            "Action was not executed because an agent is not alive";

        public const string ParameterIsNull =
            "Action was not executed because a parameter is unset";

        public static string TileWarning(Tile tile)
        {
            return "In tile at tile " + tile;
        }
    }
}
