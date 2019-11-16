///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 22/10/2019 18:39
///-----------------------------------------------------------------

using Com.IsartDigital.Assets._Rush.Scripts.GameObjects.ObjectsInstanciate;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Rush.GameObjects.ObjectsInstanciate {
	public class Turnstile : ObjectsInstanciateScript {
        private static List<Turnstile> list = new List<Turnstile>(); 
        public int changeSense = 1;

        public static void ResetSense() {
            for(int i = list.Count - 1; i >= 0; i--) {
                list[i].changeSense = 1; 
            }
        }
        override public void Init() {
            base.Init();
            list.Add(this);

        }
            public void checkSense() {
            changeSense *= -1; 
        }

        
    }
}