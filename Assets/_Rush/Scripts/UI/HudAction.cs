///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 17/11/2019 18:41
///-----------------------------------------------------------------

using Com.IsartDigital.Rush.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Com.IsartDigital.Rush.UI {
	public class HudAction : MonoBehaviour {
        [SerializeField] Slider slider; 
      
		
		private void Update () {
            TimeManager.Speed = slider.value; 
		}
	}
}