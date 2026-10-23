using Player;
using UnityEngine;

namespace Loot
{
    public class Loot : MonoBehaviour, IRunnerTrigger
    {
        //GAME CONFIG
        [SerializeField] private SSO_GameConfig _gameConfig;
        [SerializeField] private SSO_LootItem _lootItem;
        
        //DISPATCH
        [SerializeField] private RSE_LootCollected _lootCollectedEvent;
        
        //LISTEN
        
        //WRITE
        
        //READ
        

        
        private bool _collected;
        
        

        public void OnPlayerTrigger()
        {
            Collect();
        }
        
        private void Collect()
        {
            if (_collected) return;
            _collected = true;
            _lootCollectedEvent.Dispatch(_lootItem);
            gameObject.SetActive(false); //réinitilisé par le générateur
        }
    }
    
}