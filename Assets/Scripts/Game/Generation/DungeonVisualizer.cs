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
        public float Height = 1f;

        private void OnDrawGizmosSelected()
        {
            if (Board == null) throw new ArgumentNullException(nameof(Board));
            if (DungeonData == null) return;

            foreach (var room in DungeonData.Rooms)
            {
                var origin = room.Origin.ToWorld(Board.Scale, Board.transform.position);
                var center = origin + (new Vector3(room.Size.X - 1, 0, room.Size.Y - 1) * Board.Scale / 2f);
                center.y += Height;
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(center, RoomSphereRadius);
            }
        }
    }
}