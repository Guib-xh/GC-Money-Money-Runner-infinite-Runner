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
        
        //WRITE
        
        //READ
        [SerializeField] private RSO_Hearts _hearts;
        [SerializeField] private RSO_MaxHearts _maxHearts;
        
        //UI
        [SerializeField] private Image[] _heartIcons;

        private void OnEnable()
        {
            _hearts.OnChanged += RefreshHearts;
        }
        
        private void OnDisable()
        {

            _hearts.OnChanged -= RefreshHearts;
        }
        
        private void RefreshHearts(int health)
        {
            for (int i = 0; i < _heartIcons.Length; i++)
                //affiche le nombre de coeur possible max (grisé ou non suivant le cas
                _heartIcons[i].enabled = i < _maxHearts.Value; 
            for (int i = 0; i < _heartIcons.Length; i++)
                if (_heartIcons[i].enabled) _heartIcons[i].color = GetHeartsIconColor(i);
        }

        private Color GetHeartsIconColor(int iconIndex)
        {
            return iconIndex < _hearts.Value ? Color.white : new Color(1, 1, 1, 0.25f);
        }
        }
}