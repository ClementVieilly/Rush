///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 22/10/2019 09:55
///-----------------------------------------------------------------

using Com.IsartDigital.Assets._Rush.Scripts.GameObjects;
using Com.IsartDigital.Assets._Rush.Scripts.GameObjects.ObjectsInstanciate;
using Com.IsartDigital.Assets._Rush.Scripts.GameObjects.ObjectsOnLevelAtStart;
using Com.IsartDigital.Rush.GameObjects.ObjectsInstanciate;
using Com.IsartDigital.Rush.GameObjects.ObjectsOnLevelAtStart;
using Com.IsartDigital.Rush.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Rush.Manager
{
    public class GameManager : MonoBehaviour
    {

        private uint targetCounter = 0;
        public static bool stopTest = false;
        [SerializeField] protected TimeManager timeManager;
        [SerializeField] private UIManager UIManager;
        [SerializeField] private GameObject level;
        [SerializeField] private Hud hudReflexion;
        [SerializeField] private GameObject hudAction;
        [SerializeField] private GameObject loseScreen; 
        public List<GameObject> levelList = new List<GameObject>();
        [SerializeField] private CameraMove cameraMove; 

        private Level levelScript;
        [SerializeField] private Player player;
        private bool actionPhase;
        public bool onPause;

        private void Start() {   
            
            CubeMove.OnLoseContext += CubeMove_OnLoseContext;
            Target.OnAllCubeOnTarget += Target_OnAllCubeOnTarget;
            
        }

        public void SetPlay() {
            timeManager.SetModeReplay();
            player.SetModeNormal();
            cameraMove.SetModeNormal();
            onPause = false;
        }

        public void SetPause() {
            
            timeManager.SetModePause();
            player.SetModeVoid();
            cameraMove.SetModeVoid(); 
            onPause = true; 
        }

        public void Init(int levelIndex = 0) {
            level = levelList[levelIndex]; 
            levelScript = level.GetComponent<Level>();
            levelScript.Init();
            CreateLevel();
            player.Init();
           
            cameraMove.SetModeNormal(); 
        }

       private void ControllerManager_OnMouseDown0(float axeX, float axeY) {
            loseScreen.SetActive(false); 
            hudReflexion.gameObject.SetActive(true);
            hudAction.SetActive(false);
            ControllerManager.OnMouse0Down -= ControllerManager_OnMouseDown0;
            
            ReorganiseLevel();
        }
        public void SwitchPhase() {
            timeManager.SetModeVoid();
            ReorganiseLevel();
            hudReflexion.gameObject.SetActive(true);
            hudAction.SetActive(false); 
        }
        private void CubeMove_OnLoseContext() {
            timeManager.SetModeVoid();
            loseScreen.SetActive(true); 
            ControllerManager.OnMouse0Down += ControllerManager_OnMouseDown0;
        }

        private void Target_OnAllCubeOnTarget() {
            targetCounter++;
            if(targetCounter == Target.list.Count) Win();
        }

        private void Win() {
            targetCounter = 0; 
            UIManager.DisplayWin(); 
        }

        private void ReorganiseLevel() {
            CubeMove.DestroyAll();
            Spawner.EmptySpawner();
            Target.EmptyTarget();
            Turnstile.ResetSense(); 
            player.SetModeNormal();
            targetCounter = 0; 
        }

        public void OnGo() {
            timeManager.SetModeNormal();
            actionPhase = true;
            player.SetModeVoid();
            hudReflexion.gameObject.SetActive(false);
            hudAction.gameObject.SetActive(true); 
        }

        private void InitAllGameObjectsOnLevelAtStart() {
            ObjectsOnLevelAtStartScript.InitAllGameObjectAtLevelAtStart(); 
        }

        protected void CreateLevel() {
            actionPhase = false;
            level = Instantiate(level, Vector3.zero, Quaternion.identity);
            FillPlayerTab(); 
            InitAllGameObjectsOnLevelAtStart();
        }

        private void FillPlayerTab() {
            for(int i = levelScript.inventoryLevel.Count - 1; i >= 0; i--) {
                Player.inventory.Add(levelScript.inventoryLevel[i]);
            }
        }

        public void ResetLevel() {
            onPause = false;
            cameraMove.SetModeNormal();
            ObjectsInstanciateScript.RemoveAll(); 
            ReorganiseLevel();
            Player.inventory.Clear(); 
            levelScript.Init();
            FillPlayerTab(); 
            player.Init();   
        }
        public void DestroyLevel() {
            Destroy(level.gameObject);
            Player.inventory.Clear();
            ObjectsInstanciateScript.RemoveAll();
            player.SetModeVoid();
            timeManager.SetModeVoid(); 

        }

        private void OnDestroy() {
            CubeMove.OnLoseContext -= CubeMove_OnLoseContext;
            Target.OnAllCubeOnTarget -= Target_OnAllCubeOnTarget;
           // ControllerManager.OnMouse0Down -= ControllerManager_OnMouseDown0;
        }
    }
}