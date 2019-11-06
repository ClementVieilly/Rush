using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.IsartDigital.Assets._Rush.Scripts
{
    enum direction : int
    {
          FORWARD  = 0, 
          BACKWARD  = 180, 
          RIGHT  = 90, 
          LEFT  = -90
    }; 

    [Serializable]
    public class Inventory
    {
        [SerializeField] private GameObject _tile;
        [SerializeField] private direction _orientation;
        [SerializeField] private int _count;
        private List<GameObject> _tilesList = new List<GameObject>();

        public GameObject Tile { get { return _tile; } }
        public int getCount{ get { return _count; } }
        public Quaternion Orientation { get { return Quaternion.AngleAxis((int)_orientation, Vector3.up); } }
        public List<GameObject> TilesList { get { return _tilesList; } }
           
        
    }
}
