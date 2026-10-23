using UnityEngine;

namespace Loot
{
    [CreateAssetMenu(fileName = "SSO_PowerUp", menuName = "SSO/PowerUp/SSO_PowerUp")]
    public class SSO_PowerUp : ScriptableObject
    {
        public string DisplayName;
        public Sprite Icon;
        public int DurationRows;
        public bool IsInstant => DurationRows <= 0;
        [Range(0f, 100)] public int SpawnProbability;
        
        [SerializeField] private RuntimeScriptableEvent _activatePowerUpEvent;
        [SerializeField] private RuntimeScriptableEvent _desactivatePowerUpEvent;
        
        public void Activate()
        {
            _activatePowerUpEvent.Dispatch();
        }

        public void Desactivate()
        {
            _desactivatePowerUpEvent.Dispatch();
        }
    }
}