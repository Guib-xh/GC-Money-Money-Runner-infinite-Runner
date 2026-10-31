using System;
using System.Collections;
using Boss;
using Countdown;
using UnityEngine;

namespace Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private RSE_BossStarted _bossStartedEvent;
        [SerializeField] private RSE_RunStarted _runStartedEvent;
        [SerializeField] private RSE_CountdownFinished _countdownFinishedEvent;
        [SerializeField] private RSE_MoveLeft _moveLeftEvent;
        [SerializeField] private RSE_MoveRight _moveRightEvent;
        [SerializeField] private RSE_PlayerDied _playerDiedEvent;
        [SerializeField] private RSE_PlayerDieAnimationFinished _playerDieAnimationFinishedEvent;
        
        
        [SerializeField] private RSO_Invulnerable _invulnerable;
        
        [SerializeField] private Animator _animator;
        
        

        private void OnEnable()
        {
            _bossStartedEvent.Event += StartIdle;
            _runStartedEvent.Event += StartIdle;
            _countdownFinishedEvent.Event += StartSprint;
            _moveLeftEvent.Event += OnMoveLeft;
            _moveRightEvent.Event += OnMoveRight;
            _playerDiedEvent.Event += OnPlayerDied;
            _invulnerable.OnChanged += OnInvulnerable;
        }
        
        private void OnDisable()
        {
            _bossStartedEvent.Event -= StartIdle;
            _runStartedEvent.Event -= StartIdle;
            _countdownFinishedEvent.Event -= StartSprint;
            _moveLeftEvent.Event -= OnMoveLeft;
            _moveRightEvent.Event -= OnMoveRight;
            _playerDiedEvent.Event -= OnPlayerDied;
            _invulnerable.OnChanged -= OnInvulnerable;
            

            _animator.SetBool("Sprint", false);
        }

        private void StartIdle()
        {
            _animator.SetBool("Sprint", false);
            
        }
        
        private void StartSprint()
        {
            _animator.SetBool("Sprint", true);
        }

        private void OnInvulnerable(bool invulnerable)
        {
            _animator.SetBool("Invulnerable", invulnerable);
        }
        
        private void OnMoveLeft()
        {
            Debug.unityLogger.Log("MoveLeft");
            _animator.SetTrigger("MoveLeft");
        }
        
        private void OnMoveRight()
        {
            Debug.unityLogger.Log("MoveRight");
            _animator.SetTrigger("MoveRight");
        }
        
        private void OnPlayerDied()
        {
            _animator.SetTrigger("Die");
        }
        
        public void OnDeathAnimationEnd()
        {
            StartCoroutine(GameOverCoroutine());
        }

        private IEnumerator GameOverCoroutine()
        {
            yield return new WaitForSeconds(1f);
            
            _animator.ResetTrigger("Die");
            _animator.SetTrigger("Revive");
            yield return new WaitForSeconds(0.01f); //TODO faire plus propre 
            
            _playerDieAnimationFinishedEvent.Dispatch();
        }
    }
    
}