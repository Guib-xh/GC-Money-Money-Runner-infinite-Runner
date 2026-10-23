using Player;
using TMPro;
using UnityEngine;

namespace Money
{
    public class MoneyDisplay : MonoBehaviour
    {
        //GAME CONFIG
        
        //DISPATCH
        
        //LISTEN
        
        //WRITE
        
        //READ
        [SerializeField] private RSO_MoneyInRun _moneyInRun;
        
        //UI
        [SerializeField] private TMP_Text _moneyText;
        

        private void OnEnable()
        {
            _moneyInRun.OnChanged += RefreshMoney;
        }
        
        private void OnDisable()
        {
            _moneyInRun.OnChanged -= RefreshMoney;
        }
        
        private void RefreshMoney(int _) => _moneyText.text = $"{_moneyInRun.Value} $";

    }
}