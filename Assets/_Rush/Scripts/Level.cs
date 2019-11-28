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
    public class Level : MonoBehaviour
    {
        public List<Inventory> inventoryLevel = new List<Inventory>();
        public float radius;

        public List<Transform> conffetisPos = new List<Transform>();
        [SerializeField] private List<Transform> tribunesPos = new List<Transform>();
        [SerializeField] private ParticleSystem confettiBurst;
        [SerializeField] private GameObject tribunesRedPrefab;
        [SerializeField] private GameObject tribunesBluePrefab;
        [HideInInspector] public List<GameObject> tribunesRedList = new List<GameObject>();
        [HideInInspector] public List<GameObject> tribunesBlueList = new List<GameObject>();

        [HideInInspector] public GameObject tribunes;
        public void Init() {

            Inventory lInventory;

            // for(int i = confettiBurst.Count - 1; i >= 0; i--) confettiBurst[i].GetComponent<ParticleSystem>().Stop();
            tribunesBlueList.Clear(); 
            tribunesRedList.Clear(); 
            for(int i = inventoryLevel.Count - 1; i >= 0; i--) {
                lInventory = inventoryLevel[i];
                lInventory.TilesList.Clear();
                for(int j = lInventory.getCount - 1; j >= 0; j--) {
                    lInventory.TilesList.Add(lInventory.Tile);

                }
            }
            for(int i = tribunesPos.Count - 1; i >= 0; i--) {
                if(i > 2) {
                    tribunes = Instantiate(tribunesBluePrefab, tribunesPos[i].transform.position - transform.position, tribunesPos[i].transform.rotation);
                    tribunesBlueList.Add(tribunes);
                }
                else {
                    tribunes = Instantiate(tribunesRedPrefab, tribunesPos[i].transform.position - transform.position, tribunesPos[i].transform.rotation);
                    tribunesRedList.Add(tribunes);

                }
            }

            //tribunes.GetComponent<Animator>().

        }

        public void PlayConfetits() {
            for(int i = conffetisPos.Count - 1; i >= 0; i--) {
                Instantiate(confettiBurst, conffetisPos[i].position - Vector3.up * 10, confettiBurst.transform.rotation);

            }
        }

        public void PlayTribunesRedAnim() {
            for(int i = tribunesRedList.Count - 1; i >= 0; i--) {
                tribunesRedList[i].GetComponent<Animator>().SetTrigger("Win");
            }
        }
        public void PlayTribunesBlueAnim() {
            for(int i = tribunesBlueList.Count - 1; i >= 0; i--) {
                tribunesBlueList[i].GetComponent<Animator>().SetTrigger("Win");
            }
        }public void StopTribunesBlueAnim() {
            for(int i = tribunesBlueList.Count - 1; i >= 0; i--) {
                tribunesBlueList[i].GetComponent<Animator>().SetTrigger("Stop");
            }
        }
       


    }
}