///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 11/11/2019 16:10
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Rush.UI {
    public delegate void LevelSelectorEventHandler(int level); 
    public class LevelSelector : MonoBehaviour {
        public static event LevelSelectorEventHandler OnChooseLevel;
        private int index = 0;

        public void TestLevel (int level) {
            GetComponent<Animator>().SetTrigger("Disappear"); 
            index = level; 
        }

        private void AnimEnd() {
            OnChooseLevel?.Invoke(index);
        }
        

    }
}