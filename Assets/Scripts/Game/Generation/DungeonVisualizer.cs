#nullable enable

using UnityEngine;
using System;

using Zongband.Game.Boards;

namespace Zongband.Game.Generation
{
    public class DungeonVisualizer : MonoBehaviour
    {
        public Board? Board;
        public DungeonData? DungeonData;
        public float RoomSphereRadius = 1f;

        private void OnDrawGizmosSelected()
        {
            if (Board == null) throw new ArgumentNullException(nameof(Board));
            if (DungeonData == null) return;

            foreach (var room in DungeonData.Rooms)
            {
                var origin = room.origin.ToWorld(Board.Scale, Board.transform.position);
                var center = origin + (new Vector3(room.size.x - 1, 0, room.size.y - 1) * Board.Scale / 2f);
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(center, RoomSphereRadius);
            }
        }
    }
}