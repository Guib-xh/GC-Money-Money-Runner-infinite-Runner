using System;
using Countdown;
using JetBrains.Annotations;
using UnityEngine;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        //DISPATCH
        [SerializeField] private RSE_RunStarted _runStartedEvent;
        
        //LISTEN
        [SerializeField] private RSE_GameOver _gameOverEvent;
        [SerializeField] private RSE_ReturnToMainMenu _returnToMainMenuEvent;
        
        //WRITE
        
        //READ

        [SerializeField] private GameObject _runRoot;
        [SerializeField] private GameObject _mainMenuRoot;
        [SerializeField] private GameObject _gameOverRoot;
        
        private void Start() => ShowMainMenu();

        private void OnEnable()
        {
            _returnToMainMenuEvent.Event += ShowMainMenu;
            _gameOverEvent.Event += ShowGameOver;
        }

        private void OnDisable()
        {
            _returnToMainMenuEvent.Event -= ShowMainMenu;
            _gameOverEvent.Event -= ShowGameOver;
        }


        public void OnLaunchRunPressed()
        {
            _mainMenuRoot.SetActive(false);
            _runRoot.SetActive(true);
            _runStartedEvent.Dispatch();
        }

        private void ShowMainMenu()
        {
            _mainMenuRoot.SetActive(true);
            _gameOverRoot.SetActive(false);
            _runRoot.SetActive(false);
        }

        private void ShowGameOver()
        {
            _gameOverRoot.SetActive(true);
            _runRoot.SetActive(false);
        }
    }
}