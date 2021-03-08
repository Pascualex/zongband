#nullable enable

using UnityEngine;

using Zongband.Utils;

namespace Zongband.Game.Boards
{
    [CreateAssetMenu(fileName = "Board", menuName = "ScriptableObjects/Board")]
    public class BoardSO : ScriptableObject
    {
        public Size size = new Size(10, 10);
        public float scale = 1f;

        private void OnValidate()
        {
            size.x = Mathf.Max(size.x, 0);
            size.y = Mathf.Max(size.y, 0);
            if (scale <= 0) scale = 1f;
        }
    }
}
