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
        protected RaycastHit ground;
        protected RaycastHit tileOnground;
        private bool allListEmpty = false;

        public void Init() {
            preview = Instantiate(previewPrefab);

            currentTile = inventory[index].TilesList[0];
            ControllerManager.OnMouse0Down += ControllerManager_OnMouse0Down;
        }

        private void ControllerManager_OnMouse0Down(float axeX, float axeY) {

            if(!hitSomething) return;

            if(notFree && RecupTile()) return; 

            if(inventory[index].TilesList.Count > 0) {
                currentTile = Instantiate(currentTile, ground.collider.gameObject.transform.position + Vector3.up / 2, inventory[index].Orientation);
                currentTile.layer = 8;

                inventory[index].TilesList.RemoveAt(0);
                SetActiveFalseAllPreview();
                if(inventory[index].TilesList.Count == 0) {
                    index++;
                    index = Mathf.Clamp(index, 0, inventory.Count - 1);
                   
                    CheckTabsCount();
                }
            }
        }
        private void Update() {

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            hitSomething = Physics.Raycast(ray, out ground, 30);

            if(!hitSomething) {
                preview.SetActive(false);
                return;
            }

            notFree = Physics.Raycast(ground.collider.transform.position, Vector3.up, out tileOnground, 10);

            if(allListEmpty) return;
             if(!notFree) {
                DisplayPreview(); 
             }

            else preview.SetActive(false);
        }

        private void CheckTabsCount() {
            for(int i = 0; i < inventory.Count; i++) {
                if(inventory[i].TilesList.Count != 0) {
                    index = i;
                    break;
                }
            }
            allListEmpty = inventory[index].TilesList.Count == 0 ? true : false;
        }

        private void DisplayPreview() {
            currentTile = inventory[index].TilesList[0];
            for(int i = 0; i < preview.transform.childCount; i++) {
                if(currentTile.CompareTag(preview.transform.GetChild(i).tag)) {
                    
                    preview.transform.GetChild(i).gameObject.SetActive(true);
                    
                    break;
                }
          
            }
            
            preview.SetActive(true);
            preview.transform.position = ground.collider.gameObject.transform.position + Vector3.up / 2;
            preview.transform.rotation = inventory[index].Orientation;

        }

        private bool RecupTile() {
            Inventory lInventory;
            for(int i = 0; i < inventory.Count; i++) {
                lInventory = inventory[i];
                if(tileOnground.collider.CompareTag(lInventory.Tile.tag) && tileOnground.transform.rotation == lInventory.Orientation) {
                    Destroy(tileOnground.collider.gameObject);
                    lInventory.TilesList.Add(lInventory.Tile);
                    CheckTabsCount();
                    index = inventory.IndexOf(lInventory);
                    currentTile = lInventory.TilesList[0];
                    
                    return true;
                }
                
            }

            return false; 
            
        }

        private void SetActiveFalseAllPreview() {
            for(int i = 0; i < preview.transform.childCount; i++) {
                preview.transform.GetChild(i).gameObject.SetActive(false); 
            }
        }

    }
}
