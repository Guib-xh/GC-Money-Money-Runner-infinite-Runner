using System;
using Health;
using UnityEngine;

namespace GameOver
{
    public class GameOverManager : MonoBehaviour
    {
        //DISPATCH
        [SerializeField] private RSE_GameOver _gameOverEvent;
        
        //LISTEN
        
        //WRITE

        //READ
        [SerializeField] private RSO_Hearts _hearts;

        private void OnEnable()
        {
            _hearts.OnChanged += OnHeartsEmpty;
        }

        private void OnDisable()
        {
            _hearts.OnChanged -= OnHeartsEmpty;
        }

        private void OnHeartsEmpty(int hearts)
        {
            if (hearts != 0) return;
            _gameOverEvent.Dispatch();
        }
        
        
    }
}