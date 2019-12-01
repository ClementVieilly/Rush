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
        public GameObject level;
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
        }

        public void SwitchPhase() {
           if(loseScreen.activeSelf) loseScreen.SetActive(false);
            hudAction.gameObject.GetComponent<Animator>().SetTrigger("Disappear");
            //callBack du bouton Reset dans hudAction
            timeManager.SetModeVoid();
            ReorganiseLevel();
            hudReflexion.gameObject.GetComponent<Animator>().SetTrigger("Appear");
            levelScript.StopTribunesBlueAnim(); 
            Spawner.PlaySpawnParticles();
            TimeManager.Speed = 1.2f; 
        }
        private void CubeMove_OnLoseContext(CubeMove send = null) {
            timeManager.SetModeVoid();
            loseScreen.SetActive(true);
            levelScript.PlayTribunesBlueAnim(); 
        }

        private void Target_OnAllCubeOnTarget() {
            targetCounter++;
            if(targetCounter == Target.list.Count) Win();
        }

        private void Win() {
            levelScript.PlayConfetits();
            levelScript.PlayTribunesRedAnim(); 
            targetCounter = 0; 
            UIManager.DisplayWin();
            hudAction.GetComponent<Animator>().SetTrigger("Disappear"); 
        }
        public void ConffettisTestPos() {
            levelScript.PlayConfetits();
        }
        private void ReorganiseLevel() {
            Spawner.EmptySpawner();
            CubeMove.DestroyAll();
            Target.EmptyTarget();
            Turnstile.ResetSense(); 
            player.SetModeNormal();
            targetCounter = 0; 
        }

        public void OnGo() {
            hudReflexion.GetComponent<Animator>().SetTrigger("Disappear"); 
            hudAction.GetComponent<Animator>().SetTrigger("Appear"); 
            timeManager.SetModeNormal();
            actionPhase = true;
            player.SetModeVoid();
           
        }

        private void InitAllGameObjectsOnLevelAtStart() {
            ObjectsOnLevelAtStartScript.InitAllGameObjectAtLevelAtStart(); 
        }

        protected void CreateLevel() {
            actionPhase = false;
            level = Instantiate(level, Vector3.zero, Quaternion.identity);
            FillPlayerTab(); 
            InitAllGameObjectsOnLevelAtStart();
            cameraMove.SetModeZoom();
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
            for(int i = levelScript.tribunesRedList.Count - 1; i >= 0; i--) {
                Destroy(levelScript.tribunesRedList[i]); 
            }
            for(int i = levelScript.tribunesBlueList.Count - 1; i >= 0; i--) {
                Destroy(levelScript.tribunesBlueList[i]); 
            }
            Destroy(level.gameObject);
            Player.inventory.Clear();
            ObjectsInstanciateScript.RemoveAll();
            player.SetModeVoid();
            timeManager.SetModeVoid();
            cameraMove.SetModeVoid(); 

        }

        private void OnDestroy() {
            CubeMove.OnLoseContext -= CubeMove_OnLoseContext;
            Target.OnAllCubeOnTarget -= Target_OnAllCubeOnTarget;
           // ControllerManager.OnMouse0Down -= ControllerManager_OnMouseDown0;
        }
    }
}