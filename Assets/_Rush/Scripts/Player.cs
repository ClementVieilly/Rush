///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 06/11/2019 09:42
///-----------------------------------------------------------------

using Com.IsartDigital.Assets._Rush.Scripts;
using Com.IsartDigital.Rush.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Rush
{
    public class Player : MonoBehaviour
    {
        public static List<Inventory> inventory = new List<Inventory>();
        protected GameObject currentTile;
        protected int index = 0;
        [SerializeField] protected Camera cam;
        [SerializeField] protected GameObject previewPrefab;
        private GameObject preview;
        protected bool hitSomething;
        protected bool notFree;
        protected RaycastHit hitInfo;
        private bool allListEmpty = false; 

        public void Init() {
            preview = Instantiate(previewPrefab);

            currentTile = inventory[index].TilesList[0];
            ControllerManager.OnMouse0Down += ControllerManager_OnMouse0Down;
        }

        private void ControllerManager_OnMouse0Down(float axeX, float axeY) {

            if(!hitSomething) return;
            /*if(hitInfo.collider.gameObject.layer == 8) {
                currentTile = hitInfo.collider.gameObject;
                currentTile.layer = 2;
                
                return;
            }*/
            if(inventory[index].TilesList.Count > 0) {
                currentTile = Instantiate(currentTile, hitInfo.collider.gameObject.transform.position + Vector3.up / 2, inventory[index].Orientation);
                currentTile.layer = 8;

                inventory[index].TilesList.RemoveAt(0);

                if(inventory[index].TilesList.Count == 0) {
                    index++;
                    index = Mathf.Clamp(index, 0, inventory.Count - 1);
                    if(inventory[index].TilesList.Count == 0) allListEmpty = true; 

                }
            }
            
              

        }



        private void Update() {
            if(allListEmpty) return; 
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            hitSomething = Physics.Raycast(ray, out hitInfo, 30);
            if(!hitSomething) {
                preview.SetActive(false);
                return;
            }

            notFree = Physics.Raycast(hitInfo.collider.transform.position, Vector3.up, 10);

            if(!notFree) {
                for(int i = 0; i < preview.transform.childCount; i++) {
                    if(currentTile.CompareTag(preview.transform.GetChild(i).tag)) {

                        preview.transform.GetChild(i).gameObject.SetActive(true);
                        break;
                    }
                }
                currentTile = inventory[index].TilesList[0];
                preview.SetActive(true);
                preview.transform.position = hitInfo.collider.gameObject.transform.position + Vector3.up / 2;
                preview.transform.rotation = inventory[index].Orientation;
            }
            else preview.SetActive(false);
        }



    }
}