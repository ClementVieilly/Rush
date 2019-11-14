///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 30/10/2019 12:52
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using Com.IsartDigital.Assets._Rush.Scripts.GameObjects.ObjectsInstanciate;
using Com.IsartDigital.Assets._Rush.Scripts.GameObjects.ObjectsOnLevelAtStart;
using Com.IsartDigital.Rush.GameObjects.ObjectsInstanciate;
using Com.IsartDigital.Rush.Manager;
using UnityEngine;

namespace Com.IsartDigital.Rush.GameObjects.ObjectsOnLevelAtStart {
    public class Spawner : ObjectsOnLevelAtStartScript
    {
        [SerializeField] private GameObject cubePrefab;
        [SerializeField] private uint spawnFrequence = 4;
        [SerializeField] private uint spawnNumber; 
        private int frequencyCounter = 1;
        private int spawnCounter = 0;
        private static List<Spawner> list = new List<Spawner>(); 
        
        public static void EmptySpawner() {
            Spawner lSpawn; 
            for(int i = list.Count - 1; i >= 0; i--) {
                lSpawn = list[i];
                lSpawn.spawnCounter = 0;
                lSpawn.frequencyCounter = 0; 
            }

            
        }

        public static void InitAll() {
            Spawner lSpawn;
          
            for(int i = list.Count - 1; i >= 0; i--) {
                lSpawn = list[i];
                lSpawn.Init(); 
            }
        }
        public override void Init() {
            base.Init();
            list.Add(this);
            TimeManager.OnTick += TimeManager_OnTick;
        }

        private void TimeManager_OnTick() {
           
            frequencyCounter++;
             
            if(frequencyCounter > spawnFrequence && spawnCounter < spawnNumber) {
                frequencyCounter = 0; 
                GameObject go = Instantiate(cubePrefab, transform.position + new Vector3(0, 1f / 2, 0), transform.rotation);
                go.GetComponent<CubeMove>().Init();
                spawnCounter++;
            }

        }
        public override void Destroy() {
            base.Destroy();
            TimeManager.OnTick -= TimeManager_OnTick;
            list.RemoveAt(list.IndexOf(this)); 
        }
       

       
    }
}