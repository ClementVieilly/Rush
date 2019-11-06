///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 06/11/2019 10:59
///-----------------------------------------------------------------

using Com.IsartDigital.Assets._Rush.Scripts;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Rush {
	public class Level : MonoBehaviour{
        public List<Inventory> inventoryLevel = new List<Inventory>();


        private void Start () {
            for(int i = 0; i < inventoryLevel.Count; i++) {
                
            }
        }

    }
}