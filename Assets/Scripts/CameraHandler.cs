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

        public static CameraHandler singleton;

        public float lookSpeed;
        public float followSpeed;
        public float pivotSpeed;

        private float defaultPosition;
        private float lookAngle;
        private float pivotAngle;
        public float minimumPivot = -35;
        public float maximumPivot = 35;


        private void Awake()
        {
            singleton = this;
            myTransform = transform; // myTransfomr is equal to transform of this game object
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
        }


        //Follow target transform (i.e the player)
        public void FollowTarget(float delta)
        {
            // target speed equals a linear interpolation between cameras current transform & target transform
            Vector3 targetPosition = Vector3.Lerp(myTransform.position, targetTransform.position, delta / followSpeed);
            myTransform.position = targetPosition;
        }

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {

        }
    }
}
