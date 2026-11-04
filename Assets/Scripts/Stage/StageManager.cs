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
        [SerializeField] private SSO_StagePhase _stagePhase;

        //DISPATCH
        [SerializeField] private RSE_BossStarted _bossStartedEvent;
        [SerializeField] private RSE_RequestCoundown _requestCoundownEvent;
        [SerializeField] private RSE_StageStarted _stageStartedEvent;
        
        //LISTEN
        [SerializeField] private RSE_RunStarted _runStartedEvent;
        [SerializeField] private RSE_ContinuRun _continuRunEvent;
        [SerializeField] private RSE_CountdownFinished _countdownFinishedEvent;

        //READ
        [SerializeField] private RSO_StageLengthRows _stageLengthRows;
        [SerializeField] private RSO_Speed _speed;
        [SerializeField] private RSO_IsInputBlocked _isInputBlocked;
        [SerializeField] private RSO_InStageDistanceRows _inStageDistanceRows;
        [SerializeField] private RSO_TotalDistanceRows _totalDistanceRows;
        
        //WRITE
        [SerializeField] private RSO_CurrentStage _currentStage;
        [SerializeField] private RSO_CurrentStagePhase _currentStagePhase;
        

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
            _currentStagePhase.Value = _stagePhase.Phases[0];
            _inStageDistanceRows.Value = 0f;
            _currentTotalDistanceRows = 0;
            _stageStartedEvent.Dispatch();
            StartCountdownBeforeStage();
        }

        private void StartStage()
        {
            _currentStage.Value++;
            _inStageDistanceRows.Value = 0;
            int n = _currentStage.Value;
            float duration = Mathf.Max(
                _gameConfig.minStageDurationSeconds,
                _gameConfig.firstStageDuration - _durationDecreasePerStage * (n - 1));
            //Debug.Log($"Stage {n}: {duration} seconds");
            _stageLengthRows.Value = _speed.Value * duration / _gameConfig.rowDepthR;
            //Debug.Log($"Stage {n}: {_speed.Value} m/s");
            //Debug.Log($"Stage {n}: {_stageLengthRows.Value} m");
            
        }

        private void UpdateStagePhase(float value)
        {
            if (_isInputBlocked.Value || value == 0) return;
            _inStageDistanceRows.Value += value - _currentTotalDistanceRows;
            _currentTotalDistanceRows = value;
            
            
            float fraction = _inStageDistanceRows.Value / _stageLengthRows.Value;
            Phase current = _stagePhase.Phases[0];
            foreach (Phase phase in _stagePhase.Phases)
            {
                if (fraction >= phase.startThreshold)
                {
                    current = phase; 
                }
            }
            _currentStagePhase.Value = current;
            
            if (fraction >= 1f)
            {
                _bossStartedEvent.Dispatch();
            }
        }

        private void StartCountdownBeforeStage()
        {
            _requestCoundownEvent.Dispatch(3);
            _inStageDistanceRows.Value = 0f;
        }
    }
}