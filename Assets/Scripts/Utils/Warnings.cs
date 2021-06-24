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
        public const string RowsFull =
            "Rows could not advance because they exceed the maximum height";

        public static string Tile(Tile tile)
        {
            return "In tile at tile " + tile;
        }

        public static string Field(string path)
        {
            return $"\"{path}\" field not found";
        }
    }
}
