#nullable enable

using UnityEngine;
using System;

using Zongband.Game.Core;

namespace Zongband.UI
{
    public class CameraController : MonoBehaviour
    {
        public Vector3 cameraOffset = new Vector3(0f, 10f, 0f);
        public float fixedSpeed = 1f;
        public float variableSpeed = 15f;

        [SerializeField] private Camera? mainCamera;
        [SerializeField] private GameManager? gameManager;

        public void Refresh()
        {
            MoveCamera();
        }

        public void MoveCamera()
        {
            if (mainCamera == null) throw new ArgumentNullException(nameof(mainCamera));
            if (gameManager == null) throw new ArgumentNullException(nameof(gameManager));
            if (gameManager.board == null) throw new ArgumentNullException(nameof(gameManager.board));

            var lastPlayer = gameManager.LastPlayer;
            if (lastPlayer == null) return;

            var board = gameManager.board;
            var playerPosition = lastPlayer.tile.ToWorld(board.Scale, board.transform.position);
            var targetPosition = playerPosition + cameraOffset;

            var transform = mainCamera.transform;
            var remainingDistance = Vector3.Distance(transform.position, targetPosition);
            var variableDistance = remainingDistance * variableSpeed;
            var distance = (variableDistance + fixedSpeed) * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, distance);
        }
    }
}