///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 30/10/2019 14:21
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using Com.IsartDigital.Rush.Manager;
using UnityEngine;

namespace Com.IsartDigital.Rush
{
    public delegate void TargetEventHandler(); 
    public class Target : MonoBehaviour
    {
        public static event TargetEventHandler OnAllCubeOnTarget; 
        private RaycastHit hit;
        private uint cubeCounter = 0;
        [SerializeField] private uint winNumber = 5;
        public static List<Target> list = new List<Target>();

        public static void EmptyTarget() {
            Target lTarget; 
            for(int i = 0; i < list.Count; i++) {
                lTarget = list[i];
                lTarget.cubeCounter = 0; 
            }
        }

        public static void InitAll() {
            Target lTarget;
            for(int i = 0; i < list.Count; i++) {
                
                lTarget = list[i];
                lTarget.Init(); 
            }
        }
        private void Awake() {
            list.Add(this);
        }

        private void Init() {
            TimeManager.OnTick += TimeManager_OnTick;
        }

        private void TimeManager_OnTick() {
            CheckCollisionCube();
        }

        private void CheckCollisionCube() {
            if(Physics.Raycast(new Vector3(transform.position.x,transform.position.y -0.3f,transform.position.z), Vector3.up, out hit,15)) {
                cubeCounter++;
                hit.collider.gameObject.GetComponent<CubeMove>().Destroy();  
                if(cubeCounter == winNumber) OnAllCubeOnTarget?.Invoke();
            } 
        }

       
    }
}