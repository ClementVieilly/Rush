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
        public static bool testPhase;

        private Action doAction; 

        //faire un getter pour le ratio
        public static event TimeManagerEventHandler OnTick;

        private void Start() {
            SetModeVoid(); 
        }

        public void SetModeVoid() {
            doAction = doActionVoid; 
        }

        private void doActionVoid() {
            
        }

        public void SetModeNormal() {
            doAction = doActionNormal;
        }

        private void doActionNormal() {
            Tick(); 
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