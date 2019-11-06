///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 06/11/2019 10:59
///-----------------------------------------------------------------

using Com.IsartDigital.Assets._Rush.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Rush {
	public class Level : MonoBehaviour{
        public List<Inventory> inventoryLevel = new List<Inventory>(); 

        internal void Init() {

            Inventory lInventory;
           
            for(int i = 0; i < inventoryLevel.Count; i++) {
                lInventory = inventoryLevel[i];
                lInventory.TilesList.Clear();
                for(int j = 0; j < lInventory.getCount; j++) {
                    lInventory.TilesList.Add(lInventory.Tile);
                   
                }
            }

            
        }
    }
}