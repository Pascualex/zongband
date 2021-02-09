#nullable enable

using UnityEngine;

using Zongband.Game.Core;
using Zongband.Utils;

namespace Zongband.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Camera? mainCamera;
        [SerializeField] private TileHighlighter? tileHighlighter;

        public void Refresh()
        {
            tileHighlighter?.Refresh();
        }

        public void SetMousePosition(Vector2 mousePosition)
        {
            if (mainCamera == null) return;

            var ray = mainCamera.ScreenPointToRay(mousePosition);
            var plane = new Plane(Vector3.up, Vector3.zero);
            
            var boardPosition = new Vector2Int(-1, -1);
            if (plane.Raycast(ray, out var distance))
            {
                var position = ray.GetPoint(distance);
                boardPosition = new Vector2Int((int)position.x, (int)position.z);
            }

            if (tileHighlighter != null) tileHighlighter.boardPosition = boardPosition;
        }
    }
}