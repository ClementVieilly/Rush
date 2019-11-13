///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 22/10/2019 18:39
///-----------------------------------------------------------------

using Com.IsartDigital.Assets._Rush.Scripts.GameObjects.ObjectsInstanciate;
using UnityEngine;

namespace Com.IsartDigital.Rush.GameObjects.ObjectsInstanciate {
	public class Turnstile : ObjectsInstanciateScript {
        public int changeSense = -1; 
		
        public void checkSense() {
            changeSense *= -1; 
        }

        
    }
}