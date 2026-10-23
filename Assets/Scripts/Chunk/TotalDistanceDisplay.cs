using TMPro;
using UnityEngine;

namespace Chunk
{
    public class TotalDistanceDisplay : MonoBehaviour
    {
        //GAME CONFIG
        [SerializeField] private SSO_GameConfig _gameConfig;
        
        //DISPATCH
        
        //LISTEN
        
        //WRITE
        
        //READ
        [SerializeField] private RSO_TotalDistanceRows _totalDistanceRows;

        //UI
        [SerializeField] private TMP_Text _distanceText;


        private void OnEnable()
        {
            _totalDistanceRows.OnChanged += RefreshDistance;
        }
        
        private void OnDisable()
        {
            _totalDistanceRows.OnChanged -= RefreshDistance;
        }
        
        private void RefreshDistance(float _) => _distanceText.text =
            $"{Mathf.RoundToInt(_totalDistanceRows.Value * _gameConfig.rowDepthR)} m";
        
    }
}