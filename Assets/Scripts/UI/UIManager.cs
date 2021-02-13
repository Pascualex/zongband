#nullable enable

using UnityEngine;

using Zongband.Utils;

namespace Zongband.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Camera? mainCamera;
        [SerializeField] private TileHighlighter? tileHighlighter;
        [SerializeField] private PlayerHighlighter? playerHighlighter;
        [SerializeField] private AgentInspector? agentInspector;

        public void Refresh()
        {
            tileHighlighter?.Refresh();
            playerHighlighter?.Refresh();
            agentInspector?.Refresh();
        }

        public void SetMousePosition(Vector2 mousePosition)
        {
            if (mainCamera == null) return;

            var ray = mainCamera.ScreenPointToRay(mousePosition);
            var plane = new Plane(Vector3.up, Vector3.zero);
            
            var mouseTile = Tile.MinusOne;
            if (plane.Raycast(ray, out var distance))
            {
                var position = ray.GetPoint(distance);
                mouseTile = new Tile((int)position.x, (int)position.z);
            }

            if (tileHighlighter != null) tileHighlighter.MouseTile = mouseTile;
            if (agentInspector != null) agentInspector.MouseTile = mouseTile;
        }

        public void HandleMouseClick()
        {
            agentInspector?.HandleMouseClick();
        }
    }
}