///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 05/11/2019 16:24
///-----------------------------------------------------------------

using System;
using UnityEngine;

namespace Com.IsartDigital.Rush.Manager {
   
	public class UIManager : MonoBehaviour {
        [SerializeField] private Menu menu; 
        [SerializeField] private LevelSelector levelSelector; 
        [SerializeField] private Hud hud; 
        [SerializeField] private GameManager gameManager; 
        private void Start () {
            Menu.OnClickOnMenu += Menu_OnClickOnMenu; 
            LevelSelector.OnChooseLevel += LevelSelector_OnChooseLevel; 
		}

        private void LevelSelector_OnChooseLevel(int level) {
            levelSelector.gameObject.SetActive(false);
            hud.gameObject.SetActive(true);
            gameManager.Init();
          
        }

        private void Menu_OnClickOnMenu() {
            menu.gameObject.SetActive(false); 
            levelSelector.gameObject.SetActive(true); 
        }
    }
}