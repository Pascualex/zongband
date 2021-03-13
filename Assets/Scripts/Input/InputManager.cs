#nullable enable

using UnityEngine;
using UnityEngine.InputSystem;
using System;

using Zongband.UI;
using Zongband.Game.Controllers;
using Zongband.Utils;

namespace Zongband.Input
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputManager : MonoBehaviour
    {
        public UIManager? UIManager;
        public PlayerController? PlayerController;

        public void ProcessInput()
        {
            InputSystem.Update();
        }

        public void ClearInput()
        {
            if (PlayerController == null) throw new ArgumentNullException(nameof(PlayerController));

            PlayerController.Clear();
        }

        private void OnMove(InputValue value)
        {
            if (PlayerController == null) throw new ArgumentNullException(nameof(PlayerController));
            var vector = value.Get<Vector2>();
            var direction = new Tile((int)vector.x, (int)vector.y);
            PlayerController.PlayerAction = new PlayerAction(direction, true, true);
        }

        private void OnSkipTurn(InputValue value)
        {
            if (PlayerController == null) throw new ArgumentNullException(nameof(PlayerController));
            var skipTurn = value.Get<float>() >= 0.5f;
            PlayerController.SkipTurn = skipTurn;
        }

        private void OnMouseMove(InputValue value)
        {
            if (UIManager == null) throw new ArgumentNullException(nameof(UIManager));

            var mousePosition = value.Get<Vector2>();
            UIManager.SetMousePosition(mousePosition);
        }

        private void OnMouseClick()
        {
            if (UIManager == null) throw new ArgumentNullException(nameof(UIManager));
            if (PlayerController == null) throw new ArgumentNullException(nameof(PlayerController));

            UIManager.HandleMouseClick();
            var mouseTile = UIManager.MouseTile;
            PlayerController.PlayerAction = new PlayerAction(mouseTile, false, true);
        }

        private void OnCtrlMouseClick()
        {
            if (UIManager == null) throw new ArgumentNullException(nameof(UIManager));

            UIManager.HandleCtrlMouseClick();
        }
    }
}
