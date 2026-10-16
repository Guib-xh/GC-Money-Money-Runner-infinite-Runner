using System;
using Stage;
using UnityEngine;

namespace Obstacle
{
    public class Obstacle : MonoBehaviour, IRunnerTrigger
    {
        [SerializeField] private RSE_ObstacleCollided _obstacleCollidedEvent;
        
        public void OnPlayerTrigger()
        {
            _obstacleCollidedEvent.Dispatch();
            Debug.Log("OBSTACLE TRIGGER ENTER");
        }
    }
}