#nullable enable

using UnityEngine;
using System;
using DG.Tweening;

using Zongband.Input;
using Zongband.UI;
using Zongband.Game.Core;

namespace Zongband.Core
{
    public class CoreManager : MonoBehaviour
    {
        [SerializeField] private InputManager? InputManager;
        [SerializeField] private UIManager? UIManager;
        [SerializeField] private GameManager? GameManager;

        private void Start()
        {
            if (GameManager == null) throw new ArgumentNullException(nameof(GameManager));

            DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
            DOTween.defaultUpdateType = UpdateType.Manual;

            GameManager.SetupExample2();
        }

        private void Update()
        {
            if (InputManager == null) throw new ArgumentNullException(nameof(InputManager));
            if (GameManager == null) throw new ArgumentNullException(nameof(GameManager));
            if (UIManager == null) throw new ArgumentNullException(nameof(UIManager));

            InputManager.ProcessInput();
            DOTween.ManualUpdate(Time.deltaTime, Time.unscaledDeltaTime);
            GameManager.GameLoop();
            UIManager.Refresh();
            InputManager.ClearInput();
        }
    }
}
