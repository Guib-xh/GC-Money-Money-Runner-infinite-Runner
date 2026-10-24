using Boss;
using Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class InterStageDisplay : MonoBehaviour
    {
        //DISPATCH
        [SerializeField] private RSE_RunStarted _runStartedEvent;
        [SerializeField] private RSE_ContinuRun _continuRunEvent;
        [SerializeField] private RSE_ReturnToMainMenu _returnToMainMenuEvent;
        
        //LISTEN
        [SerializeField] private RSE_BossDefeated _bossDefeatedEvent;
        
        
        //WRITE
        
        //READ
        [SerializeField] private RSO_MoneyChest _moneyChest;
        
        //UI
        [SerializeField] private GameObject _interStageCanvas;
        [SerializeField] private TMP_Text _rewardText;
        [SerializeField] private TMP_Text _moneyChestText;
        

        private void OnEnable()
        {
            _runStartedEvent.Event += Initialize;
            _bossDefeatedEvent.Event += OnBossDefeated;

        }

        private void OnDisable()
        {
            _runStartedEvent.Event -= Initialize;
            _bossDefeatedEvent.Event -= OnBossDefeated;
        }

        private void Initialize()
        {
            _interStageCanvas.SetActive(false);
        }

        private void OnBossDefeated(int reward)
        {
            Debug.Log("Reward : " + reward);
            _rewardText.text = $"{reward.ToString()} $";
            _moneyChestText.text = $"{_moneyChest.Value} $";
            _interStageCanvas.SetActive(true);
        }
        
        public void OnContinueClicked()
        {
            _interStageCanvas.SetActive(false);
            _continuRunEvent.Dispatch();
        }

        public void OnReturnMainMenuClicked()
        {
            _returnToMainMenuEvent.Dispatch();
        }
    }
}