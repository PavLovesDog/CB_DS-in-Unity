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

        private void Awake()
        {
        }

        void Start()
        {
            cameraHandler = CameraHandler.singleton;
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
        }
    
        void Update()
        {
            float delta = Time.deltaTime;

            // get state of bool from animator state
            isInteracting = anim.GetBool("isInteracting");
            
            inputHandler.TickInput(delta);
            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleRollingAndSprinting(delta);
            playerLocomotion.HandleTwerk(delta);
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
