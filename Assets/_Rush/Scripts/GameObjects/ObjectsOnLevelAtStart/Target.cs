///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 30/10/2019 14:21
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using Com.IsartDigital.Assets._Rush.Scripts.GameObjects.ObjectsOnLevelAtStart;
using Com.IsartDigital.Rush.GameObjects.ObjectsInstanciate;
using Com.IsartDigital.Rush.Manager;
using UnityEngine;

namespace Com.IsartDigital.Rush.GameObjects.ObjectsOnLevelAtStart {
    public delegate void TargetEventHandler(); 
    public class Target : ObjectsOnLevelAtStartScript
    {
        public static event TargetEventHandler OnAllCubeOnTarget; 
        private RaycastHit hit;
        private uint cubeCounter = 0;
        [SerializeField] private uint winNumber; 
        [SerializeField] private uint alias;
        public static List<Target> list = new List<Target>();

        public static void EmptyTarget() {
            Target lTarget; 
            for(int i = list.Count - 1; i >= 0; i--) {
                lTarget = list[i];
                lTarget.cubeCounter = 0; 
            }
        }

        public static void InitAll() {
            Target lTarget;
            for(int i = list.Count - 1; i >= 0; i--) {
                
                lTarget = list[i];
                lTarget.Init(); 
            }
        }
        private void Awake() {
            list.Add(this);
        }

        public override void Init() {
          
            TimeManager.OnTick += TimeManager_OnTick;
        }
        

        private void TimeManager_OnTick() {
            CheckCollisionCube();
        }

        private void CheckCollisionCube() {
            if(Physics.Raycast(new Vector3(transform.position.x,transform.position.y -0.3f,transform.position.z), Vector3.up, out hit,15)) {
              
                if(hit.collider.gameObject.GetComponent<CubeMove>().alias != alias) return; 
                cubeCounter++;
                hit.collider.gameObject.GetComponent<CubeMove>().Destroy();
                if(cubeCounter == winNumber) {
                    OnAllCubeOnTarget?.Invoke();
                    
                }
            } 
        }

        private void OnDestroy() {

            base.Destroy();
            TimeManager.OnTick -= TimeManager_OnTick;

            list.RemoveAt(list.IndexOf(this));
        }

    }
}