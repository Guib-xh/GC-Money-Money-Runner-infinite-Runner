using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pause
{
    public class PauseManager : MonoBehaviour
    {
        //DISPATCH
        [SerializeField] private RSE_ReturnToMainMenu _returnToMainMenuEvent;
        [SerializeField] private RSE_RunStarted _runStartedEvent;
        
        //LISTEN
        [SerializeField] private InputActionReference inputPauseActionReference;
        
        //WRITE

        //UI
        [SerializeField] private GameObject pauseCanvas;
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject commandePanel;
        
        
        public bool isPaused;

        private void OnEnable()
        {
            inputPauseActionReference.action.performed += OnPauseButtunPressed;
            _runStartedEvent.Event += Initialize;
        }
        
        private void OnDisable()
        {
            inputPauseActionReference.action.performed -= OnPauseButtunPressed;
            _runStartedEvent.Event -= Initialize;
            
        }

        private void Initialize()
        {
            isPaused = false;
            pauseCanvas.SetActive(false);
            pausePanel.SetActive(true);
            commandePanel.SetActive(false);
            
        }

        private void OnPauseButtunPressed(InputAction.CallbackContext ctx)
        {
            if (!isPaused)
            {
                OpenPauseMenu();
            }
            else
            {
                ClosePauseMenu();
            }
           
        }
        
        public void OpenPauseMenu()
        {
            isPaused = true;
            Time.timeScale = 0f;
            pauseCanvas.SetActive(true);
        }
        
        public void ClosePauseMenu()
        {
            isPaused = false;
            Time.timeScale = 1f;
            pauseCanvas.SetActive(false);
        }
        
        public void ExitGame()
        {
            isPaused = false;
            Time.timeScale = 1f;
            _returnToMainMenuEvent.Dispatch();
        }
        
    }
}