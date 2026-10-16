using Player;
using UnityEngine;

namespace Loot
{
    public class Loot : MonoBehaviour, IRunnerTrigger
    {
        
        [SerializeField] private RSE_LootCollected _lootCollectedEvent;
        
        [SerializeField] private SSO_GameConfig _gameConfig;
        [SerializeField] private SSO_LootItem _lootItem;
        
        private bool _collected;
        
        

        public void OnPlayerTrigger()
        {
            Collect();
        }
        
        private void Collect()
        {
            if (_collected) return;
            _collected = true;
            _lootCollectedEvent.Dispatch(_lootItem.Value);
            gameObject.SetActive(false); //réinitilisé par le générateur
        }
    }
    
}