using UnityEngine;

namespace UI
{
    public class InterStageDisplay : MonoBehaviour
    {
        //DISPATCH
        [SerializeField] private RSE_ContinuRun _continuRunEvent;
        [SerializeField] private RSE_ReturnToMainMenu _returnToMainMenuEvent;
        
        //LISTEN
        
        //WRITE
        
        //READ
        
        //UI
        [SerializeField] private GameObject _interStagePanel;
        
        public void OnContinueClicked()
        {
            _interStagePanel.SetActive(false);
            _continuRunEvent.Dispatch();
        }

        public void OnReturnMainMenuClicked()
        {
            _returnToMainMenuEvent.Dispatch();
        }
    }
}