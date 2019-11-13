using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.IsartDigital.Assets._Rush.Scripts.GameObjects
{
   public class GameObjects : MonoBehaviour
    {
        private static List<GameObjects> list = new List<GameObjects>();

        public static void DestroyAllGameObjects() {
            for(int i = list.Count - 1; i >= 0; i--) {
                list[i].Destroy(); 
            }
        }
        private void Awake() {
            list.Add(this);  
        }

        virtual public void Init() { }

        private void Destroy() {
            list.RemoveAt(list.IndexOf(this));
            Destroy(gameObject); 
        }


    }
}
