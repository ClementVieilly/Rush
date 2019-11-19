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
        [SerializeField] private Hud hudReflexion;
        [SerializeField] private GameObject hudAction;
        [SerializeField] private Pause pause;
        [SerializeField] private GameObject WinScreen;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private CameraMove cam;
        
        private void Start() {
            Menu.OnClickOnMenu += Menu_OnClickOnMenu;
            LevelSelector.OnChooseLevel += LevelSelector_OnChooseLevel;
           
        }

        private void cam_OnZoomFinish() {
            hudReflexion.GetComponent<Animator>().SetTrigger("Appear");
            cam.OnZoomFinish -= cam_OnZoomFinish;
            //hudReflexion.Init();

        }

        public void ControllerManager_OnEchapDown(float axeX, float axeY) {
            SetPause(); 
        }

        public void SetPause() {
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
            cam.OnZoomFinish += cam_OnZoomFinish;

            levelSelector.gameObject.SetActive(false);
            hudReflexion.gameObject.SetActive(true);
            
            gameManager.Init(level);
            
            ControllerManager.OnEchapDown += ControllerManager_OnEchapDown;
        }

        private void Menu_OnClickOnMenu() {
            menu.gameObject.SetActive(false);
            levelSelector.gameObject.SetActive(true);
        }

        public void QuitLevel() {
            if(pause.isActiveAndEnabled) {
                
                pause.gameObject.SetActive(false);
            }
            else WinScreen.SetActive(false);
            gameManager.DestroyLevel();
            levelSelector.gameObject.SetActive(true);
            hudReflexion.gameObject.SetActive(false);
            hudReflexion.ResetHud();

        }

        public void DisplayWin() {
            ControllerManager.OnEchapDown -= ControllerManager_OnEchapDown;
            hudAction.SetActive(false); 
            WinScreen.SetActive(true); 
        }

        public void ResetLevel() {
            //ControllerManager.OnEchapDown += ControllerManager_OnEchapDown;
            pause.gameObject.SetActive(false);
            gameManager.ResetLevel();
            hudReflexion.ResetHud(); 
            hudReflexion.Init(); 
        }
        private void OnDestroy() {
            Menu.OnClickOnMenu -= Menu_OnClickOnMenu;
            LevelSelector.OnChooseLevel -= LevelSelector_OnChooseLevel;
            ControllerManager.OnEchapDown -= ControllerManager_OnEchapDown;
        }


    }
}