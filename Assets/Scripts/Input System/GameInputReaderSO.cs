using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Match3
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "ScriptableObject/InputReader")]
    public class GameInputReaderSO : ScriptableObject, GameInput.IGameplayActions
    {
        private GameInput _gameInput;
        public event Action ClickEvent;
        public event Action TouchEvent;
        public void Initialization()
        {
            if (_gameInput == null)
            {
                _gameInput = new GameInput();
                _gameInput.Gameplay.SetCallbacks(this);
                Debug.Log("Init Input");
                SetGameplay();
            }

        }

        private void SetGameplay()
        {
            _gameInput.Gameplay.Enable();
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                ClickEvent?.Invoke();
                Debug.Log("Click-click");
            }
        }

        public void OnTouch(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                TouchEvent?.Invoke();
            }
        }
    }
}
