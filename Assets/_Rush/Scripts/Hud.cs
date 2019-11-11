///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 05/11/2019 16:10
///-----------------------------------------------------------------

using Com.IsartDigital.Assets._Rush.Scripts;
using UnityEngine;

namespace Com.IsartDigital.Rush {

    public class Hud : MonoBehaviour
    {
        [SerializeField] private GameObject tilesContainer; 
        [SerializeField] private Level level;
        [SerializeField] private Player player;
        private GameObject tiles;

        public void Init() {
            Inventory inventory;
           for(int i = Player.inventory.Count - 1; i >= 0; i--) {
                inventory = Player.inventory[i];
                tiles = Instantiate(inventory.Tile, tilesContainer.transform.GetChild(tilesContainer.transform.childCount - (i +2)).transform);
                tiles.transform.localScale = new Vector3(80, 80, 80);
                tiles.layer = 2;
            }
        }

     

       
    }
}