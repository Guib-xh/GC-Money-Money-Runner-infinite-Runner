using System;
using Stage;
using UnityEngine;

namespace Obstacle
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private RSE_ObstacleCollided _obstacleCollidedEvent;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            _obstacleCollidedEvent.Dispatch();
            Debug.Log("OBSTACLE TRIGGER ENTER");
        }
    }
}