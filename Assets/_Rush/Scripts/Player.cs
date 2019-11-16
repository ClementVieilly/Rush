///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 06/11/2019 09:42
///-----------------------------------------------------------------

using Com.IsartDigital.Assets._Rush.Scripts;
using Com.IsartDigital.Assets._Rush.Scripts.GameObjects.ObjectsInstanciate;
using Com.IsartDigital.Rush.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Com.IsartDigital.Rush
{       public delegate void PlayerEventHandler(int index); 
    public class Player : MonoBehaviour
    {   
        public static List<Inventory> inventory = new List<Inventory>();
        public static event PlayerEventHandler OnInventoryEmpty; 
        public static event PlayerEventHandler OnRecupTile; 
        public static event PlayerEventHandler OnUpdateInventory; 
        protected GameObject currentTile;
        public int index;
        [SerializeField] protected Camera cam;
        [SerializeField] protected GameObject previewPrefab;
        private GameObject preview;
        protected bool hitSomething;
        protected bool notFree;
        protected bool isVoid; 
        protected RaycastHit ground;
        protected RaycastHit tileOnground;
        private bool allListEmpty = false;

        private Action DoAction;
        private void Awake() {
            ControllerManager.OnMouse0Down += ControllerManager_OnMouse0Down;
            SetModeVoid();
            preview = Instantiate(previewPrefab);
        }
        public void Init() {
            SetActiveFalseAllPreview(); 
            allListEmpty = false; 
            index = inventory.Count - 1;
            
            currentTile = inventory[index].TilesList[0];
            SetModeNormal();
          
        }
        public void SetModeVoid() {
            DoAction = DoActionVoid;
            isVoid = true; 
        }

        public void SetModeNormal() {
            isVoid = false; 
            DoAction = DoActionNormal; 
        }



        private void DoActionVoid() {

        }

        private void DoActionNormal() {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            hitSomething = Physics.Raycast(ray, out ground, 30);

            if(!hitSomething) {
                preview.SetActive(false);
                return;
            }

            notFree = Physics.Raycast(ground.collider.transform.position, Vector3.up, out tileOnground,10);
            
            if(allListEmpty) return;
            if(!notFree) {
                
                DisplayPreview();
            }

            else preview.SetActive(false);
        }


        private void ControllerManager_OnMouse0Down(float axeX, float axeY) {

            if(isVoid || !hitSomething) return;
            if(notFree && RecupTile()) return; 

            if(inventory[index].TilesList.Count > 0) {
                currentTile = Instantiate(currentTile, ground.collider.gameObject.transform.position + Vector3.up / 2, inventory[index].Orientation);
                currentTile.layer = 0;
                currentTile.gameObject.GetComponent<ObjectsInstanciateScript>().Init();  
                currentTile.transform.GetChild(0).gameObject.layer = 0;
                inventory[index].TilesList.RemoveAt(0);
                 
                SetActiveFalseAllPreview();
                OnUpdateInventory?.Invoke(inventory[index].TilesList.Count); 
                if(inventory[index].TilesList.Count == 0) {
                    OnInventoryEmpty?.Invoke(index);
                    CheckTabsCount();
                    index = Mathf.Clamp(index, 0, inventory.Count - 1);
                }
            }
        }
        private void Update() {
            DoAction(); 
        }

        private void CheckTabsCount() {
            for(int i = inventory.Count - 1; i >= 0; i--) {
                if(inventory[i].TilesList.Count != 0) {
                    
                    index = i;
                   
                    break;
                }
            }
            if(inventory[index].TilesList.Count == 0) {
                allListEmpty = true;
                OnInventoryEmpty?.Invoke(index);
            }
            else allListEmpty = false; 
        }

        private void DisplayPreview() {
            currentTile = inventory[index].TilesList[0];
            for(int i = preview.transform.childCount - 1; i >= 0; i--) {
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
            for(int i = inventory.Count - 1; i >= 0; i--) {
                lInventory = inventory[i];
                if(tileOnground.collider.CompareTag(lInventory.Tile.tag) && tileOnground.transform.rotation == lInventory.Orientation) {
                    tileOnground.collider.gameObject.GetComponent<ObjectsInstanciateScript>().Destroy();
                    
                    lInventory.TilesList.Add(lInventory.Tile);
                    CheckTabsCount();
                    index = inventory.IndexOf(lInventory);
                    OnUpdateInventory?.Invoke(inventory[index].TilesList.Count);
                    OnRecupTile?.Invoke(index);
                    currentTile = lInventory.TilesList[0];
                    SetActiveFalseAllPreview(); 
                    return true;
                }
                if(tileOnground.collider.CompareTag("Target") || tileOnground.collider.CompareTag("Ground")) return true; 

            }

            return false; 
            
        }

        public void SetActiveFalseAllPreview() {
            for(int i = preview.transform.childCount - 1; i >= 0; i--) {
                preview.transform.GetChild(i).gameObject.SetActive(false); 
            }
        }


        
    }
}
