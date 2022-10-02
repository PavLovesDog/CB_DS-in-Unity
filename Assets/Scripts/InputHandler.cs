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

        #region Key Input Bools & flags
        public bool a_Input; // interact button
        public bool t_Input; // random key for dancing
        public bool e_input; // jump
        public bool b_Input; // sprint/roll/backstep
        public bool rb_Input; // light attack
        public bool rt_Input; // heavy attack
        public bool d_Pad_Up; // invetory quickslots
        public bool d_Pad_Down;
        public bool d_Pad_Left;
        public bool d_Pad_Right;

        public bool rollFlag;
        public bool twerkFlag;
        public bool jumpFlag;
        public bool sprintFlag;
        public bool comboFlag;
        public float rollInputTimer;
        #endregion


        PlayerControls inputActions;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;
        PlayerManager playerManager;

        Vector2 movementInput;
        Vector2 cameraInput;

        private void Awake()
        {
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
        }

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
            HandleRollingAndSprintInput(delta);
            HandleJumpAndDanceInput(delta);
            HandleAttackInput(delta);
            HandleQuickSlotInput();
            HandleInteractingButtonInput();
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

        private void HandleRollingAndSprintInput(float delta)
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

        private void HandleJumpAndDanceInput(float delta)
        {
            //detect when key is pressed & turn bool to true by checking InputActions
            t_Input = inputActions.PlayerActions.Dance.phase == UnityEngine.InputSystem.InputActionPhase.Started;
            e_input = inputActions.PlayerActions.Jump.phase == UnityEngine.InputSystem.InputActionPhase.Started;

            if (t_Input)
            {
                twerkFlag = true;
            }

            if(e_input)
            {
                jumpFlag = true;
            }
        }

        private void HandleAttackInput(float delta)
        {
            inputActions.PlayerActions.RBLightAttack.performed += i => rb_Input = true;
            inputActions.PlayerActions.RTStrongAttack.performed += i => rt_Input = true;

            //RB handles the right hand weapons light attack
            if(rb_Input)
            {
                // handle combo inputs
                if(playerManager.canDoCombo)
                {
                    comboFlag = true;
                    playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                    comboFlag = false;
                }
                else // not doing combo, regular attack
                {
                    if (playerManager.isInteracting) return; // don't play again if attack already happening
                    if (playerManager.canDoCombo) return; // don't play again if expecting a combo
                    playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
                }
            }

            if(rt_Input)
            {
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            }

        }

        private void HandleQuickSlotInput()
        {
            //Map buttons with input manager
            inputActions.PlayerQuickSlots.DPadRight.performed += i => d_Pad_Right = true;
            inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;

            //check for presses
            if (d_Pad_Right)
            {
                playerInventory.ChangeRightWeapon();
            }
            else if(d_Pad_Left)
            {
                playerInventory.ChangeLeftWeapon();
            }
        }

        private void HandleInteractingButtonInput()
        {
            // listen for button press and mark true when button is depressed
            inputActions.PlayerActions.Interact.performed += i => a_Input = true;


        }
    }
}
