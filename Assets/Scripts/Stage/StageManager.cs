using Boss;
using Countdown;
using Health;
using JetBrains.Annotations;
using Player;
using Speed;
using UnityEngine;
using UnityEngine.Serialization;

namespace Stage
{
    public class StageManager : MonoBehaviour
    {
        // GAME CONFIG
        [SerializeField] private SSO_GameConfig _gameConfig;

        //DISPATCH
        [SerializeField] private RSE_BossStarted _bossStartedEvent;
        [SerializeField] private RSE_RequestCoundown _requestCoundownEvent;
        
        //LISTEN
        [SerializeField] private RSE_RunStarted _runStartedEvent;
        [SerializeField] private RSE_ContinuRun _continuRunEvent;
        [SerializeField] private RSE_CountdownFinished _countdownFinishedEvent;

        //READ
        [SerializeField] private RSO_StageLengthRows _stageLengthRows;
        [SerializeField] private RSO_Speed _speed;
        [SerializeField] private RSO_BossActive _bossActive;
        [SerializeField] private RSO_InStageDistanceRows _inStageDistanceRows;
        [SerializeField] private RSO_TotalDistanceRows _totalDistanceRows;
        
        //WRITE
        [SerializeField] private RSO_CurrentStage _currentStage;

        private float _currentTotalDistanceRows;
        
        private void OnEnable()
        {
            _runStartedEvent.Event += Initialize;
            _continuRunEvent.Event += StartCountdownBeforeStage;
            _countdownFinishedEvent.Event += StartStage;

            _totalDistanceRows.OnChanged += UpdateStagePhase;
        }

        private void OnDisable()
        {
            _runStartedEvent.Event -= Initialize;
            _continuRunEvent.Event -= StartCountdownBeforeStage;
            _countdownFinishedEvent.Event -= StartStage;
            
            _totalDistanceRows.OnChanged -= UpdateStagePhase;
            
        }

        private float _durationDecreasePerStage = 3f;

        private void Initialize()
        {
            _currentStage.Value = 0; //On init à 0 pour passer à 1 des le premier startStage
            _inStageDistanceRows.Value = 0f;
            _currentTotalDistanceRows = 0;
            StartCountdownBeforeStage();
        }

        private void StartStage()
        {
            _currentStage.Value++;
            int n = _currentStage.Value;
            float duration = Mathf.Max(
                _gameConfig.minStageDurationSeconds,
                _gameConfig.firstStageDuration - _durationDecreasePerStage * (n - 1));

            _stageLengthRows.Value = _speed.Value * duration / _gameConfig.rowDepthR;
        }

        private void UpdateStagePhase(float value)
        {
            if (_bossActive.Value || value == 0) return;
            _inStageDistanceRows.Value += value - _currentTotalDistanceRows;
            _currentTotalDistanceRows = value;
            if (_inStageDistanceRows.Value >= _stageLengthRows.Value)
            {
                _bossStartedEvent.Dispatch();
            }
        }

        private void StartCountdownBeforeStage()
        {
            _requestCoundownEvent.Dispatch(3);
        }
    }
}