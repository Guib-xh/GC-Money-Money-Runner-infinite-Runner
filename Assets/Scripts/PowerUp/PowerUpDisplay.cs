using Loot;
using Player;
using UnityEngine;
using UnityEngine.Serialization;
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
        [SerializeField] private RSO_ActivePowerUpTimeRemaining _activePowerUpTimeRemaining;
        
        
        //UI
        [SerializeField] private Image _heldPowerUpIcon;
        [SerializeField] private GameObject _heldPowerUpPanel;
        
        [SerializeField] private Image _activePowerUpIcon;
        [SerializeField] private Image _nextHeldPowerUpIcon;
        [SerializeField] private GameObject _activePowerUpPanel;
        [SerializeField] private Image _activePowerUpProgressBarFill;
        

        private void OnEnable()
        {
            _heldPowerUp.OnChanged += RefreshHeldPowerUp;
            _activePowerUp.OnChanged += RefreshActivePowerUp;
            _activePowerUpTimeRemaining.OnChanged += UpdateProgressBar;
        }
        
        private void OnDisable()
        {
            _heldPowerUp.OnChanged -= RefreshHeldPowerUp;
            _activePowerUp.OnChanged -= RefreshActivePowerUp;
            _activePowerUpTimeRemaining.OnChanged -= UpdateProgressBar;
        }
        
        private void RefreshHeldPowerUp(SSO_PowerUp powerUp)
        {
            bool hasHeldPowerUp = _heldPowerUp.Value is not null;
            if (hasHeldPowerUp)
            {
                _heldPowerUpIcon.sprite = powerUp.Icon;
                _nextHeldPowerUpIcon.sprite = powerUp.Icon;
            }
            _heldPowerUpPanel.SetActive(hasHeldPowerUp);
            _nextHeldPowerUpIcon.gameObject.SetActive(hasHeldPowerUp);
        }

        private void RefreshActivePowerUp(SSO_PowerUp powerUp)
        {
            bool isPowerUpInProgress = _activePowerUp.Value is not null;
            if (isPowerUpInProgress)
            {
                _activePowerUpIcon.sprite  = powerUp.Icon;
            }
            _activePowerUpPanel.SetActive(isPowerUpInProgress);
            bool hasNextHeldPowerUp = _heldPowerUp.Value is not null;
            if (hasNextHeldPowerUp){
                _nextHeldPowerUpIcon.sprite = _heldPowerUp.Value.Icon;
            }
            _nextHeldPowerUpIcon.gameObject.SetActive(hasNextHeldPowerUp);
            
            
        }
        
        private void UpdateProgressBar(float value)
        {
            _activePowerUpProgressBarFill.fillAmount = Mathf.Clamp01(value);
        }
    }
}