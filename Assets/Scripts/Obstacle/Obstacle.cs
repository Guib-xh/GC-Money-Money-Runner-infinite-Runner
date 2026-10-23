using System;
using Stage;
using UnityEngine;

namespace Obstacle
{
    public class Obstacle : MonoBehaviour, IRunnerTrigger
    {
        //DISPATCH
        [SerializeField] private RSE_ObstacleCollided _obstacleCollidedEvent;
        
        //LISTEN
        
        //WRITE
        
        //READ
        
        private bool _hasBeenCollided = false; //Utile pour le obstacle avec plusieurs collider
        
        public void OnPlayerTrigger()
        {
            if (!_hasBeenCollided)
            {
                _hasBeenCollided = true;
                _obstacleCollidedEvent.Dispatch();
            }
        }
    }
}