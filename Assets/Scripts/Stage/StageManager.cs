using Player;
using Speed;
using UnityEngine;
using UnityEngine.Serialization;

namespace Stage
{
    public class StageManager : MonoBehaviour
    {
        [SerializeField] private SSO_GameConfig _gameConfig;

        [SerializeField] private RSE_RunStarted _runStarted;

        [SerializeField] private RSO_StageLengthRows _stageLengthRows;
        [SerializeField] private RSO_InStageDistanceRows inStageDistanceRows;
        [SerializeField] private RSO_CurrentStage _currentStage;
        [SerializeField] private RSO_Speed _speed;

        private void OnEnable() => _runStarted.Event += Initialize;
        private void OnDisable() => _runStarted.Event -= Initialize;

        private float _durationDecreasePerStage = 3f;

        private void Initialize()
        {
            _currentStage.Value = 1;
            StartStage();
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
    }
}