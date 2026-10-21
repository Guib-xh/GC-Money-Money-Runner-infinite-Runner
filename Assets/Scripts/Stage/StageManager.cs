using Boss;
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
        
        [SerializeField] private RSO_StageLengthRows _stageLengthRows;
        [SerializeField] private RSO_InStageDistanceRows inStageDistanceRows;
        [SerializeField] private RSO_CurrentStage _currentStage;
        [SerializeField] private RSO_Speed _speed;
        [SerializeField] private RSO_BossActive _bossActive;
        
        private void OnEnable()
        {
            _runStartedEvent.Event += Initialize;
            _continuRunEvent.Event += GoNextStage;
            inStageDistanceRows.OnChanged += UpdateStagePhase;
        }

        private void OnDisable()
        {
            _runStartedEvent.Event -= Initialize;
            _continuRunEvent.Event -= GoNextStage;
            inStageDistanceRows.OnChanged -= UpdateStagePhase;
        }

        private float _durationDecreasePerStage = 3f;

        private void Initialize()
        {
            _currentStage.Value = 1;
            StartStage();
        }

        private void StartStage()
        {
            Debug.Log("Start new Stage");
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
    }
}