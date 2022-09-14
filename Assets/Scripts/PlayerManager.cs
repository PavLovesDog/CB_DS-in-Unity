using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB_DarkSouls
{
    public class PlayerManager : MonoBehaviour
    {
        InputHandler inputHandler;
        Animator anim;
        CameraHandler cameraHandler;
        PlayerLocomotion playerLocomotion;

        // flag type bool
        public bool isInteracting;

        [Header("Player Flags")]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool canDoCombo;

        private void Awake()
        {
        }

        void Start()
        {
            //cameraHandler = CameraHandler.singleton;
            cameraHandler = FindObjectOfType<CameraHandler>(); // note* using FindType assumes there is only ONE camera in scene.
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
        }
    
        void Update()
        {
            float delta = Time.deltaTime;

            // link bools, get state of bool from animator state
            isInteracting = anim.GetBool("isInteracting");
            canDoCombo = anim.GetBool("canDoCombo");
            
            inputHandler.TickInput(delta);
            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleRollingAndSprinting(delta);
            playerLocomotion.HandleJumpAndDance(delta, playerLocomotion.moveDirection);
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
        }

        // Fixed update controls Camera Follow operations
        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }
        }

        // late update speecifically for resetting flags
        private void LateUpdate()
        {
            inputHandler.rollFlag = false; // reset flagfs for animations at end of each frame for ONE time register
            inputHandler.twerkFlag = false;//     "  "        "   "
            inputHandler.sprintFlag = false;
            inputHandler.jumpFlag = false;

            inputHandler.rb_Input = false;
            inputHandler.rt_Input = false;
            //isSprinting = inputHandler.b_Input; // whenever you hold 'b' button, sprinting will be true, otherwise false

            // increment inAirTimer if player is in the Air
            if(isInAir)
            {
                playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
            }
        }

    }
}
