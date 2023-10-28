
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class CameraFollow : MonoBehaviour
{
    [Header("CameraMove")]
    public Transform theTarget;
    //public static CameraFollow Instance;
    //public Transform targetPlr, targetXr, targetXl, targetYr, targetYl, targetZr, targetZl, targetWr, targetWl;
    //public string cameraTarget;

    public float smoothSpeed = 0.125f; public Vector3 zoomOffset;

    void FixedUpdate() { transform.position = Vector3.Lerp(transform.position, theTarget.position + zoomOffset, smoothSpeed); }

    /*    private void Start()
        {
            theTarget = targetPlr;
        }
        void FixedUpdate()
        {   //Camera Follow
            MoveCamera();
        }

        public void MoveCamera() {

            switch (cameraTarget)
            {
                default: break;
                case "targetXr": transform.position = Vector3.Lerp(transform.position, targetXr.position + zoomOffset, smoothSpeed); break;
                case "targetXl": transform.position = Vector3.Lerp(transform.position, targetXl.position + zoomOffset, smoothSpeed); break;

            }

        }

        public void LookFarXr() { cameraTarget = "targetXr"; }
        public void LookFarXl() { cameraTarget = "targetXl"; }
        public void LookFarYr() { theTarget = targetYr; }
        public void LookFarYl() { theTarget = targetYl; }
        public void LookFarZr() { theTarget = targetXr; }
        public void LookFarZl() { theTarget = targetXl; }
        public void LookFarWr() { theTarget = targetYr; }
        public void LookFarWl() { theTarget = targetYl; }

        //public void LookFar(Vector3 newFocus) { theTarget = newFocus; }
        public void LookBack() { theTarget = targetPlr; }*/
}