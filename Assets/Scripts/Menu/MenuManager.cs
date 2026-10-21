using System;
using JetBrains.Annotations;
using UnityEngine;

namespace UI
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private RSE_RunStarted _rseRunStartedEvent;
        [SerializeField] private RSE_ReturnToMainMenu _returnToMainMenuEvent;
        [SerializeField] private GameObject _runRoot;
        [SerializeField] private GameObject _mainMenuRoot;
        
        private void Start() => ShowMainMenu();

        private void OnEnable()
        {
            _returnToMainMenuEvent.Event += ShowMainMenu;
        }

        private void OnDisable()
        {
            _returnToMainMenuEvent.Event -= ShowMainMenu;
        }


        public void OnLaunchRunPressed()
        {
            _mainMenuRoot.SetActive(false);
            _runRoot.SetActive(true);
            _rseRunStartedEvent.Dispatch();
        }

        private void ShowMainMenu()
        {
            _mainMenuRoot.SetActive(true);
            _runRoot.SetActive(false);
        }
    }
}