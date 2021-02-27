#nullable enable

using UnityEngine;

using Zongband.Game.Core;
using Zongband.Utils;

namespace Zongband.UI
{
    public class TileHighlighterCursor : MonoBehaviour
    {
        [SerializeField] private GameObject? normalCursor;
        [SerializeField] private GameObject? warnignCursor;

        private void Awake()
        {
            SetNormal();
        }

        public void SetNormal()
        {
            normalCursor?.SetActive(true);
            warnignCursor?.SetActive(false);
        }

        public void SetWarning()
        {
            normalCursor?.SetActive(false);
            warnignCursor?.SetActive(true);
        }

        public void SetNone()
        {
            normalCursor?.SetActive(false);
            warnignCursor?.SetActive(false);
        }
    }
}