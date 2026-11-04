using System.Collections;
using System.Collections.Generic;
using Health;
using Stage;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Boss
{
    public enum QTEKey { MoveLeft, MoveRight, Jump, Slide, UsePowerUp }
    public class BossManager : MonoBehaviour
    {
        //GAME CONFIG
        [SerializeField] private SSO_BossConfig _bossConfig;
        
        //DISPATCH
        [FormerlySerializedAs("_newQTEStequenceStartedEvent")] [SerializeField] private RSE_NewQTESequenceStarted newQteSequenceStartedEvent;
        [SerializeField] private RSE_QTEFailed _QTEFailedEvent;
        [SerializeField] private RSE_BossDefeated _BossDefeatedEvent;
        [SerializeField] private RSE_QTEStepAdvanced _QTEStepAdvancedEvent;
        
        //LISTEN
        [SerializeField] private InputActionReference _moveLeftInputActionReference;
        [SerializeField] private InputActionReference _moveRightInputActionReference;
        [SerializeField] private InputActionReference _jumpInputActionReference;
        [SerializeField] private InputActionReference _slideInputActionReference;
        [SerializeField] private InputActionReference _usePowerUpActionReference;
        
        [SerializeField] private RSE_RunStarted _runStartedEvent;
        [SerializeField] private RSE_BossStarted _bossStartedEvent;
        [SerializeField] private RSE_ContinuRun _continuRunEvent;
        [SerializeField] private RSE_BossUIIntroFinished _bossUIIntroFinishedEvent;
        
        //WRITE
        [SerializeField] private RSO_BossActive _bossActive;
        [SerializeField] private RSO_CurrentQTEStep _currentQTEStep;
        [SerializeField] private RSO_QTESequence _QTESequence;
        [SerializeField] private RSO_QTEKeyPressedWindowProgress _keyPressedWindowProgress;
        
        //READ
        [SerializeField] private RSO_Hearts _hearts;
        [SerializeField] private RSO_CurrentStage _currentStage;

        private QTEKey[] _keyOrder = { QTEKey.MoveLeft, QTEKey.MoveRight, QTEKey.Jump, QTEKey.Slide, QTEKey.UsePowerUp };
        private bool _capturing = false;

        private QTEStageConfig _qteStageConfig;
        
        private Coroutine _keyPressedWindowRoutine;

        private void OnEnable()
        {
            _moveLeftInputActionReference.action.performed += OnMoveLeftPressed;
            _moveRightInputActionReference.action.performed += OnMoveRightPressed;
            _jumpInputActionReference.action.performed += OnJumpPressed;
            _slideInputActionReference.action.performed += OnSlidePressed;
            _usePowerUpActionReference.action.performed += OnUseItemPressed;;

            _bossStartedEvent.Event += StartBoss;
            _continuRunEvent.Event += OnContinueRun;
            _runStartedEvent.Event += Initialize;
            _bossUIIntroFinishedEvent.Event += StartNewQTESequence;
        }

        private void OnDisable()
        {
            _moveLeftInputActionReference.action.performed -= OnMoveLeftPressed;
            _moveRightInputActionReference.action.performed -= OnMoveRightPressed;
            _jumpInputActionReference.action.performed -= OnJumpPressed;
            _slideInputActionReference.action.performed -= OnSlidePressed;
            _usePowerUpActionReference.action.performed += OnUseItemPressed;
            
            _bossStartedEvent.Event -= StartBoss;
            _bossUIIntroFinishedEvent.Event -= StartNewQTESequence;
            _continuRunEvent.Event -= OnContinueRun;
            _runStartedEvent.Event -= Initialize;
        }

        private void Initialize()
        {
            _bossActive.Value = false;
        }
        
        private void OnMoveLeftPressed(InputAction.CallbackContext _) => OnKeyPressed(QTEKey.MoveLeft);
        private void OnMoveRightPressed(InputAction.CallbackContext _) => OnKeyPressed(QTEKey.MoveRight);
        private void OnJumpPressed(InputAction.CallbackContext _) => OnKeyPressed(QTEKey.Jump);
        private void OnSlidePressed(InputAction.CallbackContext _) => OnKeyPressed(QTEKey.Slide);
        private void OnUseItemPressed(InputAction.CallbackContext _) => OnKeyPressed(QTEKey.UsePowerUp);

        private void OnKeyPressed(QTEKey keyPressed)
        {
            if (!_capturing) return; //Afin d'éviter que la touche pressé pendant le jeu ne soit tracké ici
            if (keyPressed != _QTESequence.Value[_currentQTEStep.Value])
            {
                Fail();
                return;
            }

            _currentQTEStep.Value++;
            if (_keyPressedWindowRoutine != null) StopCoroutine(_keyPressedWindowRoutine);

            if (_currentQTEStep.Value >= _QTESequence.Value.Count)
            {
                Success();
            } else
            {
                _QTEStepAdvancedEvent.Dispatch();
                _keyPressedWindowRoutine = StartCoroutine(KeyPressedWindowTimeout());
            }
        }
        
        private IEnumerator KeyPressedWindowTimeout()
        {
            float keyPressWindowDuration = _qteStageConfig.keyPressWindowDuration;
            float elapsedTime = 0;
            _keyPressedWindowProgress.Value = 1f;
            while (elapsedTime < keyPressWindowDuration && _capturing)
            {
                elapsedTime += Time.deltaTime;
                _keyPressedWindowProgress.Value = 1f - (elapsedTime / keyPressWindowDuration);
                yield return null;
            }
            if (_capturing) Fail();
        }

        private void StartBoss()
        {
            //Debug.Log("StartBoss");
            _bossActive.Value = true;
            _QTESequence.Value = new List<QTEKey>();
        }

        //Genere une nouvelle séquence de QTE
        private void StartNewQTESequence()
        {
            //Debug.Log("StartNewQTESequence");
             _qteStageConfig = GetQTEStageConfig();
            _QTESequence.Value.Clear();
            for (int i = 0; i < _qteStageConfig.sequenceLength; i++)
            {
                int keyPoolSize = Mathf.Min(_qteStageConfig.sequenceLength, _bossConfig.maxKeyPoolSize);
                _QTESequence.Value.Add(_keyOrder[Random.Range(0, keyPoolSize-1)]);
            }
            _currentQTEStep.Value = 0;
            newQteSequenceStartedEvent.Dispatch();
            _capturing = true;
            if (_keyPressedWindowRoutine != null) StopCoroutine(_keyPressedWindowRoutine);
            _keyPressedWindowRoutine = StartCoroutine(KeyPressedWindowTimeout());
        }

        private QTEStageConfig GetQTEStageConfig()
        {
            foreach (var qteStageConfig in _bossConfig.qteConfigs)
            {
                if (qteStageConfig.stage == _currentStage.Value) return qteStageConfig;
            }
            return _bossConfig.qteConfigs[_bossConfig.qteConfigs.Length - 1];
        }

        private void Fail()
        {
            _capturing = false;
            _keyPressedWindowProgress.Value = 0f;
            _QTEFailedEvent.Dispatch();
            if (_hearts.Value <= 0) return;
            StartCoroutine(PauseThenRetry());
        }
        
        private IEnumerator PauseThenRetry()
        {
            yield return new WaitForSeconds(_bossConfig.qteFailPauseSeconds);
            StartNewQTESequence();
        }
        
        private void Success()
        {
            _capturing = false;
            int reward = GetReward();
            _BossDefeatedEvent.Dispatch(reward);
        }

        //TODO rendre les valeurs paramétrables
        private int GetReward()
        {
            return Mathf.Min(5500, 500 + _currentStage.Value * 250);
        }

        private void OnContinueRun()
        {
            _bossActive.Value = false;
        }
        
        
    }
}