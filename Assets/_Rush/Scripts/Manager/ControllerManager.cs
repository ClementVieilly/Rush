///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 05/11/2019 14:31
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Rush.Manager
{
    public delegate void ControllerManagerEventHandler(float axeX = 0, float axeY = 0);


    public class ControllerManager : MonoBehaviour
    {

        public static event ControllerManagerEventHandler OnMouseClick1Held;
        public static event ControllerManagerEventHandler OnKeyDown;
        public static event ControllerManagerEventHandler OnMouse0Down;
        public static event ControllerManagerEventHandler OnEchapDown;
        public static event ControllerManagerEventHandler OnTouchDown;

        private string vertical = "Vertical";
        private string horizontal = "Horizontal";
        private string mouseX = "Mouse X";
        private string mouseY = "Mouse Y";
        private void Update() {

#if UNITY_ANDROID || UNITY_IOS

            if(Input.touchCount > 0) {
                Touch touch = Input.GetTouch(0);

                switch(touch.phase) {
                   
                    case TouchPhase.Moved:
                        OnMouseClick1Held?.Invoke(touch.deltaPosition.x, touch.deltaPosition.y);
                        break;
                }

            }




#else
            if(Input.GetMouseButton(1)) OnMouseClick1Held?.Invoke(Input.GetAxis(mouseX), Input.GetAxis(mouseY)); //envoyer un event seulement si la souris bouges aussi !
            if(Input.GetAxis(vertical) != 0 || Input.GetAxis(horizontal) != 0) OnKeyDown?.Invoke(Input.GetAxis(horizontal), Input.GetAxis(vertical));

            if(Input.GetMouseButtonDown(0)) OnMouse0Down?.Invoke();
            if(Input.GetKeyDown(KeyCode.Escape)) OnEchapDown?.Invoke();

#endif




        }


    }
}
