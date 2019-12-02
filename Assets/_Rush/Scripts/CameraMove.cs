///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 03/11/2019 13:16
///-----------------------------------------------------------------

using System;
using Com.IsartDigital.Rush.Manager;
using Pixelplacement;
using UnityEngine;

namespace Com.IsartDigital.Rush {
    public delegate void CameraMoveEventHandler(Vector3 eulerAngle); 
    public delegate void CameraEndAnimEvent(); 
    public class CameraMove : MonoBehaviour
    {

        public static event CameraMoveEventHandler OnCameraMove;
        public event CameraEndAnimEvent OnZoomFinish; 
        [SerializeField, Range(0, 20)] private float radius;
        [SerializeField] private  GameManager gameManager;
        [SerializeField] private  AnimationCurve anim;
        private float horizontalAngle = 1;
        private float verticalAngle = 1;
        private Vector3 newDirection;
        private Transform cameraPivot;
        [SerializeField, Range(0.5f, 2f)] private float speed;

    
        private Action doAction;
        private float elapseTime = 0;
        private Vector3 cameraBasePos = new Vector3(-1, 45, 65); 
        
        private float ratio;
        public bool test = false; 
        private void Start() {
            //radius = 12;
           
            SetModeVoid();
            test = true; 

#if UNITY_ANDROID || UNITY_IOS
            speed = 0.10f; 
#else
           speed = 2;

#endif
           
            
        }

        private void ControllerManager_OnKeyDown(float axeX,float axeY) {
            horizontalAngle += axeX * Time.deltaTime * speed;
            verticalAngle = Mathf.Clamp(verticalAngle + axeY * Time.deltaTime * speed, -89.9f * Mathf.Deg2Rad, 89.9f * Mathf.Deg2Rad);
            
        }

        private void ControllerManager_OnMouseClick1Held(float axeX, float axeY) {
            horizontalAngle += -axeX * Time.deltaTime * speed;
            verticalAngle = Mathf.Clamp(verticalAngle - axeY * Time.deltaTime * speed, -89.9f * Mathf.Deg2Rad, 89.9f * Mathf.Deg2Rad);

        }

        private void Update() {
            doAction(); 
        }
        private void doActionNormal() {

            newDirection.x = radius * Mathf.Cos(verticalAngle) * Mathf.Cos(horizontalAngle);
            newDirection.y = radius * Mathf.Sin(verticalAngle);
            newDirection.z = radius * Mathf.Cos(verticalAngle) * Mathf.Sin(horizontalAngle);

            transform.position = newDirection + cameraPivot.position;
            transform.LookAt(cameraPivot);
            OnCameraMove?.Invoke(transform.eulerAngles); 
        }

        public void SetModeVoid() {
            ControllerManager.OnMouseClick1Held -= ControllerManager_OnMouseClick1Held;
            ControllerManager.OnKeyDown -= ControllerManager_OnKeyDown;
            doAction = doActionVoid;
            
        }

        private void doActionVoid() {
            
        }

        public void SetModeNormal() {
            if(test) {
                ControllerManager.OnMouseClick1Held += ControllerManager_OnMouseClick1Held;
                ControllerManager.OnKeyDown += ControllerManager_OnKeyDown;
                test = false; 
            }
           
            doAction = doActionNormal;
            elapseTime = 0;
            

        }

        public void SetModeZoom() {
            horizontalAngle = 1;
            verticalAngle = 1;
            transform.position = cameraBasePos;
            cameraPivot = gameManager.level.transform;

            doAction = DoActionZoom; 
        }

        private void DoActionZoom() {
           
            elapseTime +=  Time.deltaTime;
            ratio = anim.Evaluate(elapseTime);
            radius = gameManager.level.GetComponent<Level>().radius; 
            newDirection.x = radius * Mathf.Cos(verticalAngle) * Mathf.Cos(horizontalAngle);
            newDirection.y = radius * Mathf.Sin(verticalAngle);
            newDirection.z = radius * Mathf.Cos(verticalAngle) * Mathf.Sin(horizontalAngle);

            transform.position =Vector3.Lerp(transform.position,newDirection + cameraPivot.position,ratio);
            transform.LookAt(cameraPivot);
            if(ratio == 1) {
                SetModeNormal();
                OnZoomFinish?.Invoke(); 
            }
        }
        public void SetModeLoose(Transform target) {
            horizontalAngle = 1;
            verticalAngle = 1;
           
            cameraPivot = target;

            doAction = DoActionLoose;
        }

        private void DoActionLoose() {

            elapseTime += Time.deltaTime;
            ratio = elapseTime /7f;
            radius = gameManager.level.GetComponent<Level>().radius;
            newDirection.x = 10 * Mathf.Cos(verticalAngle) * Mathf.Cos(horizontalAngle);
            newDirection.y = 10 * Mathf.Sin(verticalAngle);
            newDirection.z = 10 * Mathf.Cos(verticalAngle) * Mathf.Sin(horizontalAngle);

            transform.position = Vector3.Lerp(transform.position, newDirection + cameraPivot.position, ratio);
            transform.LookAt(cameraPivot);
           
        }
        private void OnDestroy() {
            
            ControllerManager.OnMouseClick1Held -= ControllerManager_OnMouseClick1Held;
            ControllerManager.OnKeyDown -= ControllerManager_OnKeyDown;
        }
    }
}