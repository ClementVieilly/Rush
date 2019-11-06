///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 22/10/2019 09:55
///-----------------------------------------------------------------

using System;
using UnityEngine;

namespace Com.IsartDigital.Rush.Manager {
	public class GameManager : MonoBehaviour {

        private uint targetCounter = 0;
        public static bool stopTest = false;
        protected TimeManager timeManager;
        [SerializeField] private GameObject level;

        private void Start() {
            
            CubeMove.OnLoseContext += CubeMove_OnLoseContext;
            Target.OnAllCubeOnTarget += Target_OnAllCubeOnTarget;
            timeManager = FindObjectOfType<TimeManager>();
            CreateLevel(); 

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
            
        }

        public void OnGo() {
            timeManager.SetModeNormal();
            InitAllGameObject(); 
        }

        private void InitAllGameObject() {
            Target.InitAll();
            Spawner.InitAll();
            Teleport.InitAll();
            //Mettre tout les init ici 
        }

        protected void CreateLevel() {
            level = Instantiate(level, Vector3.zero, Quaternion.identity);
            for(int i = 0; i < level.GetComponent<Level>().inventoryLevel.Count; i++) {
                Player.inventory.Add(level.GetComponent<Level>().inventoryLevel[i]); 
            }
           
        }
    }
}