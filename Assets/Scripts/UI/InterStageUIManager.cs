using UnityEngine;

namespace UI
{
    public class InterStageUIManager : MonoBehaviour
    {
        
        [SerializeField] private RSE_ContinuRun _continuRunEvent;
        [SerializeField] private RSE_ReturnToMainMenu _returnToMainMenuEvent;
        
        [SerializeField] private GameObject _interStagePanel;
        

        public void OnContinuePressed()
        {
            _interStagePanel.SetActive(false);
            _continuRunEvent.Dispatch();
        }

        public void OnReturnMainMenuPressed()
        {
            _interStagePanel.SetActive(false);
            _returnToMainMenuEvent.Dispatch();
        }
    }
}