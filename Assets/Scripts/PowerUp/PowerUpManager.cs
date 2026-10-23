using System.Collections.Generic;
using Loot;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PowerUp
{

    public class PowerUpManager : MonoBehaviour
    {
        //GAME CONFIG 
        [SerializeField] private List<SSO_PowerUp> _availablePowerUps;
        
        //DISPATCH
        
        //LISTEN
        [SerializeField] private RSE_RunStarted _runStartedEvent;
        [SerializeField] private RSE_LootCollected _lootCollectedEvent;
        [SerializeField] private RSE_UsePowerUpRequested _usePowerUpRequestedEvent;
        [SerializeField] private RSE_ShieldConsumned _shieldConsumnedEvent;
        
        //WRITE
        [SerializeField] private RSO_HeldPowerUp _heldPowerUp;
        [SerializeField] private RSO_ActivePowerUp _activePowerUp;
        
        //READ
        [SerializeField] private RSO_TotalDistanceRows _totalDistanceRows;
        
        
        private float _startDistance;

        private void OnEnable()
        {
            _lootCollectedEvent.Event += OnLootCollected;
            _usePowerUpRequestedEvent.Event += ActivatePowerUp;
            _totalDistanceRows.OnChanged += OnDistanceChanged;
            _runStartedEvent.Event += Initialize;
            _shieldConsumnedEvent.Event += DesactivateCurrent;
        }

        private void OnDisable()
        {
            _lootCollectedEvent.Event -= OnLootCollected;
            _usePowerUpRequestedEvent.Event -= ActivatePowerUp;
            _totalDistanceRows.OnChanged -= OnDistanceChanged;
            _runStartedEvent.Event -= Initialize;
            _shieldConsumnedEvent.Event -= DesactivateCurrent;
        }

        private void Initialize()
        {
            DesactivateCurrent();
            _heldPowerUp.Value = null;
        }

        private void OnDistanceChanged(float distance)
        {
            SSO_PowerUp current = _activePowerUp.Value;
            if (current is null) return;
            float traveledDistance = distance - _startDistance;

            if (traveledDistance >= current.DurationRows) DesactivateCurrent();
        }

        private void OnLootCollected(SSO_LootItem loot)
        {
            if (!loot.IsLootBox) return;
            _heldPowerUp.Value = GetRandomPowerUp();
        }

        private SSO_PowerUp GetRandomPowerUp()
        {
            int total = 0;
            foreach (var pu in _availablePowerUps) total += pu.SpawnProbability;
            int roll = Random.Range(0, total);
            int acc = 0;
            foreach (var pu in _availablePowerUps)
            {
                acc += pu.SpawnProbability;
                if (roll <= acc) return pu;
            }

            return _availablePowerUps[_availablePowerUps.Count - 1];
        }

        private void ActivatePowerUp()
        {
            if (_heldPowerUp.Value is null) return;
            

            if (_heldPowerUp.Value.IsInstant)
            {
                _heldPowerUp.Value.Activate();
                _heldPowerUp.Value  = null;
                return;
            }
            DesactivateCurrent();
            
            _activePowerUp.Value = _heldPowerUp.Value;
            _heldPowerUp.Value  = null;
            
            _startDistance = _totalDistanceRows.Value;
            _activePowerUp.Value.Activate();
        }

        private void DesactivateCurrent()
        {
            if (_activePowerUp.Value is null) return;
            _activePowerUp.Value.Desactivate();
            _activePowerUp.Value = null;
        }
    }
}