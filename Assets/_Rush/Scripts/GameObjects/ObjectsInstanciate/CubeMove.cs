///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 22/10/2019 10:35
///-----------------------------------------------------------------


using Com.IsartDigital.Assets._Rush.Scripts.GameObjects.ObjectsInstanciate;
using Com.IsartDigital.Rush.GameObjects.ObjectsOnLevelAtStart;
using Com.IsartDigital.Rush.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Rush.GameObjects.ObjectsInstanciate {
    public delegate void CubeMoveEventHandler();
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

        private bool testStop = false;
        //Tiles tag 
        private string groundTag = "Ground";
        private string arrowTag = "Arrow";
        private string conveyorTag = "Conveyor";
        private string turnstileTag = "Turnstile";
        private string teleportTag = "Teleport";
        private string stopTag = "Stop";
        private string wallTag = "Wall";
        private string cubeTag = "Cube";


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

            rayCastDistance = cubeSide / 2 + rayCastOffsetDistance;
            cubeFaceDiag = Mathf.Sqrt(2) * cubeSide;
            rotationOffsetY = cubeFaceDiag / 2 - cubeSide / 2;
            //tester ici 
            movementDirection = transform.forward;
            movementRotation = Quaternion.AngleAxis(90f, transform.right);
            toPosition = transform.position;
            toRotation = transform.rotation;
            SetModeVoid();
            TimeManager.OnTick += TimeManager_OnTick;

        }

        private void TimeManager_OnTick() {
            CheckForwardCollision();
            //Regarder si y'a mieux a faire
            //CheckTilesCollision();
        }

        private void CheckForwardCollision() {

            if(Physics.Raycast(transform.position, movementDirection, out hit, rayCastDistance)) {
                
                if(hit.collider.CompareTag(wallTag)) {
                    SetDirectionTo(Vector3.Cross(Vector3.up, movementDirection));
                    SetModeVoid();

                    return;
                }

            }

            else CheckTilesCollision();

        }

        private void CheckTilesCollision() {

            if(testStop) {
                testStop = false;
                SetModeMove();
                return;
            }
            down = Vector3.down;
            if(Physics.Raycast(transform.position, down, out hit, rayCastDistance)) {

                GameObject hitObject = hit.collider.gameObject;
                if(hit.collider.CompareTag(groundTag)) {
                    SetModeMove();
                }

                if(hit.collider.CompareTag(arrowTag)) {
                    SetDirectionTo(hitObject.transform.forward);
                    SetModeMove();
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
                    testStop = true; 
                    SetModeVoid(); 
                    
                    
                }
                //Il y'a mieux à faire 
                if(hit.collider.CompareTag(stopTag)) {
                    testStop = true;
                    SetModeVoid();

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

        private void SetModeVoid() {
            DoAction = DoActionVoid;

        }

        private void DoActionVoid() {

        }

        private void SetDirectionTo(Vector3 vector) {
            movementDirection = vector;
            movementRotation = Quaternion.AngleAxis(90f, Vector3.Cross(Vector3.up, movementDirection));
        }
        private void InitRotation() {
            fromPosition = toPosition;
            fromRotation = toRotation;
            toPosition = fromPosition + movementDirection;
            toRotation = movementRotation * fromRotation;
        }
        private void SetModeMove() {
            InitRotation();
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
            transform.position = Vector3.Lerp(fromPosition, toPosition, ratio) + (Vector3.up * (rotationOffsetY * Mathf.Sin(Mathf.PI * ratio)));
            transform.rotation = Quaternion.Lerp(fromRotation, toRotation, ratio);
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
                Destroy(); 
                OnLoseContext?.Invoke();
            }
        }

        private void OnTriggerEnter(Collider other) {
            if(other.CompareTag(cubeTag)) {
                OnLoseContext?.Invoke();

            }
        }
       


        public override void Destroy() {
            base.Destroy();
            TimeManager.OnTick -= TimeManager_OnTick;
            list.RemoveAt(list.IndexOf(this));
            Destroy(gameObject);
        }
    }
}