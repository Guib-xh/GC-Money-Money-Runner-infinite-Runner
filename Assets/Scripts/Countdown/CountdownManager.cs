
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Countdown
{
    public class CountdownManager : MonoBehaviour
    {
        [SerializeField] private RSE_CountdownFinished _countdownFinishedEvent;
        [SerializeField] private RSE_RequestCoundown _requestCoundownEvent;
        
        [SerializeField] private RSO_Countdown _countdown;
        [SerializeField] private RSO_CountdownActive _countdownActive;
        
        private Coroutine _countdownRoutine;
        
        

        private void OnEnable()
        {
            _requestCoundownEvent.Event += StartCountdown;
        }
        
        private void OnDisable()
        {
            _requestCoundownEvent.Event -= StartCountdown;
        }

        private void StartCountdown(int duration)
        {
            if (_countdownRoutine != null) StopCoroutine(_countdownRoutine);
            _countdownRoutine = StartCoroutine(RunCountdown(duration));
        }

        private IEnumerator RunCountdown(int duration)
        {
            _countdownActive.Value = true;
            for (int i = duration; i > 0; i--)
            {
                _countdown.Value = i;
                float t = 0f;
                while (t < 1f)
                {
                    t += Time.deltaTime;
                    yield return null;
                }
            }
            _countdown.Value = 0;
            _countdownActive.Value = false;
            _countdownRoutine = null;
            _countdownFinishedEvent.Dispatch();
            
        }
    }
}