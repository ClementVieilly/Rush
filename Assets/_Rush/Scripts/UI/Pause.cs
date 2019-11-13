///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 13/11/2019 11:08
///-----------------------------------------------------------------

using Com.IsartDigital.Rush.Manager;
using UnityEngine;

namespace Com.IsartDigital.Rush.UI {
	public class Pause : MonoBehaviour {
        [SerializeField] UIManager UIManager; 
        private void Start() {
            
        }
        public void OnClickOnResetButton() {
            UIManager.ResetLevel();
            Debug.Log("Reset Level"); 
        }

        public void OnClickOnQuitButton() {
            Debug.Log("Quit Level");
            UIManager.QuitLevel(); 
        }
    }
}