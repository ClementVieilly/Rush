using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.IsartDigital.Assets._Rush.Scripts
{
    [Serializable]
    public class Inventory
    {
        [SerializeField] private GameObject _tile;
        [SerializeField] private string orientation;
        public GameObject Tile {
            get { return _tile; }
        }
    }
}
