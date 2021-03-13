#nullable enable

using UnityEngine;
using System;

using Zongband.Game.Core;

namespace Zongband.UI
{
    public class CameraController : MonoBehaviour
    {
        public Vector3 CameraOffset = new Vector3(0f, 10f, 0f);
        public float FixedSpeed = 1f;
        public float VariableSpeed = 15f;

        [SerializeField] private Camera? MainCamera;
        [SerializeField] private GameManager? GameManager;

        public void Refresh()
        {
            MoveCamera();
        }

        public void MoveCamera()
        {
            if (MainCamera == null) throw new ArgumentNullException(nameof(MainCamera));
            if (GameManager == null) throw new ArgumentNullException(nameof(GameManager));
            if (GameManager.Board == null) throw new ArgumentNullException(nameof(GameManager.Board));

            var lastPlayer = GameManager.LastPlayer;
            if (lastPlayer == null) return;

            var board = GameManager.Board;
            var playerPosition = lastPlayer.Tile.ToWorld(board.Scale, board.transform.position);
            var targetPosition = playerPosition + CameraOffset;

            var transform = MainCamera.transform;
            var remainingDistance = Vector3.Distance(transform.position, targetPosition);
            var variableDistance = remainingDistance * VariableSpeed;
            var distance = (variableDistance + FixedSpeed) * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, distance);
        }
    }
}