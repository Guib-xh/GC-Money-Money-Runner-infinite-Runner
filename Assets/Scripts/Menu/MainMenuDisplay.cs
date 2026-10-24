using System;
using Countdown;
using JetBrains.Annotations;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Menu
{
    public class MainMenuDisplay : MonoBehaviour
    {
        //DISPATCH
        
        //LISTEN
        
        //WRITE
        
        //READ
        [SerializeField] private RSO_MoneyChest _moneyChest;
        [SerializeField] private RSO_ActiveProfile _activeProfile;
        

        //UI
        [SerializeField] private TMP_Text _moneyChestValue;
        [SerializeField] private TMP_Text _bestScoreValue;
        [SerializeField] private TMP_Text _rubberValue;
        

        private void OnEnable()
        {
            RefreshSaveData();
        }
        

        
        private void RefreshSaveData()
        {
            _moneyChestValue.text = $"{_moneyChest.Value} $";
            _bestScoreValue.text = _activeProfile.Value.BestScore.ToString();
            _rubberValue.text = _activeProfile.Value.CatchRubberNumber.ToString();
        }
    }
}