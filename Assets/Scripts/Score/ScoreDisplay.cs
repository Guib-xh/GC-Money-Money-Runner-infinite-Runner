using TMPro;
using UnityEngine;

namespace Score
{
    public class ScoreDisplay : MonoBehaviour
    {
        //GAME CONFIG
        
        //DISPATCH
        
        //LISTEN
        
        //WRITE
        
        //READ
        [SerializeField] private RSO_Score _score;
        
        //UI
        [SerializeField] private TMP_Text _scoreText;
        

        private void OnEnable()
        {
            _score.OnChanged += RefreshScore;
        }
        
        private void OnDisable()
        {
            _score.OnChanged -= RefreshScore;
        }
        
        private void RefreshScore(int _) => _scoreText.text = _score.Value.ToString();
    }
}