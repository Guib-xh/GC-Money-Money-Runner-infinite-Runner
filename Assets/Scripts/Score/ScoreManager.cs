using Player;
using UnityEngine;

namespace Score
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private RSE_LootCollected _lootCollectedEvent;
        [SerializeField] private RSE_RunStarted _runStartedEvent;
        
        [SerializeField] private RSO_Score _score;
        [SerializeField] private RSO_TotalDistanceRows _totalDistanceRows;
        [SerializeField] private RSO_MoneyInRun _moneyInRun;
        
        [SerializeField] private SSO_GameConfig _gameConfig;
        

        
        private void OnEnable()
        {
            _runStartedEvent.Event += Initialize;
            _lootCollectedEvent.Event += OnLootcollected;
            _totalDistanceRows.OnChanged += OnDistanceChanged;
        }
        
        private void OnDisable()
        {
            _runStartedEvent.Event -= Initialize;
            _lootCollectedEvent.Event -= OnLootcollected;
            _totalDistanceRows.OnChanged -= OnDistanceChanged;
        }

        private void Initialize()
        {
            _score.Value = 0;
        }

        private void OnLootcollected(int _) => UpdateScore();
        private void OnDistanceChanged(float _) => UpdateScore();

        private void UpdateScore()
        {
            int numberOfBossDefeated = 0; //TODO
            _score.Value = _moneyInRun.Value * _gameConfig.scorePerMoney //Score de l'argent récolté
                            + Mathf.RoundToInt(_totalDistanceRows.Value * _gameConfig.scorePerRow) //Score pour la distance parcourue
                            + numberOfBossDefeated * _gameConfig.scorePerBoss; //Score pour le nombre de boss vaincu
        }
        
    }
}