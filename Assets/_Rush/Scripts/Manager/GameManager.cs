///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 22/10/2019 09:55
///-----------------------------------------------------------------

using System;
using UnityEngine;

namespace Com.IsartDigital.Rush.Manager
{
    public class GameManager : MonoBehaviour
    {

        private uint targetCounter = 0;
        public static bool stopTest = false;
        protected TimeManager timeManager;
        [SerializeField] private GameObject level;
        private Level levelScript;
        [SerializeField] private Player player;
        private bool actionPhase;

        private void Start() {
            levelScript = level.GetComponent<Level>();
            CubeMove.OnLoseContext += CubeMove_OnLoseContext;
            Target.OnAllCubeOnTarget += Target_OnAllCubeOnTarget;
            ControllerManager.OnMouse0Down += ControllerManager_OnMouseDown0;
            timeManager = FindObjectOfType<TimeManager>();
            levelScript.Init();
            CreateLevel();

            player.Init();
        }

        private void ControllerManager_OnMouseDown0(float axeX, float axeY) {
            if(actionPhase) {
                timeManager.SetModeVoid();
                ReorganiseLevel();
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
            for(int i = 0; i < levelScript.inventoryLevel.Count; i++) {
                Player.inventory.Add(levelScript.inventoryLevel[i]);
            }
            InitAllGameObject();

        }
    }
}