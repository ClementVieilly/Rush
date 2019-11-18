///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 05/11/2019 16:10
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using Com.IsartDigital.Assets._Rush.Scripts;
using Pixelplacement;
using UnityEngine;
using UnityEngine.UI;

namespace Com.IsartDigital.Rush.UI {

    public class Hud : MonoBehaviour
    {
        [SerializeField] private GameObject tilesContainerContainer;
        [SerializeField] private Level level;
        [SerializeField] private Player player;
        [SerializeField] private Button pauseButton; 
        [SerializeField] private AnimationCurve anim; 
        private GameObject tiles;
        private float angle;
        private float speed = 100;
        private int indexOnEmpty = 0; 
        private int indexOnRecup = 0; 
        private List<Transform> listOfTilesTransform = new List<Transform>(); 
        private Vector3 scaleOnClick = new Vector3(1.3f, 1.3f, 1.3f);
        private Vector3 scaleNormal = new Vector3(0.8f, 0.8f, 0.8f);
        private void ControllerManager_OnKeyDown(Vector3 eulerAngle) {
            for(int i = listOfTilesTransform.Count - 1; i >= 0; i--) {
                listOfTilesTransform[i].eulerAngles = new Vector3(0,0 , eulerAngle.y ); 
            }
           
        }

        public void Init() {

#if UNITY_ANDROID || UNITY_IOS
        pauseButton.gameObject.SetActive(true); 
#else

            pauseButton.gameObject.SetActive(false); 
#endif
            CameraMove.OnCameraMove += ControllerManager_OnKeyDown;
            Player.OnInventoryEmpty += Player_OnInventoryEmpty;
            Player.OnRecupTile += Player_OnRecupTile;
            Player.OnUpdateInventory += Player_OnUpdateInventory;
            Player.OnIndexChange += Player_OnIndexChange;

            Inventory lInventory;
            Transform lTilesContainer;
            int index; 
            for(int i = Player.inventory.Count - 1; i >= 0; i--) {
                lInventory = Player.inventory[i];
                lTilesContainer = tilesContainerContainer.transform;
                index = Player.inventory.Count - 1 - i;
                tiles = Instantiate(lInventory.Tile, lTilesContainer.GetChild(index).transform.GetChild(2).transform) ;
                listOfTilesTransform.Add(lTilesContainer.GetChild(index).transform.GetChild(2).transform);
                lTilesContainer.GetChild(index).transform.GetChild(0).GetComponent<Text>().gameObject.SetActive(true);
                lTilesContainer.GetChild(index).transform.GetChild(1).GetComponent<Image>().gameObject.SetActive(true);
                lTilesContainer.GetChild(index).transform.GetChild(0).GetComponent<Text>().text = lInventory.TilesList.Count.ToString();
                tiles.transform.localScale = new Vector3(75, 75, 75);
                tiles.transform.rotation = Quaternion.AngleAxis(-90, tiles.transform.right) *  lInventory.Orientation;
                //Tween.LocalScale(lTilesContainer.GetChild(i).transform.GetChild(2).transform, new Vector3(1.6f, 1.6f, 1.6f), 0.3f, 0.3f*i, Tween.EaseOutBack);
                Tween.LocalScale(lTilesContainer.GetChild(i).transform.GetChild(2).transform, scaleNormal, 0.6f, 0.2f*i, anim);

            }
            Tween.LocalScale(tilesContainerContainer.transform.GetChild(0).transform.GetChild(2).transform, scaleOnClick, 0.3f, 1.5f, Tween.EaseOutBack);
        }

        private void Player_OnIndexChange(int index) {
            Tween.LocalScale(tilesContainerContainer.transform.GetChild(listOfTilesTransform.Count - 1 - index).transform.GetChild(2).transform, scaleOnClick, 0.2f, 0, Tween.EaseInOutBack);
        }

        private void Player_OnUpdateInventory(int index) {
            tilesContainerContainer.transform.GetChild(Player.inventory.Count - 1 - player.index).transform.GetChild(0).GetComponent<Text>().text = index.ToString();
        }

        private void Player_OnRecupTile(int index) {
            ResetScaleHud(); 
            indexOnRecup = index;
            Tween.LocalScale(tilesContainerContainer.transform.GetChild(Player.inventory.Count - 1 - indexOnRecup).transform.GetChild(2).transform, scaleOnClick, 0.3f, 0.3f, Tween.EaseInBack, Tween.LoopType.None, () => { tilesContainerContainer.transform.GetChild(Player.inventory.Count - 1 - indexOnRecup).transform.GetChild(2).gameObject.SetActive(true); });

           /* if(Player.inventory.Count - 1 - index != tilesContainerContainer.transform.childCount -1) */Tween.Value(tilesContainerContainer.transform.GetChild(Player.inventory.Count - 1 - index).GetComponent<RectTransform>().rect.height, 100, changeSlotHeightOnRecupTile, 0.2f, 0, Tween.EaseIn); //tilesContainerContainer.transform.GetChild(Player.inventory.Count - 1 - index).GetComponent<RectTransform>().rect.height,new Vector2()

        }

        private void changeSlotHeightOnRecupTile(float obj) {
            Vector2 size = tilesContainerContainer.transform.GetChild(Player.inventory.Count - 1 - indexOnRecup).GetComponent<RectTransform>().sizeDelta;
            size.y = obj;
            tilesContainerContainer.transform.GetChild(Player.inventory.Count - 1 - indexOnRecup).GetComponent<RectTransform>().sizeDelta = size;
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)tilesContainerContainer.transform);

        }

        private void Player_OnInventoryEmpty(int index) {
            Tween.LocalScale(tilesContainerContainer.transform.GetChild(Player.inventory.Count - 1 - index).transform.GetChild(2).transform, Vector3.zero, 0.3f, 0, Tween.EaseInBack,Tween.LoopType.None, null,()=> { tilesContainerContainer.transform.GetChild(Player.inventory.Count - 1 - index).transform.GetChild(2).gameObject.SetActive(false); });
            indexOnEmpty = index;
            //if(Player.inventory.Count - 1 - index != tilesContainerContainer.transform.childCount - 2) Tween.Value(tilesContainerContainer.transform.GetChild(Player.inventory.Count - 1 - index).GetComponent<RectTransform>().rect.height, -40, changeSlotHeightOnEmptyInventory, 0.2f, 0.3f,Tween.EaseIn);
           
        }

        private void changeSlotHeightOnEmptyInventory(float obj) {
            Vector2 size = tilesContainerContainer.transform.GetChild(Player.inventory.Count - 1-indexOnEmpty).GetComponent<RectTransform>().sizeDelta;
            size.y = obj;
            tilesContainerContainer.transform.GetChild(Player.inventory.Count - 1 - indexOnEmpty).GetComponent<RectTransform>().sizeDelta = size;
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)tilesContainerContainer.transform);
        }

        protected void ChangeIndexOfInventory(int index) {
            if(!tilesContainerContainer.transform.GetChild(index).transform.GetChild(2).gameObject.activeSelf) return;
            ResetScaleHud(); 
           
            player.index = (Player.inventory.Count - 1) - index;
            Tween.LocalScale(tilesContainerContainer.transform.GetChild(index).transform.GetChild(2).transform, scaleOnClick, 0.2f, 0, Tween.EaseInOutBack); 
            
            player.SetActiveFalseAllPreview(); 
        }


        private void OnDestroy() {
            Player.OnInventoryEmpty -= Player_OnInventoryEmpty;
            Player.OnRecupTile -= Player_OnRecupTile;
            Player.OnUpdateInventory -= Player_OnUpdateInventory;
            CameraMove.OnCameraMove -= ControllerManager_OnKeyDown;
        }

        public void ResetHud() {
            CameraMove.OnCameraMove -= ControllerManager_OnKeyDown;
            Player.OnInventoryEmpty -= Player_OnInventoryEmpty;
            Player.OnRecupTile -= Player_OnRecupTile;
            Player.OnUpdateInventory -= Player_OnUpdateInventory;
            for(int i = listOfTilesTransform.Count - 1; i >= 0; i--) {
                Destroy(listOfTilesTransform[i].GetChild(0).gameObject);
                listOfTilesTransform[i].localScale = Vector3.zero; 
                listOfTilesTransform[i].rotation = Quaternion.identity;
                listOfTilesTransform[i].gameObject.SetActive(true);
                listOfTilesTransform[i].eulerAngles = new Vector3(0, 0, 0);
            }

            for(int i = tilesContainerContainer.transform.childCount - 1; i >= 0; i--) {
                tilesContainerContainer.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().gameObject.SetActive(false);
            }

            listOfTilesTransform.Clear(); 

        }

        private void ResetScaleHud() {
            for(int i = listOfTilesTransform.Count - 1; i >= 0; i--) {
                //if(!listOfTilesTransform[i].gameObject.activeSelf) continue; 
                Tween.LocalScale(listOfTilesTransform[i], scaleNormal, 0.2f, 0,Tween.EaseOutBack); 
            }
        }

        
    }
}