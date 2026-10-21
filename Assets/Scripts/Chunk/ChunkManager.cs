using System;
using System.Collections.Generic;
using Boss;
using JetBrains.Annotations;
using Player;
using Speed;
using Stage;
using UnityEngine;
using UnityEngine.Serialization;

public class ChunkManager : MonoBehaviour
{
    [SerializeField] private RSE_RunStarted _runStarted;
    
    [SerializeField] private RSO_CurrentLine _currentLine;
    [SerializeField] private RSO_Speed _speed;
    [SerializeField] private RSO_InStageDistanceRows _inStageDistanceRows;
    [SerializeField] private RSO_TotalDistanceRows _totalDistanceRows;
    [SerializeField] private RSO_BossActive _bossActive;
    
    [SerializeField] private SSO_GameConfig _gameConfig;
    [SerializeField] private List<GameObject> _chunkPrefabs;
    
    private List<GameObject> _activeChunk = new ();
    private System.Random _rng;
    private String _lastChunkId;
    private float _nextSpawnPositionZ;
    private int CHUNK_LENGHT_ROW = 10;
    private int _seed;
    
    
    private void OnEnable() => _runStarted.Event += Initialize;
    private void OnDisable() => _runStarted.Event -= Initialize;

    private void Initialize()
    {
        foreach (var chunk in _activeChunk) Destroy(chunk);
        _activeChunk.Clear();
        _lastChunkId = null;
        _nextSpawnPositionZ = 0f;
        _totalDistanceRows.Value = 0f;
        if (_seed == 0) _seed = Environment.TickCount;
        _rng = new System.Random(_seed);

        while (_nextSpawnPositionZ < _gameConfig.visibilityRows * _gameConfig.rowDepthR)
            SpawnNextChunk();
    }

    private void Update()
    {
        if (_bossActive.Value) return;
        float deltaMeters = _speed.Value * Time.deltaTime;
        float deltaRows = deltaMeters / _gameConfig.rowDepthR;
        _inStageDistanceRows.Value += deltaRows;
        _totalDistanceRows.Value += deltaRows;
        _nextSpawnPositionZ -= deltaMeters;

        for (int i = _activeChunk.Count - 1; i >= 0; i--)
        {
            GameObject chunk = _activeChunk[i];
            chunk.transform.position -= new Vector3(0, 0, deltaMeters);
            //Si le chunk arrive derrière le joueur on le supprime
            if (chunk.transform.position.z < -20 * _gameConfig.rowDepthR) //TODO revoir le despawn
            {
                Destroy(chunk);
                _activeChunk.RemoveAt(i);
            }
        }
        if (_nextSpawnPositionZ < _gameConfig.visibilityRows * _gameConfig.rowDepthR) SpawnNextChunk();
    }

    private void SpawnNextChunk()
    {
        //Debug.Log("Spawning chunk");
        GameObject chunkPrefab = PickEligibleChunk();
        GameObject chunk = Instantiate(chunkPrefab, transform);
        chunk.transform.position = new Vector3(0f, 0f, _nextSpawnPositionZ);
        _nextSpawnPositionZ += CHUNK_LENGHT_ROW * _gameConfig.rowDepthR; //TODO rendre variable le nombre de rows d'un chunk
        _activeChunk.Add(chunk);
    }

    private GameObject PickEligibleChunk()
    {
        List<GameObject> eligible = new List<GameObject>();
        foreach (GameObject pref in _chunkPrefabs)
        {
            //TODO règle à ajouter pour les chunks
            eligible.Add(pref);
        }
        if (eligible.Count == 0) return null;
        return eligible[_rng.Next(eligible.Count)];
    }
}
