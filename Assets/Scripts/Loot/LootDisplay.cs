using System;
using Player;
using UnityEngine;

namespace Loot
{
    public class LootDisplay : MonoBehaviour
    {
        
        //CONFIG
        [SerializeField] private SSO_LootItem _lootItem;
       
        //DISPATCH
        
        //LISTEN
        [SerializeField] private RSE_ActivateDiamondRequested _activateDiamondRequestedEvent;
        [SerializeField] private RSE_DesactivateDiamondRequested _desactivateCoffeeRequestedEvent;
        
        
        //WRITE
        
        //READ
        [SerializeField] private RSO_DiamondActive _diamondActive;
        
        
        [SerializeField] private ParticleSystem _glowParticles;
        

        private void OnEnable()
        {
            _activateDiamondRequestedEvent.Event += glowOn;
            _desactivateCoffeeRequestedEvent.Event += glowOff;
            
            if (_diamondActive.Value) glowOn();
        }

        private void OnDisable()
        {
            _activateDiamondRequestedEvent.Event -= glowOn;
            _desactivateCoffeeRequestedEvent.Event -= glowOff;
        }
        
        private void glowOn()
        {
            if (_lootItem.IsLootBox) return;
            _glowParticles.Play();
        }

        private void glowOff()
        {
            if (_lootItem.IsLootBox) return;
            _glowParticles.Stop();
        }
    }
    
}