using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HUDManager : MonoBehaviour
    {
        [SerializeField] private RSO_Hearts _hearts;
        [SerializeField] private RSO_Score _score;
        [SerializeField] private RSO_MoneyInRun _moneyInRun;
        [SerializeField] private RSO_TotalDistanceRows _totalDistanceRows;
        [SerializeField] private RSO_MaxHearts _maxHearts;
        
        [SerializeField] private SSO_GameConfig _gameConfig;
        
        
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _moneyText;
        [SerializeField] private TMP_Text _distanceText;
        
        [SerializeField] private Image[] _heartIcons;

        private void OnEnable()
        {
            _score.OnChanged += RefreshScore;
            _moneyInRun.OnChanged += RefreshMoney;
            _totalDistanceRows.OnChanged += RefreshDistance;
            _hearts.OnChanged += RefreshHearts;
        }
        
        private void OnDisable()
        {
            _score.OnChanged -= RefreshScore;
            _moneyInRun.OnChanged -= RefreshMoney;
            _totalDistanceRows.OnChanged -= RefreshDistance;
            _hearts.OnChanged -= RefreshHearts;
        }
        
        private void RefreshScore(int _) => _scoreText.text = _score.Value.ToString("N0");
        private void RefreshMoney(int _) => _moneyText.text = $"{_moneyInRun.Value} $";

        private void RefreshDistance(float _) => _distanceText.text =
            $"{Mathf.RoundToInt(_totalDistanceRows.Value * _gameConfig.rowDepthR)} m";

        private void RefreshHearts(int health)
        {
            for (int i = 0; i < _heartIcons.Length; i++)
                //affiche le nombre de coeur possible max (grisé ou non suivant le cas
                _heartIcons[i].enabled = i < _maxHearts.Value; 
            for (int i = 0; i < _heartIcons.Length; i++)
                if (_heartIcons[i].enabled) _heartIcons[i].color = GetHeartsIconColor(i);
        }

        private Color GetHeartsIconColor(int iconIndex)
        {
            return iconIndex < _hearts.Value ? Color.white : new Color(1, 1, 1, 0.25f);
        }
    }
}