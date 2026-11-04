using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace PowerUp
{
    public class ShieldAnimator : MonoBehaviour
    {
        //GAME CONFIG
        
        //DISPATCH
        
        //LISTEN
        [SerializeField] private RSE_ActivateShieldRequested _activateShieldRequestedEvent;
        [SerializeField] private RSE_DesactivateShieldRequested _desactivateShieldRequestedEvent;
        [SerializeField] private RSE_ShieldConsumed _shieldConsumedEvent;
        
        //WRITE
        
        //READ
        
        [SerializeField] private Animator _animator;


        private void OnEnable()
        {
            _activateShieldRequestedEvent.Event += ShowShield;
            _desactivateShieldRequestedEvent.Event += hideShield;
            _shieldConsumedEvent.Event += hideShield;
        }
        
        private void OnDisable()
        {
            _activateShieldRequestedEvent.Event -= ShowShield;
            _desactivateShieldRequestedEvent.Event -= hideShield;
            _shieldConsumedEvent.Event -= hideShield;
        }

        private void ShowShield()
        {
            _animator.SetBool("Active",true);
        }

        private void hideShield()
        {
            _animator.SetBool("Active",false);

        }
    }
}