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

        InteractableUI interactableUI;
        public GameObject interactableUIGameObject;
        public GameObject IteminteractableUIGameObject;
        public LayerMask interactableLayerMask;

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
            interactableUI = FindObjectOfType<InteractableUI>();
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

            CheckForInteractableObject(); // constantly check for interactable objeect
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
            // reset flagfs for animations at end of each frame for ONE time register
            inputHandler.rollFlag = false; 
            inputHandler.twerkFlag = false;
            inputHandler.sprintFlag = false;
            inputHandler.jumpFlag = false;

            // reset input bools
            inputHandler.rb_Input = false;
            inputHandler.rt_Input = false;
            inputHandler.d_Pad_Up = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.d_Pad_Right = false;
            inputHandler.a_Input = false;
            //isSprinting = inputHandler.b_Input; // whenever you hold 'b' button, sprinting will be true, otherwise false

            // increment inAirTimer if player is in the Air
            if(isInAir)
            {
                playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
            }

        }

        //Function to constantly check for interactable items within the world
        public void CheckForInteractableObject()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 0.95f, interactableLayerMask); 
            foreach (Collider collider in colliders)
            {
                if (collider.tag == "Interactable")
                {
                    Debug.Log("Touching an Interacting Object!");
                    Interactable interactableObject = collider.GetComponent<Interactable>();

                    if (interactableObject != null)
                    {
                        string interactableText = interactableObject.interactableText;
                        // SET UI Text To the Interactable objetcts text.
                        interactableUI.interactableText.text = interactableText;
                        interactableUIGameObject.SetActive(true);
                        //enable UI pop up!

                        // if player presses pick-up button inside ray hit
                        if (inputHandler.a_Input)
                        {
                            //call the interact function to pick up item
                            collider.GetComponent<Interactable>().Interact(this);
                        }
                    }
                }
            }
            
            // checkk if there are not coliider hits, and disable pop up screen for text interaction
            if(interactableUIGameObject != null && colliders.Length < 1)
            {
                interactableUIGameObject.SetActive(false);
            }

            // for item pick up, disable item name poop up after they press 'a' again
            if (IteminteractableUIGameObject != null && colliders.Length < 1 && inputHandler.a_Input)
            {
                IteminteractableUIGameObject.SetActive(false);
            }
            #region Old raycast for Items. was being finicky
            //// cast a ray 1 unit out fornt of player with radius of 1, ignore all layers NOT tagged "Interactable"
            //if (Physics.SphereCast(transform.position, 0.95f, transform.forward, out hit, 0.75f, interactableLayerMask))
            //{
            //    if(hit.collider.tag == "Interactable")
            //    {
            //        Debug.Log("Touching an Interacting Object!");
            //        Interactable interactableObject = hit.collider.GetComponent<Interactable>();
            //
            //        if(interactableObject != null)
            //        {
            //            string interactableText = interactableObject.interactableText;
            //            // SET UI Text To the Interactable objetcts text.
            //            //enable UI pop up!
            //
            //            // if player presses pick-up button inside ray hit
            //            if(inputHandler.a_Input)
            //            {
            //                //call the interact function to pick up item
            //                hit.collider.GetComponent<Interactable>().Interact(this);
            //            }
            //        }
            //    }
            //}
            #endregion
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position + transform.forward * 0.75f, 0.95f);
        }
    }
}
