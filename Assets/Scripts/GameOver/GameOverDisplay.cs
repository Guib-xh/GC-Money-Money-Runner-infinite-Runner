using Player;
using TMPro;
using UnityEngine;

namespace GameOver
{
    public class GameOverDisplay : MonoBehaviour
    {
        //GAME CONFIG
        [SerializeField] private SSO_GameConfig _gameConfig;
        
        //DISPATCH
        [SerializeField] private RSE_GameOver _gameOverEvent;
        [SerializeField] private RSE_ReturnToMainMenu _returnToMainMenuEvent;
        
        //LISTEN
        
        //WRITE

        //READ
        [SerializeField] private RSO_Score _score;
        [SerializeField] private RSO_MoneyInRun _moneyInRun;
        [SerializeField] private RSO_MoneyChest _moneyChest;
        [SerializeField] private RSO_TotalDistanceRows _totalDistanceRows;
        
        //UI        
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _moneyText;
        [SerializeField] private TMP_Text _moneyChestText;
        [SerializeField] private TMP_Text _distanceText;
        
        private void OnEnable()
        {
            ShowGameOver();
        }
        

        private void ShowGameOver()
        {
            _scoreText.text = _score.Value.ToString();
            _moneyText.text = $"{_moneyInRun.Value} $";
            _moneyChestText.text = $"{_moneyChest.Value} $";
            _distanceText.text = $"{Mathf.RoundToInt(_totalDistanceRows.Value * _gameConfig.rowDepthR)} m";
        }

        public void OnReturnMainMenuClicked()
        {
            _returnToMainMenuEvent.Dispatch();
        }
    }
}