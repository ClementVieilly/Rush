///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 05/11/2019 16:24
///-----------------------------------------------------------------

using Com.IsartDigital.Rush.UI;
using System;
using UnityEngine;

namespace Com.IsartDigital.Rush.Manager
{

    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Menu menu;
        [SerializeField] private LevelSelector levelSelector;
        [SerializeField] private Hud hud;
        [SerializeField] private Pause pause;
        [SerializeField] private GameManager gameManager;
        private void Start() {
            Menu.OnClickOnMenu += Menu_OnClickOnMenu;
            LevelSelector.OnChooseLevel += LevelSelector_OnChooseLevel;
            ControllerManager.OnEchapDown += ControllerManager_OnEchapDown;
        }

        private void ControllerManager_OnEchapDown(float axeX, float axeY) {
            if(!gameManager.onPause) {
                pause.gameObject.SetActive(true);
                gameManager.SetPause();

            }

            else {
                pause.gameObject.SetActive(false);
                gameManager.SetPlay(); 
            }
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

        public void QuitLevel() {
            gameManager.DestroyLevel();
            hud.gameObject.SetActive(false);
            levelSelector.gameObject.SetActive(true);
            pause.gameObject.SetActive(false); 
        }

        public void ResetLevel() {
            pause.gameObject.SetActive(false);
            gameManager.ResetLevel();
            hud.Init(); 
        }
        private void OnDestroy() {
            Menu.OnClickOnMenu -= Menu_OnClickOnMenu;
            LevelSelector.OnChooseLevel -= LevelSelector_OnChooseLevel;
            ControllerManager.OnEchapDown -= ControllerManager_OnEchapDown;

        }


    }
}