#nullable enable

using UnityEngine;
using System;

namespace Zongband.UI
{
    public class TileHighlighterCursor : MonoBehaviour
    {
        [SerializeField] private GameObject? normalCursor;
        [SerializeField] private GameObject? warningCursor;

        private void Awake()
        {
            SetNormal();
        }

        public void SetNormal()
        {
            if (normalCursor == null) throw new ArgumentNullException(nameof(normalCursor));
            if (warningCursor == null) throw new ArgumentNullException(nameof(warningCursor));

            normalCursor.SetActive(true);
            warningCursor.SetActive(false);
        }

        public void SetWarning()
        {
            if (normalCursor == null) throw new ArgumentNullException(nameof(normalCursor));
            if (warningCursor == null) throw new ArgumentNullException(nameof(warningCursor));

            normalCursor.SetActive(false);
            warningCursor.SetActive(true);
        }

        public void SetNone()
        {
            if (normalCursor == null) throw new ArgumentNullException(nameof(normalCursor));
            if (warningCursor == null) throw new ArgumentNullException(nameof(warningCursor));

            normalCursor.SetActive(false);
            warningCursor.SetActive(false);
        }
    }
}