#nullable enable

using UnityEngine;

using Zongband.Game.Core;
using Zongband.Utils;

namespace Zongband.UI
{
    public class TileHighlighter : MonoBehaviour
    {
        public Tile MouseTile { private get; set; } = Tile.MinusOne;

        [SerializeField] private GameManager? gameManager;
        [SerializeField] private TileHighlighterCursor? initialCursorPrefab;
        private TileHighlighterCursor? cursor;

        private void Awake()
        {
            if (initialCursorPrefab != null) ChangeCursor(initialCursorPrefab);
        }

        public void Refresh()
        {
            HighlightTile();
        }

        public void ChangeCursor(TileHighlighterCursor cursorPrefab)
        {
            if (cursor != null) Destroy(cursor);
            cursor = Instantiate(cursorPrefab, transform);
            cursor.SetNone();
        }

        private void HighlightTile()
        {
            if (gameManager == null) return;
            if (cursor == null) return;

            var board = gameManager.board;
            if (board == null) return;

            var lastPlayer = gameManager.LastPlayer;
            if (lastPlayer != null && board.IsTileAvailable(lastPlayer, MouseTile, false))
            {
                var position = MouseTile.ToWorld(board.Scale, board.transform.position);
                cursor.transform.position = position;
                var distance = MouseTile.GetDistance(lastPlayer.tile);
                if (distance > 1) cursor.SetNormal();
                else cursor.SetWarning();
            }
            else cursor.SetNone();
        }
    }
}