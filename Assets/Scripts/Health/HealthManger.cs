using System;
using System.Collections;
using Player;
using Stage;
using UnityEngine;

namespace Health
{
    public class HealthManger : MonoBehaviour
    {
        [SerializeField] private RSE_ObstacleCollided _obstacleCollidedEvent;
        [SerializeField] private RSE_RunStarted _runStartedEvent;

        [SerializeField] private RSE_ActivateCroissantRequested _activateCroissantRequestedEvent;
        [SerializeField] private RSE_ActivateShieldRequested _activateShieldRequestedEvent;
        [SerializeField] private RSE_DesactivateShieldRequested _desactivateShieldRequestedEvent;
        [SerializeField] private RSE_ShieldConsumned _shieldConsumnedEvent;
        
        [SerializeField] private RSO_Hearts _hearts;
        [SerializeField] private RSO_MaxHearts _maxHearts;
        [SerializeField] private RSO_Invulnerable _invulnerable;
        [SerializeField] private RSO_ShieldActive _shieldActive;
        
        [SerializeField] private SSO_GameConfig _gameConfig;
        
        private void OnEnable()
        {
            _runStartedEvent.Event += Initialize;
            _obstacleCollidedEvent.Event += OnCollision;
            _activateCroissantRequestedEvent.Event += AddHearts;
            _activateShieldRequestedEvent.Event += ActivateShield;
            _desactivateShieldRequestedEvent.Event += DesactivateShield;
        }        
        
        private void OnDisable()
        {
            _runStartedEvent.Event -= Initialize;
            _obstacleCollidedEvent.Event -= OnCollision;
            _activateCroissantRequestedEvent.Event -= AddHearts;
            _activateShieldRequestedEvent.Event -= ActivateShield;
            _desactivateShieldRequestedEvent.Event -= DesactivateShield;
        }

        private void Initialize()
        {
            //maxHearts pourra servir pour augmenter les coeurs max par l'épicerie
            _maxHearts.Value = _gameConfig.startingHearts; 
            _hearts.Value = _maxHearts.Value; 
            _invulnerable.Value = false;
            _shieldActive.Value = false;
        }

        private void OnCollision()
        {
            //Debug.Log("OnCollision");
            //Debug.Log("OnCollision - _shieldActive.Value : " + _shieldActive.Value);
            if (_invulnerable.Value) return;

            if (_shieldActive.Value)
            {
                _shieldConsumnedEvent.Dispatch();
                return;
            }
            _hearts.Value = Mathf.Max(0, _hearts.Value - 1);
            
            StartCoroutine(InvulnerabilityCoroutine(_gameConfig.postCollisionInvulnerabilitySeconds));
        }
        
        private IEnumerator InvulnerabilityCoroutine(float seconds)
        {
            _invulnerable.Value = true;
            yield return new WaitForSeconds(seconds);
            _invulnerable.Value = false;
        }
        
        private void AddHearts()
        {
            _hearts.Value = Mathf.Min(_maxHearts.Value, _hearts.Value + 1);
        }

        private void ActivateShield()
        {
            //Debug.Log("Active Shield");
            _shieldActive.Value = true;
        }
        
        private void DesactivateShield()
        {
            //Debug.Log("Desactivate Shield");
            _shieldActive.Value = false;
        }
    }
}