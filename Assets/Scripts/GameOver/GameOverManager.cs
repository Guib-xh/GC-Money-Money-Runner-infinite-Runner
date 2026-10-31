using System;
using System.Collections;
using Health;
using UnityEngine;

namespace GameOver
{
    public class GameOverManager : MonoBehaviour
    {
        //DISPATCH
        [SerializeField] private RSE_GameOver _gameOverEvent;
        
        //LISTEN
        [SerializeField] private RSE_PlayerDieAnimationFinished _playerDieAnimationFinishedEvent;
        
        //WRITE

        //READ
        [SerializeField] private RSO_Hearts _hearts;

        private void OnEnable()
        {
            _playerDieAnimationFinishedEvent.Event += OnPlayerDieAnimationFinished;
        }

        private void OnDisable()
        {
            _playerDieAnimationFinishedEvent.Event -= OnPlayerDieAnimationFinished;
        }

        private void OnPlayerDieAnimationFinished()
        {
            _gameOverEvent.Dispatch();
        }
        
        
        
    }
}