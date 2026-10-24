using Boss;
using Loot;
using Player;
using UnityEngine;

namespace Money
{
    public class MoneyManager : MonoBehaviour
    {
        //DISPATCH
        
        //LISTEN
        [SerializeField] private RSE_LootCollected _lootCollectedEvent;
        [SerializeField] private RSE_RunStarted _runStartedEvent;
        [SerializeField] private RSE_GameOver _gameOverEvent;
        [SerializeField] private RSE_BossDefeated _bossDefeatedEvent;
        [SerializeField] private RSE_ReturnToMainMenu _returnToMainMenuEvent;
        [SerializeField] private RSE_ActivateDiamondRequested _activateDiamondRequestedEvent;
        [SerializeField] private RSE_DesactivateDiamondRequested _desactivateDiamondRequestedEvent;
        
        //WRITE
        [SerializeField] private RSO_MoneyInRun _moneyInRun;
        [SerializeField] private RSO_MoneyInStage _moneyInStage;
        [SerializeField] private RSO_MoneyChest _moneyChest;
        [SerializeField] private RSO_DiamondActive _diamondActive;
        
        //READ

        private void OnEnable()
        {
            _runStartedEvent.Event += Initialize;
            _gameOverEvent.Event += OnEndRun;
            _returnToMainMenuEvent.Event += OnEndRun;
            _bossDefeatedEvent.Event += StoreMoney;
            _lootCollectedEvent.Event += OnLootcollected;
            _activateDiamondRequestedEvent.Event += ActivateDiamond;
            _desactivateDiamondRequestedEvent.Event += DesactivateDiamond;
        }
        
        private void OnDisable()
        {
            _runStartedEvent.Event -= Initialize;
            _gameOverEvent.Event -= OnEndRun;
            _returnToMainMenuEvent.Event -= OnEndRun;
            _bossDefeatedEvent.Event -= StoreMoney;
            _lootCollectedEvent.Event -= OnLootcollected;
            _activateDiamondRequestedEvent.Event -= ActivateDiamond;
            _desactivateDiamondRequestedEvent.Event -= DesactivateDiamond;
        }

        private void Initialize()
        {
            _moneyInRun.Value = 0;
            _moneyInStage.Value = 0;
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

        private void OnEndRun()
        {
            _moneyInStage.Value = 0;
        }

        private void StoreMoney(int reward)
        {
            _moneyInRun.Value += reward;
            _moneyInStage.Value += reward;
            _moneyChest.Value += _moneyInRun.Value;
            _moneyInStage.Value = 0;
        }
    }
}