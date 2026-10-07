using System;
using Player;
using Stage;
using UnityEngine;

namespace Health
{
    public class HealthManger : MonoBehaviour
    {
        [SerializeField] private RSE_ObstacleCollided _obstacleCollidedEvent;
        [SerializeField] private RSE_RunStarted _runStartedEvent;
        
        [SerializeField] private RSO_Hearts _hearts;
        [SerializeField] private RSO_MaxHearts _maxHearts;
        
        [SerializeField] private SSO_GameConfig _gameConfig;
        


        private void OnEnable()
        {
            _runStartedEvent.Event += Initialize;
            _obstacleCollidedEvent.Event += ReduceHealth;
        }        
        
        private void OnDisable()
        {
            _runStartedEvent.Event -= Initialize;
            _obstacleCollidedEvent.Event -= ReduceHealth;
        }

        private void Initialize()
        {
            //maxHearts pourra servir pour augmenter les coeurs max par l'épicerie
            _maxHearts.Value = _gameConfig.startingHearts; 
            _hearts.Value = _maxHearts.Value; 
        }

        private void ReduceHealth()
        {
            _hearts.Value = Mathf.Max(0, _hearts.Value - 1);
        }
    }
}