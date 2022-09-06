using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB_DarkSouls
{
    public class PlayerLocomotion : MonoBehaviour
    {
        PlayerManager playerManager;
        Transform cameraObject;
        InputHandler inputHandler;
        public Vector3 moveDirection;

        [HideInInspector]
        public Transform myTransform;
        [HideInInspector]
        public AnimatorHandler animatorHandler;

        public new Rigidbody rigidbody;
        public GameObject normalCamera;

        [Header("Ground & Aire Detection Stats")]
        [SerializeField]
        float groundDetectionRayStartPoint = 0.5f; // beginning of raycast
        [SerializeField]
        float minimumDistanceNeededToBeginFall = 1f; // distance needed for player fall animation to start
        [SerializeField]
        float groundDirectionRayDistance = 0.2f; // offset raycast distance, if needed
        LayerMask ignoreForGroundCheck;
        public float inAirTimer;

        [Header("Movement Stats")]
        [SerializeField]
        float movementSpeed = 5.0f;
        [SerializeField]
        float walkingSpeed = 1f;
        [SerializeField]
        float sprintSpeed = 7.0f;
        [SerializeField]
        float rotationSpeed = 10.0f;
        [SerializeField]
        float fallingSpeed = 45f;
        [SerializeField] // this varible abd below doesn't really need to be shown in editor..
        float fallVelocity;
        [SerializeField]
        float gravityIntesity = 9.8f;



        void Start()
        {
            playerManager = GetComponent<PlayerManager>();
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            cameraObject = Camera.main.transform;
            myTransform = transform;
            animatorHandler.Initialize();

            //start player on ground upon startup
            playerManager.isGrounded = true;
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
            fallVelocity = fallingSpeed; // set up fall speed variable

        }

        #region Movement
        Vector3 normalVector;
        Vector3 targetPosition;

        //Handle the players Rotation
        private void HandleRotation(float delta)
        {
            Vector3 targetDirection = Vector3.zero;
            float moveOverride = inputHandler.moveAmount;

            // Set look directions based on input
            targetDirection = cameraObject.forward * inputHandler.vertical;
            targetDirection += cameraObject.right * inputHandler.horizontal;

            targetDirection.Normalize();
            targetDirection.y = 0; // ensure no y-axis rotation will be happening

            if (targetDirection == Vector3.zero)
                targetDirection = myTransform.forward;

            float rs = rotationSpeed;
            
            //apply rotation
            Quaternion tr = Quaternion.LookRotation(targetDirection);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

            myTransform.rotation = targetRotation;
        }

        public void HandleMovement(float delta)
        {
            // cannot interrupt rolls with added movement
            if (inputHandler.rollFlag)
                return;

            //// cant move if falling
            //if (playerManager.isInteracting)
            //    return;

            //assign movement from input Handler
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0; // this\ll need to be changed for jumping...

            float speed = movementSpeed;

            if(inputHandler.sprintFlag)
            {
                speed = sprintSpeed;
                playerManager.isSprinting = true;
                moveDirection *= speed;
            }
            else
            {
                moveDirection *= speed;
            }

            //set movement along plane
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity;

            animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);

            // check for rotation
            if (animatorHandler.canRotate)
            {
                HandleRotation(delta);
            }
        }

        public void HandleRollingAndSprinting(float delta)
        {
            //do this so we cannot roll whenever, when other actions are happening
            if (animatorHandler.anim.GetBool("isInteracting"))
                return;

            //if we Can roll, calulate roll direction
            if (inputHandler.rollFlag)
            {
                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;

                //if you have any movement at all when roll button is pressed
                if(inputHandler.moveAmount > 0)
                {
                    // play target animation Roll in dirtection of movemtm
                    animatorHandler.PlayTargetAnimation("Rolling", true);
                    moveDirection.y = 0; // not flying up or down

                    // rotate into direction we are rolling
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = rollRotation;
                }
                else // if no movement on button press
                {
                    //play back dodge animation
                    animatorHandler.PlayTargetAnimation("Backstep", true);
                }
            }
        }

        public void HandleTwerk(float delta)
        {
            if (animatorHandler.anim.GetBool("isInteracting"))
                return;

            // if button been pressed
            if(inputHandler.twerkFlag)
            {
                animatorHandler.PlayTargetAnimation("Twerk", true);
            }
        }

        public void HandleFalling(float delta, Vector3 moveDirection)
        {
            playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = myTransform.position;
            origin.y += groundDetectionRayStartPoint; // start ray at base of player collider

            // if raycast jits something directly infront, your not moving
            if (Physics.Raycast(origin, myTransform.forward, out hit, 0.04f))
            {
                moveDirection = Vector3.zero;
            }

            if(playerManager.isInAir)
            {
                //fallVelocity = fallingSpeed;
                //fallVelocity += (Time.deltaTime * gravityIntesity); // increse fall rate
                fallVelocity += delta * gravityIntesity; // increse fall rate
                rigidbody.AddForce((-Vector3.up * fallVelocity) + moveDirection); // make player fall at rate of falling spoeed
                rigidbody.AddForce(moveDirection.normalized * 4f, ForceMode.Impulse); //OPTIONAL: if walk of ledge, it pushes you off a little so players don't get stuck
            }

            Vector3 dir = moveDirection;
            dir.Normalize();
            origin = origin + dir * groundDirectionRayDistance;

            targetPosition = myTransform.position;

            // draw ray for debugging
            Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
            if(Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, ignoreForGroundCheck))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                playerManager.isGrounded = true;
                targetPosition.y = tp.y; // if ray comes out and hits something, we are grounded

                if(playerManager.isInAir)
                {
                    // if fell for alloted time
                    if(inAirTimer > 0.15f)
                    {
                        Debug.Log("you were in the air for " + inAirTimer);
                        animatorHandler.PlayTargetAnimation("Land", true); // play animation
                        inAirTimer = 0;
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation("Locomotion", false);
                        inAirTimer = 0;
                    }

                    playerManager.isInAir = false;
                    fallVelocity = fallingSpeed; // reset fall velocity for next fall
                }
            }
            else
            {
                // if then player leaves ground, switch bool
                if(playerManager.isGrounded)
                {
                    playerManager.isGrounded = false;
                }

                //if wern't in air, play falling animation and reset bool
                if (playerManager.isInAir == false)
                {
                    if(playerManager.isInteracting == false)
                    {
                        animatorHandler.PlayTargetAnimation("Falling", true);
                    }

                    // get current velocity
                    Vector3 vel = rigidbody.velocity;
                    vel.Normalize();
                    rigidbody.velocity = vel * (movementSpeed / 2);
                    playerManager.isInAir = true;

                }
            }

            if (playerManager.isGrounded)
            {
                if(playerManager.isInteracting || inputHandler.moveAmount > 0)
                {
                    myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime);
                }
                else
                {
                    myTransform.position = targetPosition;
                }
            }
        }
        #endregion

    }
}
