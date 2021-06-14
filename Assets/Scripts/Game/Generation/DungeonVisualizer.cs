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

            Gizmos.color = Color.cyan;
            foreach (var room in DungeonData.Rooms)
            {
                var origin = room.Origin.ToWorld(Board.Scale, Board.transform.position);
                var center = origin + (new Vector3(room.Size.X - 1, 0, room.Size.Y - 1) * Board.Scale / 2f);
                center.y += Height;
                Gizmos.DrawSphere(center, RoomSphereRadius);
            }

            foreach (var tuple in DungeonData.Connections)
            {
                var roomA = tuple.Item1;
                var originA = roomA.Origin.ToWorld(Board.Scale, Board.transform.position);
                var centerA = originA + (new Vector3(roomA.Size.X - 1, 0, roomA.Size.Y - 1) * Board.Scale / 2f);
                centerA.y += Height;

                var roomB = tuple.Item2;
                var originB = roomB.Origin.ToWorld(Board.Scale, Board.transform.position);
                var centerB = originB + (new Vector3(roomB.Size.X - 1, 0, roomB.Size.Y - 1) * Board.Scale / 2f);
                centerB.y += Height;

                Gizmos.DrawLine(centerA, centerB);
            }
        }
    }
}