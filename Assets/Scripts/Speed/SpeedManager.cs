using Player;
using Stage;
using UnityEngine;

namespace Speed
{
    public class SpeedManager : MonoBehaviour
    {
        [SerializeField] private RSO_Speed _speed;
        [SerializeField] private RSO_Kv _kv;
        [SerializeField] private RSO_CurrentStage _currentStage;
        [SerializeField] private RSO_DistanceRowsInStage _distanceRowsInStage;
        [SerializeField] private RSO_StageLengthRows _stageLengthRows;
        
        [SerializeField] private RSE_RunStarted _runStarted;

        [SerializeField] private SSO_GameConfig _gameConfig;

        private float kvMax = 2f;
        
        public float KvBase(int stage) => Mathf.Min(1f + _gameConfig.KvGrowthPerStage * (stage - 1), kvMax);
        
        private void OnEnable()
        {
            _runStarted.Event += Initialize;
        }

        private void OnDisable()
        {
            _runStarted.Event -= Initialize;
        }

        private void Initialize()
        {
            _kv.Value = KvBase(1);
            _speed.Value = _kv.Value * _gameConfig.v0;
        }

        private void Update()
        {
            int stage = _currentStage.Value;
            float kvStart = KvBase(stage);
            float kvEnd = KvBase(stage + 1);
            float progressSpeed = _stageLengthRows.Value > 0f ?
                                Mathf.Clamp01(_distanceRowsInStage.Value / _stageLengthRows.Value) :
                                0f; 
            float target = Mathf.Lerp(kvStart, kvEnd, progressSpeed);
            _kv.Value = target;
            _speed.Value = _kv.Value * _gameConfig.v0;
            Debug.Log("_stageLengthRows :" + _stageLengthRows.Value);
            Debug.Log("_distanceRowsInStage :" + _distanceRowsInStage.Value);
            Debug.Log("speed :" + _speed.Value);
        }
    }
}
    
