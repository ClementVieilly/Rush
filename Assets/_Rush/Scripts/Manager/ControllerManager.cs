///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 05/11/2019 14:31
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Rush.Manager {
    public delegate void ControllerManagerEventHandler(float axeX = 0, float axeY = 0); 
    
    //private string vertical
	public class ControllerManager : MonoBehaviour {

        public static event ControllerManagerEventHandler OnMouseClick1Held;
        public static event ControllerManagerEventHandler OnKeyDown ;
        public static event ControllerManagerEventHandler OnMouse0Down ;

        private string vertical = "Vertical"; 
        private string horizontal = "Horizontal"; 
        private string mouseX = "Mouse X"; 
        private string mouseY = "Mouse Y";  
        private void Update() {
            if(Input.GetMouseButton(1)) OnMouseClick1Held?.Invoke(Input.GetAxis(mouseX), Input.GetAxis(mouseY)) ; //envoyer un event seulement si la souris bouges aussi !
            if(Input.GetAxis(vertical) != 0 || Input.GetAxis(horizontal) != 0) OnKeyDown?.Invoke(Input.GetAxis(horizontal), Input.GetAxis(vertical));

            if(Input.GetMouseButtonDown(0)) OnMouse0Down?.Invoke(); 
        }

    }
}