using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CB_DarkSouls
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        PlayerControls inputActions;

        Vector2 movementInput;
        Vector2 cameraInput;

        public void OnEnable()
        {
            // if no inputs exist, create them
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                //assign movement from keyboard and mouse inputs for camera and movement
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            }

            // begin listening for inputs
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        //??
        public void TickInput(float delta)
        {
            MoveInput(delta);
        }

        // function to map input values
        private void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;

        }
    }
}
