///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 05/11/2019 16:10
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using Com.IsartDigital.Assets._Rush.Scripts;
using Com.IsartDigital.Assets._Rush.Scripts.GameObjects.ObjectsInstanciate;
using Com.IsartDigital.Rush.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Com.IsartDigital.Rush.UI {

    public class Hud : MonoBehaviour
    {
        [SerializeField] private GameObject tilesContainerContainer;
        [SerializeField] private Level level;
        [SerializeField] private Player player;
        private GameObject tiles;
        private float angle;
        private float speed = 100;
        private List<Transform> listOfTilesTransform = new List<Transform>(); 

        private void Start() {
            Player.OnInventoryEmpty += Player_OnInventoryEmpty;
            Player.OnRecupTile += Player_OnRecupTile;
            Player.OnUpdateInventory += Player_OnUpdateInventory;
            CameraMove.OnCameraMove += ControllerManager_OnKeyDown;
        }

        private void ControllerManager_OnKeyDown(Vector3 eulerAngle) {
            for(int i = listOfTilesTransform.Count - 1; i >= 0; i--) {
                listOfTilesTransform[i].eulerAngles = new Vector3(0,0 , eulerAngle.y ); 
            }
           
        }

        public void Init() {
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
                tiles.transform.localScale = new Vector3(80, 80, 80);
                tiles.transform.rotation = Quaternion.AngleAxis(90,tiles.transform.right) *  lInventory.Orientation;   
            } 
        }

        private void Player_OnUpdateInventory(int index) {
            tilesContainerContainer.transform.GetChild(Player.inventory.Count - 1 - player.index).transform.GetChild(0).GetComponent<Text>().text = index.ToString();
        }

        private void Player_OnRecupTile(int index) {
            tilesContainerContainer.transform.GetChild(Player.inventory.Count - 1 - index).transform.GetChild(2).gameObject.SetActive(true);

        }

        private void Player_OnInventoryEmpty(int index) {
            tilesContainerContainer.transform.GetChild(Player.inventory.Count - 1 - index).transform.GetChild(2).gameObject.SetActive(false) ; 
        }

        public void ChangeIndexOfInventory(int index) {
            player.index = (Player.inventory.Count - 1) - index;
            player.SetActiveFalseAllPreview(); 
        }


        private void OnDestroy() {
            Player.OnInventoryEmpty -= Player_OnInventoryEmpty;
            Player.OnRecupTile -= Player_OnRecupTile;
            Player.OnUpdateInventory -= Player_OnUpdateInventory;
            CameraMove.OnCameraMove -= ControllerManager_OnKeyDown;
        }

        public void ResetHud() {
            for(int i = listOfTilesTransform.Count - 1; i >= 0; i--) {
                Destroy(listOfTilesTransform[i].GetChild(0).gameObject);
                listOfTilesTransform[i].rotation = Quaternion.identity;
                listOfTilesTransform[i].gameObject.SetActive(true); 
            }

        }
    }
}