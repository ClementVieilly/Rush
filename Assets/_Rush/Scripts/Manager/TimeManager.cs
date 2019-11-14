///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 22/10/2019 09:56
///-----------------------------------------------------------------

using System;
using UnityEngine;

namespace Com.IsartDigital.Rush.Manager {
    public delegate void TimeManagerEventHandler (); 
	public class TimeManager : MonoBehaviour {
        [SerializeField, Range(0.1f, 5f)] private float speed = 0.7f;
        private float elapsedTime = 0f;
        private float durationBetweenTicks = 1f;
        public static  float ratio = 0;
        private  bool isOnTick;

        private Action doAction; 

        //faire un getter pour le ratio
        public static event TimeManagerEventHandler OnTick;

        private void Start() {
            SetModeVoid(); 
        }

        public void SetModeVoid() {
            doAction = doActionVoid;
            isOnTick = false; 
        }

        private void doActionVoid() {
            
        }

        public void SetModeNormal() {
            doAction = doActionNormal;
            isOnTick = true; 
        }

        private void doActionNormal() {
            Tick(); 
        }

        public void SetModeReplay() {
            if(isOnTick) doAction = doActionNormal;
            else doAction = doActionVoid; 
        }
       
        public void SetModePause() {
            doAction = doActionVoid; 
        }

        private void Tick() {
            if(elapsedTime > durationBetweenTicks) {
                elapsedTime = 0f;
                OnTick?.Invoke();
               
            }
            elapsedTime += Time.deltaTime * speed;
            ratio = Mathf.Clamp01(elapsedTime / durationBetweenTicks);
        }

        private void Update() {
            doAction(); 
        }
    }
}