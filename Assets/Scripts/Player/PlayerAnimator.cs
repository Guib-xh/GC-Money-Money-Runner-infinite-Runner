using System;
using Boss;
using Countdown;
using UnityEngine;

namespace Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private RSE_BossStarted _bossStartedEvent;
        [SerializeField] private RSE_CountdownFinished _countdownFinishedEvent;
        [SerializeField] private RSE_MoveLeft _moveLeftEvent;
        [SerializeField] private RSE_MoveRight _moveRightEvent;
        
        [SerializeField] private RSO_Invulnerable _invulnerable;
        
        [SerializeField] private Animator _animator;
        
        

        private void OnEnable()
        {
            _bossStartedEvent.Event += StartIdle;
            _countdownFinishedEvent.Event += StartSprint;
            _moveLeftEvent.Event += OnMoveLeft;
            _moveRightEvent.Event += OnMoveRight;
            _invulnerable.OnChanged += OnInvulnerable;
        }
        
        private void OnDisable()
        {
            _bossStartedEvent.Event -= StartIdle;
            _countdownFinishedEvent.Event -= StartSprint;
            _moveLeftEvent.Event -= OnMoveLeft;
            _moveRightEvent.Event -= OnMoveRight;
            _invulnerable.OnChanged -= OnInvulnerable;
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
            _animator.SetTrigger("MoveLeft");
        }
        
        private void OnMoveRight()
        {
            _animator.SetTrigger("MoveRight");
        }
    }
    
}