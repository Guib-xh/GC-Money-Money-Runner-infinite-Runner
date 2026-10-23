using Loot;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace PowerUp
{
    public class PowerUpDisplay : MonoBehaviour
    {
        //GAME CONFIG
        
        //DISPATCH
        
        //LISTEN
        
        //WRITE
        
        //READ
        [SerializeField] private RSO_HeldPowerUp _heldPowerUp;
        [SerializeField] private RSO_ActivePowerUp _activePowerUp;
        
        
        //UI
        [SerializeField] private Image _heldPowerUpIcon;
        [SerializeField] private GameObject _heldPowerUpPanel;
        
        [SerializeField] private Image _activePowerUpIcon;
        [SerializeField] private GameObject _activePowerUpPanel;
        

        private void OnEnable()
        {
            _heldPowerUp.OnChanged += RefreshHeldPowerUp;
            _activePowerUp.OnChanged += RefreshActivePowerUp;
        }
        
        private void OnDisable()
        {
            _heldPowerUp.OnChanged -= RefreshHeldPowerUp;
            _activePowerUp.OnChanged -= RefreshActivePowerUp;
        }
        
        private void RefreshHeldPowerUp(SSO_PowerUp powerUp)
        {
            bool hasPowerUp = _heldPowerUp.Value is not null;
            if (hasPowerUp)
            {
                _heldPowerUpIcon.sprite = powerUp.Icon;
            }
            _heldPowerUpPanel.SetActive(hasPowerUp);
        }

        private void RefreshActivePowerUp(SSO_PowerUp powerUp)
        {
            bool isPowerUpInProgress = _activePowerUp.Value is not null;
            if (isPowerUpInProgress)
            {
                _activePowerUpIcon.sprite  = powerUp.Icon;
            }
            _activePowerUpPanel.SetActive(isPowerUpInProgress);
            
        }
    }
}