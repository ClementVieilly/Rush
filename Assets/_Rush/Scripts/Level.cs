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
        [SerializeField]  private  List<Transform> tribunesPos = new List<Transform>();
        [SerializeField] private ParticleSystem confettiBurst; 
        [SerializeField] private GameObject tribunesPrefab;
        [HideInInspector] public List<GameObject> tribunesList = new List<GameObject>();
       
       [HideInInspector] public GameObject tribunes; 
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
            for(int i = tribunesPos.Count - 1; i >= 0; i--) {
                tribunes = Instantiate(tribunesPrefab, tribunesPos[i].transform.position - transform.position, tribunesPos[i].transform.rotation);
                tribunesList.Add(tribunes); 
            }

        }

        public void PlayConfetits() {
            for(int i = conffetisPos.Count - 1; i >= 0; i--) {
                 Instantiate(confettiBurst, conffetisPos[i].position - Vector3.up * 10,confettiBurst.transform.rotation );

            }
        }

        public void PlayTribunesAnim() {
            for(int i = tribunesList.Count - 1; i >= 0; i--) {
                tribunesList[i].GetComponent<Animator>().SetTrigger("Win"); 
            }
        }

        
    }
}