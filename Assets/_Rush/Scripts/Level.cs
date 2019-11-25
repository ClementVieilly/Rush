///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 06/11/2019 10:59
///-----------------------------------------------------------------

using Com.IsartDigital.Assets._Rush.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Rush
{
    public class Level : MonoBehaviour {
        public List<Inventory> inventoryLevel = new List<Inventory>();
        public float radius;

        public List<Transform> conffetisPos = new List<Transform>();
        [SerializeField] private ParticleSystem confettiBurst; 
        public void Init() {

            Inventory lInventory;

            // for(int i = confettiBurst.Count - 1; i >= 0; i--) confettiBurst[i].GetComponent<ParticleSystem>().Stop();

            for(int i = inventoryLevel.Count - 1; i >= 0; i--) {
                lInventory = inventoryLevel[i];
                lInventory.TilesList.Clear();
                for(int j = lInventory.getCount - 1; j >= 0; j--) {
                    lInventory.TilesList.Add(lInventory.Tile);

                }
            }


        }

        public void PlayConfetits() {
            for(int i = conffetisPos.Count - 1; i >= 0; i--) {
                 Instantiate(confettiBurst, conffetisPos[i].position - Vector3.up * 10,confettiBurst.transform.rotation );

            }
        }
    }
}