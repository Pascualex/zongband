#nullable enable

using UnityEngine;

using Zongband.Utils;

namespace Zongband.Game.Boards
{
    [CreateAssetMenu(fileName = "Board", menuName = "ScriptableObjects/Board")]
    public class BoardSO : ScriptableObject
    {
        public Size Size = new Size(10, 10);
        public float Scale = 1f;

        private void OnValidate()
        {
            Size.X = Mathf.Max(0, Size.X);
            Size.Y = Mathf.Max(0, Size.Y);
            if (Scale <= 0) Scale = 1f;
        }
    }
}
