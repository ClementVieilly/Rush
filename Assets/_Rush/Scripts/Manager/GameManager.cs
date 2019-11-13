///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 22/10/2019 09:55
///-----------------------------------------------------------------

using Com.IsartDigital.Assets._Rush.Scripts.GameObjects.ObjectsInstanciate;
using Com.IsartDigital.Rush.GameObjects;
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
        protected TimeManager timeManager;
        protected UIManager UIManager;
        [SerializeField] private GameObject level;
        [SerializeField] private Hud hud;
        [SerializeField] private List<Level> levelList = new List<Level>();
        [SerializeField] private CameraMove cameraMove; 

        private Level levelScript;
        [SerializeField] private Player player;
        private bool actionPhase;
        public bool onPause;

        private void Start() {   
            levelScript = levelList[0]; 
            CubeMove.OnLoseContext += CubeMove_OnLoseContext;
            Target.OnAllCubeOnTarget += Target_OnAllCubeOnTarget;
            ControllerManager.OnMouse0Down += ControllerManager_OnMouseDown0;
            timeManager = FindObjectOfType<TimeManager>();
            levelScript.Init();
            CreateLevel();
            player.Init();
            hud.Init();
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

        public void Init(int Level = 0) {
            levelScript.Init();
            CreateLevel();
            //CreateLevel(Level); 
            player.Init();
        }

        private void ControllerManager_OnMouseDown0(float axeX, float axeY) {
            if(actionPhase && !onPause) {
                timeManager.SetModeVoid();
                ReorganiseLevel();
                hud.gameObject.SetActive(true);

            }
        }

        private void CubeMove_OnLoseContext() {
            timeManager.SetModeVoid();
            Debug.Log("défaite");
            ReorganiseLevel();
            //Défaite 
            //Refaire tout le niveau
        }

        private void Target_OnAllCubeOnTarget() {
            // a revoir 
            targetCounter++;
            if(targetCounter == Target.list.Count) Win();
        }

        private void Win() {
            Debug.Log("Victoire");
        }

        private void ReorganiseLevel() {
            CubeMove.DestroyAll();
            Spawner.EmptySpawner();
            Target.EmptyTarget();
            player.SetModeNormal(); 
        }

        public void OnGo() {
            timeManager.SetModeNormal();
            actionPhase = true;
            player.SetModeVoid();
            hud.gameObject.SetActive(false); 
        }

        private void InitAllGameObject() {
            Target.InitAll();
            Spawner.InitAll();
            Teleport.InitAll();
            //Mettre tout les init ici 
        }

        protected void CreateLevel() {
            actionPhase = false;
            level = Instantiate(level, Vector3.zero, Quaternion.identity);
            FillPlayerTab(); 
            InitAllGameObject();

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
        }
    }
}