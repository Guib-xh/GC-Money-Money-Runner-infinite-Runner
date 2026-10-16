using Player;
using UnityEngine;

namespace Score
{
    public class MoneyManager : MonoBehaviour
    {
        [SerializeField] private RSE_LootCollected _lootCollectedEvent;
        [SerializeField] private RSE_RunStarted _runStartedEvent;
        [SerializeField] private RSO_MoneyInRun _moneyInRun;
        
        
        private void OnEnable()
        {
            _runStartedEvent.Event += Initialize;
            _lootCollectedEvent.Event += OnLootcollected;
        }
        
        private void OnDisable()
        {
            _runStartedEvent.Event -= Initialize;
            _lootCollectedEvent.Event -= OnLootcollected;
        }

        private void Initialize()
        {
            _moneyInRun.Value = 0;
        }

        private void OnLootcollected(int value)
        {
            _moneyInRun.Value += value;
        }
    }
}