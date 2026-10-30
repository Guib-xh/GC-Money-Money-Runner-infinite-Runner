using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Speed
{
    public class CoffeeScreenEffect : MonoBehaviour
    {
        [SerializeField] RSE_ActivateCoffeeRequested _activateCoffeeRequestedEvent;
        [SerializeField] RSE_DesactivateCoffeeRequested _desactivateCoffeeRequestedEvent;
        [SerializeField] Camera _camera;
        [SerializeField] Volume _volume;

        private Coroutine _coffeeEffectCoroutine;
        private float _t;
        private float _transitionDuration = 0.4f;

        private void OnEnable()
        {
            _activateCoffeeRequestedEvent.Event += ActivateCoffeeScreenEffect;
            _desactivateCoffeeRequestedEvent.Event += DesactivateCoffeeScreenEffect;
        }        
        
        private void OnDisable()
        {
            _activateCoffeeRequestedEvent.Event -= ActivateCoffeeScreenEffect;
            _desactivateCoffeeRequestedEvent.Event -= DesactivateCoffeeScreenEffect;
        }

        private void ApplyCoffeeScreenEffect(float target)
        {
            //Debug.Log("ApplyCoffeeScreenEffect");
            if (_coffeeEffectCoroutine != null) StopCoroutine(_coffeeEffectCoroutine);
            _coffeeEffectCoroutine = StartCoroutine(CoffeEffectRoutine(target));
        }

        private IEnumerator CoffeEffectRoutine(float target)
        {
            float speed = 1 / _transitionDuration;
            while (!Mathf.Approximately(_t, target))
            {
                _t = Mathf.MoveTowards(_t, target, speed * Time.deltaTime);
                Apply();
                yield return null;
            }
            _t = target;
            Apply();
            _coffeeEffectCoroutine = null;
        }

        private void Apply()
        {
            _camera.fieldOfView = Mathf.Lerp(60f, 50f, _t);
            _volume.weight = _t;
        }
        
        private void ActivateCoffeeScreenEffect()
        {
            ApplyCoffeeScreenEffect(1f);
        }

        private void DesactivateCoffeeScreenEffect()
        {
            ApplyCoffeeScreenEffect(0f);
        }
    }
}