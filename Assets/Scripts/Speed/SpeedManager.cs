using Player;
using Stage;
using UnityEngine;
using UnityEngine.Serialization;

namespace Speed
{
    public class SpeedManager : MonoBehaviour
    {
        //GAME CONFIG
        [SerializeField] private SSO_GameConfig _gameConfig;
        
        //DISPATCH
        
        //LISTEN
        [SerializeField] private RSE_RunStarted _runStarted;
        [SerializeField] private RSE_ActivateCoffeeRequested _activateCoffeeRequestedEvent;
        [SerializeField] private RSE_DesactivateCoffeeRequested _desactivateCoffeeRequestedEvent;
        
        //WRITE
        [SerializeField] private RSO_Speed _speed;
        [SerializeField] private RSO_Kv _kv;
        
        //READ
        [SerializeField] private RSO_CurrentStage _currentStage;
        [SerializeField] private RSO_InStageDistanceRows _inStageDistanceRows;
        [SerializeField] private RSO_StageLengthRows _stageLengthRows;
        
        private float _slowSpeedMultiplier;

        
        public float KvBase(int stage) => Mathf.Min(1f + _gameConfig.kvGrowthPerStage * (stage - 1), _gameConfig.kvMax);
        
        private void OnEnable()
        {
            _runStarted.Event += Initialize;
            _activateCoffeeRequestedEvent.Event += SlowSpeed;
            _desactivateCoffeeRequestedEvent.Event += RestoreSpeed;
        }

        private void OnDisable()
        {
            _runStarted.Event -= Initialize;
            _activateCoffeeRequestedEvent.Event -= SlowSpeed;
            _desactivateCoffeeRequestedEvent.Event -= RestoreSpeed;
        }

        private void Initialize()
        {
            _slowSpeedMultiplier = 1f;
            _kv.Value = KvBase(1);
            ComputeSpeed();
        }

        private void Update()
        {
            if (_currentStage.Value == 0) return;
            int stage = _currentStage.Value;
            float kvStart = KvBase(stage);
            float kvEnd = KvBase(stage + 1);
            float progressSpeed = _stageLengthRows.Value > 0f ?
                                Mathf.Clamp01(_inStageDistanceRows.Value / _stageLengthRows.Value) :
                                0f; 
            float target = Mathf.Lerp(kvStart, kvEnd, progressSpeed);
            _kv.Value = target;
            ComputeSpeed();
            //Debug.Log("_stageLengthRows :" + _stageLengthRows.Value);
            //Debug.Log("_distanceRowsInStage :" + _inStageDistanceRows.Value);
            //Debug.Log("progressSpeed :" + progressSpeed);
            //Debug.Log("speed :" + _speed.Value);
        }

        private void ComputeSpeed()
        {
            _speed.Value = _kv.Value * _gameConfig.v0 * _slowSpeedMultiplier;
            
        }

        private void SlowSpeed()
        {
            _slowSpeedMultiplier = 0.7f;
        }
        
        private void RestoreSpeed()
        {
            _slowSpeedMultiplier = 1f;
        }
        
    }
}
    
