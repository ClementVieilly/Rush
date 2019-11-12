///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 05/11/2019 16:10
///-----------------------------------------------------------------

using System;
using Com.IsartDigital.Assets._Rush.Scripts;
using Com.IsartDigital.Rush.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Com.IsartDigital.Rush
{

    public class Hud : MonoBehaviour
    {
        [SerializeField] private GameObject tilesContainer;
        [SerializeField] private Level level;
        [SerializeField] private Player player;
        private GameObject tiles;

        public void Init() {

            Player.OnInventoryEmpty += Player_OnInventoryEmpty; 
            Player.OnRecupTile += Player_OnRecupTile; 
            Player.OnUpdateInventory += Player_OnUpdateInventory; 

            Inventory lInventory;
            Transform lTilesContainer; 
            for(int i = Player.inventory.Count - 1; i >= 0; i--) {
                lInventory = Player.inventory[i];
                lTilesContainer = tilesContainer.transform; 
                tiles = Instantiate(lInventory.Tile, lTilesContainer.GetChild(Player.inventory.Count - 1 -i).transform) ;
                lTilesContainer.GetChild(i).transform.GetChild(0).GetComponent<Text>().gameObject.SetActive(true);
                lTilesContainer.GetChild(i).transform.GetChild(1).GetComponent<Image>().gameObject.SetActive(true);
                lTilesContainer.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = lInventory.TilesList.Count.ToString();
                tiles.transform.localScale = new Vector3(80, 80, 80);
                tiles.transform.rotation = Quaternion.AngleAxis(90,tiles.transform.right) *  lInventory.Orientation;   
            } 
        }

        private void Player_OnUpdateInventory(int index) {
            tilesContainer.transform.GetChild(Player.inventory.Count - 1 - player.index).transform.GetChild(0).GetComponent<Text>().text = index.ToString();
        }

        private void Player_OnRecupTile(int index) {
            tilesContainer.transform.GetChild(Player.inventory.Count - 1 - index).transform.GetChild(2).gameObject.SetActive(true);

        }

        private void Player_OnInventoryEmpty(int index) {
            tilesContainer.transform.GetChild(Player.inventory.Count - 1 - index).transform.GetChild(2).gameObject.SetActive(false) ; 
        }

        public void ChangeIndexOfInventory(int index) {
            player.index = (Player.inventory.Count - 1) - index;
            player.SetActiveFalseAllPreview(); 
        }


        private void OnDestroy() {
            Player.OnInventoryEmpty -= Player_OnInventoryEmpty;
            Player.OnRecupTile -= Player_OnRecupTile;
            Player.OnUpdateInventory -= Player_OnUpdateInventory;



        }
    }
}