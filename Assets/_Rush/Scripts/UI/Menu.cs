///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 11/11/2019 15:49
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Rush.UI {

    public delegate void MenuEventHandler();
	public class Menu : MonoBehaviour {
         public static event MenuEventHandler OnClickOnMenu;
		public void onClick() {
            GetComponent<Animator>().SetTrigger("Disappear");

        }
        public void AnimEnd() {
            OnClickOnMenu?.Invoke(); 
        }
        private void OnEnable() {
            GetComponent<Animator>().SetTrigger("Appear"); 
        }
    }
}