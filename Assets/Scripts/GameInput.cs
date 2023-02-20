using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace adefagia
{
    public class GameInput : MonoBehaviour
    {
        public event EventHandler OnInteractAction;

        void Awake() 
        {
            PlayerInputActions playerInputActions = new PlayerInputActions();
            playerInputActions.Player.Enable();

            playerInputActions.Player.OnClickPlayer.performed += Interact_performed;
        }

        private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            OnInteractAction?.Invoke(this, EventArgs.Empty);
        }
    }
}
