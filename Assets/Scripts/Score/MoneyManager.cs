using Loot;
using Player;
using UnityEngine;

namespace Score
{
    public class MoneyManager : MonoBehaviour
    {
        [SerializeField] private RSE_LootCollected _lootCollectedEvent;
        [SerializeField] private RSE_RunStarted _runStartedEvent;
        [SerializeField] private RSE_ActivateDiamondRequested _activateDiamondRequestedEvent;
        [SerializeField] private RSE_DesactivateDiamondRequested _desactivateDiamondRequestedEvent;
        
        [SerializeField] private RSO_MoneyInRun _moneyInRun;
        [SerializeField] private RSO_DiamondActive _diamondActive;
        
        
        private void OnEnable()
        {
            _runStartedEvent.Event += Initialize;
            _lootCollectedEvent.Event += OnLootcollected;
            _activateDiamondRequestedEvent.Event += ActivateDiamond;
            _desactivateDiamondRequestedEvent.Event += DesactivateDiamond;
        }
        
        private void OnDisable()
        {
            _runStartedEvent.Event -= Initialize;
            _lootCollectedEvent.Event -= OnLootcollected;
            _activateDiamondRequestedEvent.Event -= ActivateDiamond;
            _desactivateDiamondRequestedEvent.Event -= DesactivateDiamond;
        }

        private void Initialize()
        {
            _moneyInRun.Value = 0;
            _diamondActive.Value = false;
        }

        private void OnLootcollected(SSO_LootItem lootItem)
        {
            _moneyInRun.Value += _diamondActive.Value ? lootItem.Value * 2 : lootItem.Value;
        }

        private void ActivateDiamond()
        {
            _diamondActive.Value = true;
        }
        private void DesactivateDiamond()
        {
            _diamondActive.Value = false;
        }
    }
}