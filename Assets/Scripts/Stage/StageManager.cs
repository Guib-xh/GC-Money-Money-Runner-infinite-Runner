using Boss;
using Countdown;
using Player;
using Speed;
using UnityEngine;
using UnityEngine.Serialization;

namespace Stage
{
    public class StageManager : MonoBehaviour
    {
        [SerializeField] private SSO_GameConfig _gameConfig;

        [SerializeField] private RSE_RunStarted _runStartedEvent;
        [SerializeField] private RSE_BossStarted _bossStartedEvent;
        [SerializeField] private RSE_ContinuRun _continuRunEvent;
        [SerializeField] private RSE_CountdownFinished _countdownFinishedEvent;
        [SerializeField] private RSE_RequestCoundown _requestCoundownEvent;
        
        [SerializeField] private RSO_StageLengthRows _stageLengthRows;
        [SerializeField] private RSO_InStageDistanceRows inStageDistanceRows;
        [SerializeField] private RSO_CurrentStage _currentStage;
        [SerializeField] private RSO_Speed _speed;
        [SerializeField] private RSO_BossActive _bossActive;
        
        private void OnEnable()
        {
            _runStartedEvent.Event += Initialize;
            _continuRunEvent.Event += StartCountdownBeforeStage;
            inStageDistanceRows.OnChanged += UpdateStagePhase;
            _countdownFinishedEvent.Event += StartStage;
        }

        private void OnDisable()
        {
            _runStartedEvent.Event -= Initialize;
            _continuRunEvent.Event -= StartCountdownBeforeStage;
            inStageDistanceRows.OnChanged -= UpdateStagePhase;
            _countdownFinishedEvent.Event -= StartStage;
            
        }

        private float _durationDecreasePerStage = 3f;

        private void Initialize()
        {
            _currentStage.Value = 1;
            StartCountdownBeforeStage();
        }

        private void StartStage()
        {
            int n = _currentStage.Value;
            float duration = Mathf.Max(
                _gameConfig.minStageDurationSeconds,
                _gameConfig.firstStageDuration - _durationDecreasePerStage * (n - 1));

            _stageLengthRows.Value = _speed.Value * duration / _gameConfig.rowDepthR;
            inStageDistanceRows.Value = 0f;
        }

        private void UpdateStagePhase(float _)
        {
            if (_bossActive.Value) return;
            if (inStageDistanceRows.Value >= _stageLengthRows.Value)
            {
                _bossStartedEvent.Dispatch();
            }
        }

        private void GoNextStage()
        {
            Debug.Log("Go next stage");
            _currentStage.Value++;
            StartStage();
        }

        private void StartCountdownBeforeStage()
        {
            _requestCoundownEvent.Dispatch(3);
        }
    }
}