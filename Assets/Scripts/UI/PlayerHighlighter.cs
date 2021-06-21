#nullable enable

using UnityEngine;
using System;

using Zongband.Game.Core;

namespace Zongband.UI
{
    public class PlayerHighlighter : MonoBehaviour
    {
        [SerializeField] private GameManager? GameManager;
        [SerializeField] private GameObject? InitialCursorPrefab;

        private GameObject? Cursor;

        public void Awake()
        {
            if (InitialCursorPrefab != null) ChangeCursor(InitialCursorPrefab);
        }

        public void Refresh()
        {
            HighlightPlayer();
        }

        public void ChangeCursor(GameObject cursorPrefab)
        {
            if (Cursor != null) Destroy(Cursor);
            Cursor = Instantiate(cursorPrefab, transform);
            Cursor.gameObject.SetActive(false);
        }

        private void HighlightPlayer()
        {
            if (GameManager == null) throw new ArgumentNullException(nameof(GameManager));
            if (Cursor == null) throw new ArgumentNullException(nameof(Cursor));

            var highlight = false;
            var lastPlayer = GameManager.LastPlayer;
            if (lastPlayer != null && lastPlayer.IsAlive && lastPlayer.IsPlayer)
            {
                var gameModelContainer = lastPlayer.GameModelContainer;
                if (gameModelContainer == null) throw new ArgumentNullException(nameof(gameModelContainer));

                Cursor.transform.position = gameModelContainer.transform.position;
                highlight = true;
            }

            Cursor.gameObject.SetActive(highlight);
        }
    }
}
