#nullable enable

using UnityEngine;
using System;

using Zongband.Game.Core;

namespace Zongband.UI
{
    public class PlayerHighlighter : MonoBehaviour
    {
        [SerializeField] private GameManager? gameManager;
        [SerializeField] private GameObject? initialCursorPrefab;

        private GameObject? cursor;

        public void Awake()
        {
            if (initialCursorPrefab != null) ChangeCursor(initialCursorPrefab);
        }

        public void Refresh()
        {
            HighlightPlayer();
        }

        public void ChangeCursor(GameObject cursorPrefab)
        {
            if (cursor != null) Destroy(cursor);
            cursor = Instantiate(cursorPrefab, transform);
            cursor.gameObject.SetActive(false);
        }

        private void HighlightPlayer()
        {
            if (gameManager == null) throw new ArgumentNullException(nameof(gameManager));
            if (cursor == null) throw new ArgumentNullException(nameof(cursor));

            var highlight = false;
            var lastPlayer = gameManager.LastPlayer;
            // TODO: check if dead
            if (lastPlayer != null && lastPlayer.isPlayer)
            {
                var gameModelContainer = lastPlayer.gameModelContainer;
                if (gameModelContainer == null) throw new ArgumentNullException(nameof(gameModelContainer));

                var parent = gameModelContainer.transform;
                cursor.transform.parent = parent;
                cursor.transform.localPosition = Vector3.zero;
                highlight = true;
            }

            if (!highlight) cursor.transform.parent = transform;
            cursor.gameObject.SetActive(highlight);
        }
    }
}
