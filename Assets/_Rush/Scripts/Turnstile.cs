///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 22/10/2019 18:39
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Rush {
	public class Turnstile : MonoBehaviour {
        public int changeSense = -1; 
		
        public void checkSense() {
            changeSense *= -1; 
        }
	}
}