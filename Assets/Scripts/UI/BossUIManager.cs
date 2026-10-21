using System.Collections.Generic;
using Boss;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BossUIManager : MonoBehaviour
    {
        [SerializeField] private SSO_QTEKeyVisual _QTEKeyVisuals;
        
        [SerializeField] private RSE_RunStarted _runStartedEvent;
        [SerializeField] private RSE_QTEFailed _QTEFailedEvent;
        [SerializeField] private RSE_BossDefeated _bossDefeatedEvent;
        [SerializeField] private RSE_BossStarted _bossStartedEvent;
        [SerializeField] private RSE_QTEStepAdvanced _QTEStepAdvancedEvent;
        [SerializeField] private RSE_NewQTEStequenceStarted _newQTEStequenceStartedEvent;
        
        [SerializeField] private RSO_QTESequence _QTESequence;
        [SerializeField] private RSO_CurrentQTEStep _currentQTEStep;
        [SerializeField] private RSO_QTEKeyPressedWindowProgress _keyPressedWindowProgress;
        
        [SerializeField] private GameObject _bossPanel;
        [SerializeField] private GameObject _interStagePanel;
        [SerializeField] private TMP_Text _resultText;
        [SerializeField] private Image _progressBarFill;
        
        [SerializeField] private QTEKeySlot[] _QTEKeySlots;
        

        private void OnEnable()
        {
            _bossStartedEvent.Event += OnBossQTEStarted;
            _bossDefeatedEvent.Event += OnBossDefeated;
            _QTEFailedEvent.Event += ShowFailStep;
            _QTEStepAdvancedEvent.Event += ShowSuccessStep;
            _runStartedEvent.Event += Initialize;
            _newQTEStequenceStartedEvent.Event += ShowSequence;

            _keyPressedWindowProgress.OnChanged += UpdateProgressBar;
        }

        private void OnDisable()
        {
            _bossStartedEvent.Event -= OnBossQTEStarted;
            _bossDefeatedEvent.Event -= OnBossDefeated;
            _QTEFailedEvent.Event -= ShowFailStep;
            _QTEStepAdvancedEvent.Event -= ShowSuccessStep;
            _runStartedEvent.Event -= Initialize;
            _newQTEStequenceStartedEvent.Event -= ShowSequence;
        
            _keyPressedWindowProgress.OnChanged -= UpdateProgressBar;
        }

        private void Initialize()
        {
            _bossPanel.SetActive(false);
            _interStagePanel.SetActive(false);
            _resultText.gameObject.SetActive(false);
        }

        private void OnBossQTEStarted()
        {
            _bossPanel.SetActive(true);
        }

        private void ShowSequence()
        {
            HideAllSlots();
            _resultText.gameObject.SetActive(false);
            
            List<QTEKey> sequence = _QTESequence.Value;
            for (int i = 0; i < sequence.Count && i < _QTEKeySlots.Length; i++) 
            {
                _QTEKeySlots[i].gameObject.SetActive(true);
                _QTEKeySlots[i].Setup(_QTEKeyVisuals.GetSprite(sequence[i]));
                
            }
            _QTEKeySlots[_currentQTEStep.Value].SetCurrent();
        }

        private void HideAllSlots()
        {
            foreach (var slot in _QTEKeySlots)
            {
                slot.gameObject.SetActive(false);
            }
        }

        private void ShowSuccessStep()
        {
            _QTEKeySlots[_currentQTEStep.Value-1].SetResult(true);
            _QTEKeySlots[_currentQTEStep.Value].SetCurrent();
            _resultText.text = "GOOD !";
            _resultText.color = Color.green;
            _resultText.gameObject.SetActive(true); 
        }
        
        private void ShowFailStep()
        {
            _QTEKeySlots[_currentQTEStep.Value].SetResult(false);
            _resultText.text = "FAILED";
            _resultText.color = Color.red;
            _resultText.gameObject.SetActive(true);
        }
        
        private void OnBossDefeated(int reward){
            HideAllSlots();
            _resultText.gameObject.SetActive(false);
            _bossPanel.SetActive(false);

            _interStagePanel.SetActive(true);
        }

        private void UpdateProgressBar(float value)
        {
            Debug.Log("progress value: " + value);
            _progressBarFill.fillAmount = Mathf.Clamp01(value);
        }
    }
}