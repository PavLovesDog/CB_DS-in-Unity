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

        public bool b_Input;
        public bool t_Input;

        public bool rollFlag;
        public bool twerkFlag;
        public bool sprintFlag;
        public float rollInputTimer;
        

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

        //recieve input data consistentaly
        public void TickInput(float delta)
        {
            MoveInput(delta);
            HandleRollingInput(delta);
            HandleTwerkInput(delta);
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

        private void HandleRollingInput(float delta)
        {
            //detect when key is pressed & turn bool to true
            b_Input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;

            if(b_Input)
            {
                rollInputTimer += delta;
                sprintFlag = true;
            }
            else
            {
                if(rollInputTimer > 0 && rollInputTimer < 0.5)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
        }

        private void HandleTwerkInput(float delta)
        {
            //detect when key is pressed & turn bool to true by checking InputActions
            t_Input = inputActions.PlayerActions.Dance.phase == UnityEngine.InputSystem.InputActionPhase.Started;

            if (t_Input)
            {
                twerkFlag = true;
            }
        }
    }
}
