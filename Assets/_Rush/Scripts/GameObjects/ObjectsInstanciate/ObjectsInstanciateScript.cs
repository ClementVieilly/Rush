using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.IsartDigital.Assets._Rush.Scripts.GameObjects.ObjectsInstanciate
{
   public class ObjectsInstanciateScript : MonoBehaviour
    {
        private static List<ObjectsInstanciateScript> list = new List<ObjectsInstanciateScript>(); 

        public static void RemoveAll() {
            for(int i = list.Count - 1; i >= 0; i--) {
                list[i].Destroy(); 
            }
        }
      virtual  public void Init() {
         
            list.Add(this);
       }

       

      virtual  public void Destroy() {
            
            list.RemoveAt(list.IndexOf(this));
            Destroy(gameObject);
            
        }

    }
}
