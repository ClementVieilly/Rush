///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 06/11/2019 09:42
///-----------------------------------------------------------------

using Com.IsartDigital.Assets._Rush.Scripts;
using Com.IsartDigital.Assets._Rush.Scripts.GameObjects.ObjectsInstanciate;
using Com.IsartDigital.Rush.Manager;
using Pixelplacement;
using Pixelplacement.TweenSystem;
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
        public static event PlayerEventHandler OnIndexChange; 
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
        protected RaycastHit recupTileOnground;
        private bool allListEmpty = false;
        private uint test = 0; 
        private Action DoAction;

        //tag
        private string spawnTag = "Ground"; 
        private string targetTag = "Target"; 
        private void Awake() {
#if UNITY_ANDROID || UNITY_IOS
            
            

#else
            ControllerManager.OnMouse0Down += ControllerManager_OnMouse0Down;
          
#endif
            SetModeVoid();
            preview = Instantiate(previewPrefab);
            SetAllPreviewAlpha();
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

            if(!hitSomething || ground.collider.CompareTag("Wall")|| ground.collider.CompareTag(targetTag)) {
                
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
            DisplayTile(); 
            
        }

        private void DisplayTile() {
           
            if(isVoid || !hitSomething || ground.collider.CompareTag("Wall")) return;
            if(notFree && RecupTile()) return;
            

            if(inventory[index].TilesList.Count > 0) {
                currentTile = Instantiate(currentTile, ground.collider.gameObject.transform.position + Vector3.up * 1.5f, inventory[index].Orientation);
                Tween.LocalPosition(currentTile.transform, ground.collider.gameObject.transform.position + Vector3.up / 2, 0.2f, 0, Tween.EaseIn);
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

        private void TouchTest() {
            if(Input.touchCount > 0) {


                Touch touch = Input.GetTouch(0);
                switch(touch.phase) {
                    case TouchPhase.Began:
                        DisplayTile();
                        break;
                }
            }
        }
        private void Update() {

            DoAction();
            TouchTest(); 
        }

        private void CheckTabsCount() {
            for(int i = inventory.Count - 1; i >= 0; i--) {
                if(inventory[i].TilesList.Count != 0) {
                    index = i;
                    OnIndexChange?.Invoke(index); 
                    break;
                }
            }
            if(inventory[index].TilesList.Count == 0) {
                allListEmpty = true;
               // OnInventoryEmpty?.Invoke(index);
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

        private TweenBase tileScaleTween;

        private bool RecupTile() {
            Inventory lInventory;
            for(int i = inventory.Count - 1; i >= 0; i--) {

                lInventory = inventory[i];
                if(tileOnground.collider.CompareTag(lInventory.Tile.tag) && tileOnground.transform.rotation == lInventory.Orientation) {

                    recupTileOnground = tileOnground;
                    recupTileOnground.collider.gameObject.layer = 2;


                    lInventory.TilesList.Add(lInventory.Tile);
                    Tween.LocalPosition(recupTileOnground.transform, recupTileOnground.transform.position + Vector3.up / 2, 0.2f, 0, Tween.EaseIn);
                    tileScaleTween = Tween.LocalScale(recupTileOnground.transform, Vector3.zero, 0.2f, 0.2f, Tween.EaseIn, Tween.LoopType.None, null, () => { testPreview(index); }) ;
                  
                   
                   // CheckTabsCount();
                    index = inventory.IndexOf(lInventory);
                    OnUpdateInventory?.Invoke(inventory[index].TilesList.Count);
                    OnIndexChange?.Invoke(index); 
                    
                    currentTile = lInventory.TilesList[0];
                    SetActiveFalseAllPreview();
                    if(allListEmpty)  allListEmpty = false; 
                    return true;
                }
                if(tileOnground.collider.CompareTag(targetTag) || tileOnground.collider.CompareTag(spawnTag)||tileOnground.collider.CompareTag("Wall") || tileOnground.collider.CompareTag("Teleport")) return true; 

            }

            return false; 
            
        }

       private void SetAllPreviewAlpha() {
            for(int i = preview.transform.childCount - 1; i >= 0; i--) {
                Color a = preview.transform.GetChild(i).transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
                a.a = 0.4f;
                preview.transform.GetChild(i).transform.GetChild(0).GetComponent<MeshRenderer>().material.color = a;
                
            }
        }

        public void SetActiveFalseAllPreview() {
            for(int i = preview.transform.childCount - 1; i >= 0; i--) {
                preview.transform.GetChild(i).gameObject.SetActive(false); 
            }
        }

        private void testPreview(int index) {
            recupTileOnground.collider.gameObject.GetComponent<ObjectsInstanciateScript>().Destroy();
            OnRecupTile?.Invoke(index);
        }
        
    }
}
