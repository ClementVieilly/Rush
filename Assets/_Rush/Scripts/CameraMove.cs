///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 03/11/2019 13:16
///-----------------------------------------------------------------

using System;
using Com.IsartDigital.Rush.Manager;
using UnityEngine;

namespace Com.IsartDigital.Rush {
    public delegate void CameraMoveEventHandler(Vector3 eulerAngle); 
    public class CameraMove : MonoBehaviour
    {

        public static event CameraMoveEventHandler OnCameraMove; 
        [SerializeField, Range(0, 20)] private float radius;
        [SerializeField] private  GameManager gameManager; 
        private float horizontalAngle = 3;
        private float verticalAngle = 3;
        private Vector3 newDirection;
        private Transform cameraPivot;
        [SerializeField, Range(0.5f, 2f)] private float speed;

        private string vertical = "Vertical";
        private string horizontal = "Horizontal";

        private Action doAction; 

        private void Start() {
            radius = 14;
            speed = 2; 
            ControllerManager.OnMouseClick1Held += ControllerManager_OnMouseClick1Held; 
            ControllerManager.OnKeyDown += ControllerManager_OnKeyDown;
            SetModeVoid();
            cameraPivot = gameManager.levelList[0].transform; 
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
            doAction = doActionVoid;
            
        }

        private void doActionVoid() {
            
        }

        public void SetModeNormal() {
            doAction = doActionNormal;
        }

        

        private void OnDestroy() {
            ControllerManager.OnMouseClick1Held -= ControllerManager_OnMouseClick1Held;
            ControllerManager.OnKeyDown -= ControllerManager_OnKeyDown;
        }
    }
}