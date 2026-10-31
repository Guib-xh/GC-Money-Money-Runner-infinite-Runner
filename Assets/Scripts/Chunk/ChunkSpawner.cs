using System;
using System.Collections.Generic;
using Boss;
using Countdown;
using Player;
using Speed;
using Stage;
using UnityEngine;
using UnityEngine.Serialization;

namespace Chunk
{
    public class ChunkSpawner : MonoBehaviour
    {
        
        //GAME CONFIG
        [SerializeField] private SSO_GameConfig _gameConfig;
        
        //DISPATCH
            
        //LISTEN
        [SerializeField] private RSE_RunStarted _runStartedEvent;
        [SerializeField] private RSE_PlayerDied _playerDiedEvent;
        
        //WRITE
            
        //READ
        [SerializeField] private RSO_Speed _speed;
        [SerializeField] private RSO_TotalDistanceRows _totalDistanceRows;
        [SerializeField] private RSO_BossActive _bossActive;
        [SerializeField] private RSO_CountdownActive _countdownActive;
        [SerializeField] private RSO_CurrentLane _currentLane;
        [SerializeField] private RSO_CurrentStagePhase _currentStagePhase;
        [SerializeField] private RSO_PooledChunks _pooledChunks;

        private List<PooledChunk> _activeChunk = new ();
        private System.Random _rng;
        private String _lastChunkId;
        private float _nextSpawnPositionZ;
        private int _seed;
        private bool _stopMovement;
        
        
        
        private void OnEnable()
        {
            _runStartedEvent.Event += Initialize;
            _playerDiedEvent.Event += OnPlayerDied;
        }

        private void OnDisable()
        {
            _runStartedEvent.Event -= Initialize;
            _playerDiedEvent.Event -= OnPlayerDied;
            
        } 
        
        private void Initialize()
        {
            foreach (var chunk in _activeChunk) RecycleChunk(chunk);
            _activeChunk.Clear();
            _lastChunkId = null;
            _nextSpawnPositionZ = 0f;
            _totalDistanceRows.Value = 0f;
            _stopMovement = false;

            initSeed();

            while (_nextSpawnPositionZ < _gameConfig.visibilityRows * _gameConfig.rowDepthR)
                SpawnNextChunk();
        }

        private void initSeed()
        {
            if (_seed == 0) _seed = Environment.TickCount;
            _rng = new System.Random(_seed);
        }
        
        private void Update()
        {
            if (IsMovementActive()) return;
            float deltaMeters = _speed.Value * Time.deltaTime;
            float deltaRows = deltaMeters / _gameConfig.rowDepthR;
            _totalDistanceRows.Value += deltaRows;
            _nextSpawnPositionZ -= deltaMeters;
            
            for (int i = _activeChunk.Count - 1; i >= 0; i--)
            {
                PooledChunk chunk = _activeChunk[i];
                chunk.Chunk.transform.position -= new Vector3(0, 0, deltaMeters);
                //Si le chunk arrive derrière le joueur on le supprime
                if (chunk.Chunk.transform.position.z < _gameConfig.despawnBehind * _gameConfig.rowDepthR) 
                {
                    RecycleChunk(chunk);
                    _activeChunk.RemoveAt(i);
                }
            }
            
            //On ajoute un nouveau chunk en fonction de la visibility paramétré
            if (_nextSpawnPositionZ < _gameConfig.visibilityRows * _gameConfig.rowDepthR) SpawnNextChunk();
            
            
            
        }
        
        private void SpawnNextChunk()
        {
            //Debug.Log("Spawning chunk");
            PooledChunk chunk = PickEligibleChunk();
            chunk.Chunk.transform.position = new Vector3(0f, 0f, _nextSpawnPositionZ);
            chunk.Chunk.SetActive(true);
            _nextSpawnPositionZ += chunk.Data.rows.Length * _gameConfig.rowDepthR;
            _activeChunk.Add(chunk);
        }
        

        //Récupère dans le pool de chunk un chunk qui est à la fois : inactif, du bon niveau de difficulté par rapport à l'avancé dans le stage et le dernier chunk qui a été spawn
        private PooledChunk PickEligibleChunk()
        {
            List<int> availableDifficulties = _currentStagePhase.Value.authorizedDifficulties;
            List<PooledChunk> eligible = new List<PooledChunk>();
            Debug.Log("_pooledChunks.Value : " + _pooledChunks.Value.Count);
            foreach (PooledChunk chunk in _pooledChunks.Value)
            {
                if (availableDifficulties.Contains(chunk.Data.difficulty)
                    && chunk.Data.id != _lastChunkId
                    && !chunk.Chunk.activeInHierarchy)
                {
                    Debug.Log("chunk.Items.count" + chunk.Items.Count);
                    eligible.Add(chunk);
                }
            }

            //Si on a trouvé aucun chunk sur la difficulté, on prend un chunk au hasard parmis tous
            if (eligible.Count == 0)
            {
                foreach (PooledChunk chunk in _pooledChunks.Value)
                {
                    if (chunk.Data.id != _lastChunkId 
                        && !chunk.Chunk.activeInHierarchy)
                    {
                        eligible.Add(chunk);
                    }
                }
            }

            PooledChunk eligibleChunk = eligible[_rng.Next(eligible.Count)];
            _lastChunkId = eligibleChunk.Data.id;
            return eligibleChunk;
        }

        private void RecycleChunk(PooledChunk oldPooledChunk)
        {
            Debug.Log("RecycleChunk oldPooledChunk.ChunkData.Name : " + oldPooledChunk.Data.id);
            Debug.Log("RecycleChunk oldPooledChunk.Items.Count : " + oldPooledChunk.Items.Count);
            ResetItems(oldPooledChunk);
            oldPooledChunk.Chunk.SetActive(false);
        }

        private void ResetItems(PooledChunk pooledChunk)
        {
            foreach (GameObject item in pooledChunk.Items)
            {
                item.SetActive(true);
            }
        }
        


        private bool IsMovementActive()
        {
            return _bossActive.Value || _countdownActive.Value || _stopMovement;
        }

        private void OnPlayerDied()
        {
            _stopMovement = true;
        }
    }
}
