using Health;
using Loot;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LifeDisplay : MonoBehaviour
    {
        //GAME CONFIG
        
        //DISPATCH
        
        //LISTEN
        [SerializeField] private RSE_RunStarted _runStartedEvent;
        
        //WRITE
        
        //READ
        [SerializeField] private RSO_Hearts _hearts;
        [SerializeField] private RSO_MaxHearts _maxHearts;
        
        //UI
        [SerializeField] private Animator[] _heartsUI;
        

        private void OnEnable()
        {
            _hearts.OnChanged += RefreshHearts;
            _runStartedEvent.Event += Initialize;
        }
        
        private void OnDisable()
        {

            _hearts.OnChanged -= RefreshHearts;
            _runStartedEvent.Event -= Initialize;
            
        }

        private void Initialize()
        {
            for (int i = 0; i < _heartsUI.Length; i++)
            {
                bool active = i < _maxHearts.Value;
                _heartsUI[i].gameObject.SetActive(active);
                
                if (active) _heartsUI[i].SetBool("IsFull", true);
            } 
                
        }
        
        private void RefreshHearts(int health)
        {
            for (int i = 0; i < _heartsUI.Length; i++)
            {
                bool active = i < _maxHearts.Value;
                
                _heartsUI[i].gameObject.SetActive(active); 
                if (active) _heartsUI[i].SetBool("IsFull", i < health);
            }

        }

        private Color GetHeartsIconColor(int iconIndex)
        {
            return iconIndex < _hearts.Value ? Color.white : new Color(1, 1, 1, 0.25f);
        }
        }
}