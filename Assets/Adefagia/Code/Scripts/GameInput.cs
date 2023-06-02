using System;
using UnityEngine;

namespace Adefagia.Adefgia.Code.Scripts
{
    public class GameInput : MonoBehaviour
    {
        public event EventHandler OnInteractAction;

        void Awake() 
        {
            PlayerInputActions playerInputActions = new PlayerInputActions();
            playerInputActions.Player.Enable();

            playerInputActions.Player.OnClickPlayer.performed += InteractPerformed;
        }

        private void InteractPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            OnInteractAction?.Invoke(this, EventArgs.Empty);
        }
    }
}
