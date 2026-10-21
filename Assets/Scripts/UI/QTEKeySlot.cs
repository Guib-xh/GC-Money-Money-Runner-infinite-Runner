using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    //Correspond à un slot UI de touche pour le QTE
    public class QTEKeySlot : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Color _idleColor = new (1f, 1f, 1f, 0.5f);
        [SerializeField] private Color _currentColor = Color.white;
        [SerializeField] private Color _successColor = Color.green;
        [SerializeField] private Color _failColor = Color.darkRed;
    
        public void Setup(Sprite sprite)
        {
            _image.sprite = sprite;
            _image.color = _idleColor;
        }
        
        public void SetCurrent() => _image.color = _currentColor;
        public void SetResult(bool isSuccess) => _image.color = isSuccess ? _successColor :  _failColor;
    }
    
}