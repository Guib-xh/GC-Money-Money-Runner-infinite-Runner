using System;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;

namespace Countdown
{
    public class CountdownDisplay : MonoBehaviour
    {
        [SerializeField] private RSE_CountdownFinished _countdownFinishedEvent;
        [SerializeField] private RSE_RequestCoundown _requestCoundownEvent;
        
        [SerializeField] private RSO_Countdown _countdown;
        
        [SerializeField] private TMP_Text _countdownText;
        [SerializeField] private GameObject _countdownCanvas;

        private void OnEnable()
        {
            _countdown.OnChanged += ShowCountdown;
        }
        
        private void OnDisable()
        {
            _countdown.OnChanged -= ShowCountdown;
        }

        private void ShowCountdown(int value)
        {
            if (value == 0)
            {
                _countdownCanvas.SetActive(false);
                return;
            }
            
            _countdownCanvas.SetActive(true);
            _countdownText.text = value.ToString();
            
            
        }
    }
}