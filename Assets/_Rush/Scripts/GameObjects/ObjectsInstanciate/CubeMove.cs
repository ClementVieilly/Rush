///-----------------------------------------------------------------
/// Author : Clément VIEILLY feat Max
/// Date : 22/10/2019 10:35
///-----------------------------------------------------------------


using Com.IsartDigital.Assets._Rush.Scripts.GameObjects.ObjectsInstanciate;
using Com.IsartDigital.Rush.GameObjects.ObjectsOnLevelAtStart;
using Com.IsartDigital.Rush.Manager;
using Pixelplacement;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Rush.GameObjects.ObjectsInstanciate {
    public delegate void CubeMoveEventHandler(CubeMove send = null);
    public class CubeMove : ObjectsInstanciateScript
    {
        public static event CubeMoveEventHandler OnLoseContext;
        public static List<CubeMove> list = new List<CubeMove>(); 

        private float ratio = 0;
        private Vector3 fromPosition;
        private Vector3 toPosition;
        private Quaternion fromRotation;
        private Quaternion toRotation;
        private Vector3 movementDirection;
        private Quaternion movementRotation;

        private float rotationOffsetY = 0f;
        private float cubeSide = 1f;
        private float cubeFaceDiag = 0f;

        //state machine
        private Action DoAction;

        private float rayCastDistance = 0f;
        private float rayCastOffsetDistance = 0.4f;

        private Vector3 down;

        private RaycastHit hit;

       
        private uint stopCounter = 0; 
        //Tiles tag 
        private string groundTag = "Ground";
        private string arrowTag = "Arrow";
        private string conveyorTag = "Conveyor";
        private string turnstileTag = "Turnstile";
        private string teleportTag = "Teleport";
        private string stopTag = "Stop";
        private string wallTag = "Wall";
        private string cubeTag = "Cube";


        public uint alias;

        private void Start() {
            SetModeVoid(); 
        }
        [SerializeField] private AnimationCurve moveAnim; 
        public static void DestroyAll() {
            CubeMove lCube;
            for(int i = list.Count - 1; i >= 0; i--) {
                lCube = list[i];
                lCube.Destroy();
            }
        }
       override public void Init() {
            base.Init(); 
            list.Add(this);
            //transform.rotation = Quaternion.identity; 
            rayCastDistance = cubeSide / 2 + rayCastOffsetDistance;
            cubeFaceDiag = Mathf.Sqrt(2) * cubeSide;
            if(alias == 0) rotationOffsetY = 0.5f; 
            else  rotationOffsetY = cubeFaceDiag / 2 - cubeSide / 2;
            //tester ici 
            movementDirection = transform.forward;
            movementRotation = Quaternion.AngleAxis(90f, transform.right);
            
            toPosition = transform.position;
            toRotation = transform.rotation;
            SetModeVoid();
            TimeManager.EndTick += TimeManager_OnTick;
        }

        private void TimeManager_OnTick() {
            //CheckForwardCollision();
            CheckTilesCollision();
            //Regarder si y'a mieux a faire
        }

        private bool CheckForwardCollision() {
            
            if(Physics.Raycast(transform.position, movementDirection, out hit, rayCastDistance)) {
                // if(hit.collider.CompareTag(cubeTag)) CheckTilesCollision();
                if(hit.collider.CompareTag(wallTag)) {
                    SetDirectionTo(Vector3.Cross(Vector3.up, movementDirection));
                    return true;
                }

            }

             return false;

        }

        private void CheckTilesCollision() {
           
            if(stopCounter == 2) {
                stopCounter = 0;
                if(!CheckForwardCollision()) SetModeMove();

                return;
            }
            
            else {
                SetModeVoid();
            }
           /*if(wallCounter == 1) {
                wallCounter = 0;
                SetModeMove();
                return;
            }
            
            else {
                SetModeVoid();
            }*/
            down = Vector3.down;
            if(Physics.Raycast(transform.position, down, out hit, rayCastDistance)) {
               
                GameObject hitObject = hit.collider.gameObject;
                if(hit.collider.CompareTag(groundTag)) {
                    
                   if(!CheckForwardCollision())SetModeMove();
                 
                    return; 
                }

                if(hit.collider.CompareTag(arrowTag)) {
                    SetDirectionTo(hitObject.transform.forward);
                    SetModeMove();
                    return; 
                }

                if(hit.collider.CompareTag(conveyorTag)) {
                    SetModeConveyor(hitObject.transform.forward);

                }

                if(hit.collider.CompareTag(turnstileTag)) {
                    hit.collider.gameObject.GetComponent<Turnstile>().checkSense();
                    SetDirectionTo(Vector3.Cross(Vector3.up, movementDirection) * hit.collider.gameObject.GetComponent<Turnstile>().changeSense); 
                    SetModeMove(); 
                }

                if(hit.collider.CompareTag(teleportTag)) {
                    toPosition = hit.collider.gameObject.GetComponent<Teleport>().pair.transform.position + new Vector3(0, cubeSide / 2, 0);
                    stopCounter++; 
                    SetModeVoid(); 
                    
                    
                }
                //Il y'a mieux à faire 
                if(hit.collider.CompareTag(stopTag)) {
                    SetModeVoid();
                    stopCounter++; 
                }
            }
            else {
                SetModeFall();

            }

        }



        private void Update() {
            ratio = TimeManager.ratio;
            DoAction();
        }

        public void SetModeVoid() {
            DoAction = DoActionVoid;

        }

        private void DoActionVoid() {

        }
        

        protected void SetModeStop() {
             
        }
        private void SetDirectionTo(Vector3 vector) {
            movementDirection = vector;
            movementRotation = Quaternion.AngleAxis(90f, Vector3.Cross(Vector3.up, movementDirection));
        }
        private void InitRotation() {
            fromPosition = toPosition;
            fromRotation = toRotation;
            toPosition = fromPosition + movementDirection ;
            toRotation = movementRotation * fromRotation;
        }
        private void SetModeMove() {
           
            InitRotation();
            if(CheckForwardCollision()) {
               
                return;
            }
            DoAction = DoActionMove;
        }

        private void SetModeConveyor(Vector3 forward) {
            InitNextSlideMovement(forward);
            DoAction = DoActionConveyor;
        }

        private void DoActionConveyor() {
            transform.position = Vector3.Lerp(fromPosition, toPosition, ratio);
            
        }

        private void DoActionMove() {
            transform.position = Vector3.Lerp(fromPosition, toPosition, moveAnim.Evaluate(ratio)) + (Vector3.up * (rotationOffsetY * Mathf.Sin(Mathf.PI * moveAnim.Evaluate(ratio))));
            transform.rotation = Quaternion.Lerp(fromRotation, toRotation, moveAnim.Evaluate(ratio));
            
           if(ratio >= 0.9f) {
               transform.rotation = Quaternion.identity;
            }
           
        }

        private void SetModeFall() {
            InitNextFallingMovement();
            CheckVoidCollision();
            DoAction = DoActionFall;
        }

        private void DoActionFall() {
            transform.position = Vector3.Lerp(fromPosition, toPosition, ratio) + (Vector3.up * (rotationOffsetY * Mathf.Sin(Mathf.PI * ratio)));
        }

        private void InitNextFallingMovement() {
            fromPosition = transform.position;
            toPosition = fromPosition + Vector3.down;
        }

        private void InitNextSlideMovement(Vector3 forward) {
            fromPosition = transform.position;
            toPosition = fromPosition + forward;
        }

        private void CheckVoidCollision() {
            if(!Physics.Raycast(transform.position, Vector3.down, 100)) {
                OnLoseContext?.Invoke();
                
            }
        }

        private void OnTriggerEnter(Collider other) {
            if(other.CompareTag(cubeTag)) {
                OnLoseContext?.Invoke();

            }
        }
       
        public void StopTick() {
            TimeManager.EndTick -= TimeManager_OnTick;
            SetModeVoid(); 
        }

        public override void Destroy() {
            base.Destroy();
            //TimeManager.EndTick -= TimeManager_OnTick;
            list.RemoveAt(list.IndexOf(this));
           
        }
    }
}