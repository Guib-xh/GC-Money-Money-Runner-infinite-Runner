using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Boss
{
    public class BossDisplay : MonoBehaviour
    {
        //GAME CONFIG
        [SerializeField] private SSO_QTEKeyVisual _QTEKeyVisuals;
        
        //DISPATCH
        [SerializeField] private RSE_BossUIIntroFinished _bossUIIntroFinishedEvent;
                
        //LISTEN
        [SerializeField] private RSE_RunStarted _runStartedEvent;
        [SerializeField] private RSE_QTEFailed _QTEFailedEvent;
        [SerializeField] private RSE_BossDefeated _bossDefeatedEvent;
        [SerializeField] private RSE_BossStarted _bossStartedEvent;
        [SerializeField] private RSE_QTEStepAdvanced _QTEStepAdvancedEvent;
        [SerializeField] private RSE_NewQTEStequenceStarted _newQTEStequenceStartedEvent;
        
        //WRITE

        //READ
        [SerializeField] private RSO_QTESequence _QTESequence;
        [SerializeField] private RSO_CurrentQTEStep _currentQTEStep;
        [SerializeField] private RSO_QTEKeyPressedWindowProgress _keyPressedWindowProgress;

        //UI
        [SerializeField] private GameObject _bossCanvas;
        [SerializeField] private GameObject _interStageCanvas;
        [SerializeField] private TMP_Text _bossText;
        [SerializeField] private Image _progressBarFill;
        
        [SerializeField] private QTEKeySlot[] _QTEKeySlots;
        
        private float _readyDuration = 1f;
        private float _goDuration = 0.5f;

        private void OnEnable()
        {
            _bossStartedEvent.Event += PlayIntro;
            _bossDefeatedEvent.Event += OnBossDefeated;
            _QTEFailedEvent.Event += ShowFailStep;
            _QTEStepAdvancedEvent.Event += ShowSuccessStep;
            _runStartedEvent.Event += Initialize;
            _newQTEStequenceStartedEvent.Event += ShowSequence;

            _keyPressedWindowProgress.OnChanged += UpdateProgressBar;
        }

        private void OnDisable()
        {
            _bossStartedEvent.Event -= PlayIntro;
            _bossDefeatedEvent.Event -= OnBossDefeated;
            _QTEFailedEvent.Event -= ShowFailStep;
            _QTEStepAdvancedEvent.Event -= ShowSuccessStep;
            _runStartedEvent.Event -= Initialize;
            _newQTEStequenceStartedEvent.Event -= ShowSequence;
        
            _keyPressedWindowProgress.OnChanged -= UpdateProgressBar;
        }

        private void Initialize()
        {
            _bossCanvas.SetActive(false);
            _interStageCanvas.SetActive(false);
            _bossText.gameObject.SetActive(false);
            _progressBarFill.gameObject.SetActive(false);
        }

        private void PlayIntro()
        {
            //Debug.Log("PlayIntro");
            _bossCanvas.SetActive(true);
            HideAllSlots();
            StartCoroutine(StartBossCoroutine());
        }

        private void ShowSequence()
        {
            //Debug.Log("ShowSequence");
            _bossText.gameObject.SetActive(false);
            _progressBarFill.gameObject.SetActive(true);
            
            List<QTEKey> sequence = _QTESequence.Value;
            for (int i = 0; i < sequence.Count && i < _QTEKeySlots.Length; i++) 
            {
                _QTEKeySlots[i].gameObject.SetActive(true);
                _QTEKeySlots[i].Setup(_QTEKeyVisuals.GetSprite(sequence[i]));
                
            }
            _QTEKeySlots[_currentQTEStep.Value].SetCurrent();
        }
        
        private IEnumerator StartBossCoroutine()
        {
            _bossText.color = Color.white;
            _bossText.gameObject.SetActive(true);
            _bossText.text = "Prêt ?";
            yield return new WaitForSeconds(_readyDuration);
            _bossText.text = "Go !";
            yield return new WaitForSeconds(_goDuration);
            _bossUIIntroFinishedEvent.Dispatch();
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
            _bossText.text = "GOOD !";
            _bossText.color = Color.green;
            _bossText.gameObject.SetActive(true); 
        }
        
        private void ShowFailStep()
        {
            _QTEKeySlots[_currentQTEStep.Value].SetResult(false);
            _bossText.text = "FAILED";
            _bossText.color = Color.red;
            _bossText.gameObject.SetActive(true);
        }
        
        private void OnBossDefeated(int reward){
            HideAllSlots();
            _bossText.gameObject.SetActive(false);
            _bossCanvas.SetActive(false);
            _progressBarFill.gameObject.SetActive(false);

            _interStageCanvas.SetActive(true);
        }

        private void UpdateProgressBar(float value)
        {
            //Debug.Log("progress value: " + value);
            _progressBarFill.fillAmount = Mathf.Clamp01(value);
        }
    }
}