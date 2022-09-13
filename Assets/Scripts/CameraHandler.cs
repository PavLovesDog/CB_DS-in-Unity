using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB_DarkSouls
{
    public class CameraHandler : MonoBehaviour
    {
        public Transform targetTransform; // transform camera will go to
        public Transform cameraTransform; // trabsform of ACTUAL camera
        public Transform cameraPivotTransform; // how the camera swivels
        private Transform myTransform;
        private Vector3 cameraTransformPosition; // position of camera
        private LayerMask ignoreLayers; // used for camera collision
        private Vector3 cameraFollowVelocity = Vector3.zero;

        public static CameraHandler singleton;

        public float lookSpeed = 0.1f;
        public float followSpeed = 0.1f;
        public float pivotSpeed = 0.03f;
        public float offsetXAmount = 0.35f;

        private Vector3 cameraOffset;

        private float targetPosition;
        private float defaultPosition;
        private float lookAngle;
        private float pivotAngle;
        public float minimumPivot = -35;
        public float maximumPivot = 35;

        public float cameraSphereRadius = 0.2f;
        public float cameraCollisionOffset = 0.2f;
        public float minimumCollisionOffset = 0.2f;


        private void Awake()
        {
            singleton = this;
            myTransform = transform; // myTransfomr is equal to transform of this game object
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
            targetTransform = FindObjectOfType<PlayerManager>().transform;
            Application.targetFrameRate = 60;
        }


        //Follow target transform (i.e the player)
        public void FollowTarget(float delta)
        {
            //Vector3 targetPosition = Vector3.Lerp(myTransform.position, targetTransform.position, delta / followSpeed);

            // target speed equals a linear interpolation between cameras current transform & target transform
            Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position, 
                                                        targetTransform.position, 
                                                        ref cameraFollowVelocity, 
                                                        delta / followSpeed);

            //Add camera offset based on camera Actual position
            //TODO camera is always adding a positiv amouunt, so screen switches upon turning arounbd. this is cool BUT
            // could cast a ray or use sine to just check which direction facing, and add offset accordingly
            cameraOffset = new Vector3(cameraTransform.localPosition.x + offsetXAmount, 0.0f, 0.0f);

            myTransform.position = targetPosition + cameraOffset;

            HandleCameraCollisions(delta);
        }

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            lookAngle += (mouseXInput * lookSpeed) / delta;
            pivotAngle -= (mouseYInput * pivotSpeed) / delta;
            pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot); // clamp camera pivot

            Vector3 rotation = Vector3.zero;
            rotation.y = lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            myTransform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = pivotAngle;

            targetRotation = Quaternion.Euler(rotation);

            cameraPivotTransform.localRotation = targetRotation;
        }

        private void HandleCameraCollisions(float delta)
        {
            // set up raycats and direction to cast ray
            targetPosition = defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            // check if cast collides with something within the distance
            if(Physics.SphereCast(cameraPivotTransform.position, 
                                  cameraSphereRadius, 
                                  direction, 
                                  out hit, 
                                  Mathf.Abs(targetPosition), 
                                  ignoreLayers))
            {
                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point); // find distance of hit
                targetPosition = -(dis - cameraCollisionOffset); // set new camera position with offset
            }

            // check
            if(Mathf.Abs(targetPosition) < minimumCollisionOffset)
            {
                targetPosition = -minimumCollisionOffset;
            }

            // lerp between new found target position and old position
            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition;
        }
    }
}
