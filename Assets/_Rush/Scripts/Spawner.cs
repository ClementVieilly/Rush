///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 30/10/2019 12:52
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using Com.IsartDigital.Rush.Manager;
using UnityEngine;

namespace Com.IsartDigital.Rush {
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject cubePrefab;
        [SerializeField] private uint spawnFrequence = 4;
        [SerializeField] private uint spawnNumber; 
        private int frequencyCounter = 1;
        private int spawnCounter = 0;
        private static List<Spawner> list = new List<Spawner>(); 
        
        public static void EmptySpawner() {
            Spawner lSpawn; 
            for(int i = 0; i < list.Count; i++) {
                lSpawn = list[i];
                lSpawn.spawnCounter = 0; 
            }
        }

        public static void InitAll() {
            Spawner lSpawn;
          
            for(int i = 0; i < list.Count; i++) {
                lSpawn = list[i];
                lSpawn.Init(); 
            }
        }
        private void Init() {
           
            TimeManager.OnTick += TimeManager_OnTick;
            
        }

        private void Awake() {
            list.Add(this);
        }

        private void TimeManager_OnTick() {
           
            frequencyCounter++;
            if(frequencyCounter % spawnFrequence == 0 && spawnCounter < spawnNumber) {
                GameObject go = Instantiate(cubePrefab, transform.position + new Vector3(0, 1f / 2, 0), transform.rotation);
                go.GetComponent<CubeMove>().Init();
                spawnCounter++;
            }

        }

        private void OnDestroy() {
            TimeManager.OnTick -= TimeManager_OnTick;

        }

       
    }
}