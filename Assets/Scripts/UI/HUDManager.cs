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
        [SerializeField] private RSO_MaxHearts _maxHearts;
        
        
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private Image[] _heartIcons;

        private void OnEnable()
        {
            _score.OnChanged += RefreshScore;
            _hearts.OnChanged += RefreshHearts;
        }
        
        private void OnDisable()
        {
            _score.OnChanged -= RefreshScore;
            _hearts.OnChanged -= RefreshHearts;
        }
        
        private void RefreshScore(int score) => _scoreText.text = _score.Value.ToString("N0");

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