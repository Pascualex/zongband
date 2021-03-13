#nullable enable

using UnityEngine;
using System;

namespace Zongband.UI
{
    public class TileHighlighterCursor : MonoBehaviour
    {
        [SerializeField] private GameObject? NormalCursor;
        [SerializeField] private GameObject? WarningCursor;

        private void Awake()
        {
            SetNormal();
        }

        public void SetNormal()
        {
            if (NormalCursor == null) throw new ArgumentNullException(nameof(NormalCursor));
            if (WarningCursor == null) throw new ArgumentNullException(nameof(WarningCursor));

            NormalCursor.SetActive(true);
            WarningCursor.SetActive(false);
        }

        public void SetWarning()
        {
            if (NormalCursor == null) throw new ArgumentNullException(nameof(NormalCursor));
            if (WarningCursor == null) throw new ArgumentNullException(nameof(WarningCursor));

            NormalCursor.SetActive(false);
            WarningCursor.SetActive(true);
        }

        public void SetNone()
        {
            if (NormalCursor == null) throw new ArgumentNullException(nameof(NormalCursor));
            if (WarningCursor == null) throw new ArgumentNullException(nameof(WarningCursor));

            NormalCursor.SetActive(false);
            WarningCursor.SetActive(false);
        }
    }
}