using System;
using UnityEngine;

namespace UI
{
    public class InterStageUIManager : MonoBehaviour
    {
        
        [SerializeField] private RSE_ContinuRun _continuRunEvent;
        [SerializeField] private RSE_ReturnToMainMenu _returnToMainMenuEvent;
        
        [SerializeField] private GameObject _interStagePanel;


        private void OnEnable()
        {
        }

        public void OnContinuePressed()
        {
            Debug.Log("Dispatch Continue Run");
            _continuRunEvent.Dispatch();
            _interStagePanel.SetActive(false);
        }

        public void OnReturnMainMenuPressed()
        {
            Debug.Log("Dispatch Return MainMenu");
            _interStagePanel.SetActive(false);
            _returnToMainMenuEvent.Dispatch();
        }
    }
}