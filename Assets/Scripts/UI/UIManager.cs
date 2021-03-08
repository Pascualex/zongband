#nullable enable

using UnityEngine;
using System;

using Zongband.Utils;

namespace Zongband.UI
{
    public class UIManager : MonoBehaviour
    {
        public Tile MouseTile { get; private set; } = Tile.MinusOne;

        [SerializeField] private Camera? mainCamera;
        [SerializeField] private CameraController? cameraController;
        [SerializeField] private TileHighlighter? tileHighlighter;
        [SerializeField] private PlayerHighlighter? playerHighlighter;
        [SerializeField] private AgentInspector? agentInspector;

        private Vector2 mousePosition = new Vector2(-1, -1);

        public void Refresh()
        {
            if (cameraController == null) throw new ArgumentNullException(nameof(cameraController));
            if (tileHighlighter == null) throw new ArgumentNullException(nameof(tileHighlighter));
            if (playerHighlighter == null) throw new ArgumentNullException(nameof(playerHighlighter));
            if (agentInspector == null) throw new ArgumentNullException(nameof(agentInspector));

            tileHighlighter.Refresh();
            playerHighlighter.Refresh();
            agentInspector.Refresh();

            cameraController.Refresh();
            UpdateMouseTile();
        }

        public void SetMousePosition(Vector2 mousePosition)
        {
            this.mousePosition = mousePosition;
            UpdateMouseTile();
        }

        public void HandleMouseClick()
        {

        }

        public void HandleCtrlMouseClick()
        {
            if (agentInspector == null) throw new ArgumentNullException(nameof(agentInspector));

            agentInspector.LockAgent();
        }

        private void UpdateMouseTile()
        {
            if (mainCamera == null) throw new ArgumentNullException(nameof(mainCamera));

            var ray = mainCamera.ScreenPointToRay(mousePosition);
            var plane = new Plane(Vector3.up, Vector3.zero);

            MouseTile = Tile.MinusOne;
            if (plane.Raycast(ray, out var distance))
            {
                var position = ray.GetPoint(distance);
                MouseTile = new Tile((int)position.x, (int)position.z);
            }

            if (tileHighlighter != null) tileHighlighter.MouseTile = MouseTile;
            if (agentInspector != null) agentInspector.MouseTile = MouseTile;
        }
    }
}