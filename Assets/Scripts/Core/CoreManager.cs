#nullable enable

using UnityEngine;
using System;

using Zongband.Input;
using Zongband.UI;
using Zongband.Game.Core;

namespace Zongband.Core
{
    public class CoreManager : MonoBehaviour
    {
        [SerializeField] private InputManager? inputManager;
        [SerializeField] private UIManager? uiManager;
        [SerializeField] private GameManager? gameManager;

        private void Start()
        {
            gameManager?.SetupExample();
        }

        private void Update()
        {
            if (inputManager == null) throw new ArgumentNullException(nameof(inputManager));
            if (gameManager == null) throw new ArgumentNullException(nameof(gameManager));
            if (uiManager == null) throw new ArgumentNullException(nameof(uiManager));
            
            inputManager.ProcessInput();
            gameManager.GameLoop();
            uiManager.Refresh();
            inputManager.ClearInput();
        }
    }
}
