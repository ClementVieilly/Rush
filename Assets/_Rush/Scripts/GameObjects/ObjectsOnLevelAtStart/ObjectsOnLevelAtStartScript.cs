using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.IsartDigital.Assets._Rush.Scripts.GameObjects.ObjectsOnLevelAtStart
{
   public class ObjectsOnLevelAtStartScript : MonoBehaviour
    {
        private static List<ObjectsOnLevelAtStartScript> list = new List<ObjectsOnLevelAtStartScript>(); 
        public static void InitAllGameObjectAtLevelAtStart() {
            for(int i = list.Count - 1; i >= 0; i--) {
               
                list[i].Init(); 
            }
        }
        
        private void OnEnable() {
            list.Add(this);
           
        }
      virtual public  void Init() {
           
        }
        
       virtual public void Destroy() {
            list.RemoveAt(list.IndexOf(this)); 
            Destroy(gameObject); 
        }
    }
}
