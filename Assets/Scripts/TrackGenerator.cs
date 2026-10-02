using System;
using System.Collections.Generic;
using Player;
using Speed;
using Stage;
using UnityEngine;

public class TrackGenerator : MonoBehaviour
{
    [SerializeField] private RSE_RunStarted _runStarted;
    [SerializeField] private RSO_CurrentLine _currentLine;
    [SerializeField] private RSO_Speed _speed;
    [SerializeField] private RSO_DistanceRowsInStage _distanceRowsInStage;
    [SerializeField] private RSO_TotalDistanceRows _totalDistanceRows;
    [SerializeField] private SSO_GameConfig _gameConfig;
    [SerializeField] private List<GameObject> _chunkPrefabs;
    
    private List<GameObject> _activeChunk = new ();
    private System.Random _rng;
    private String _lastChunkId;
    private float _frontEdgeWorldZ;
    public int Seed { get; set; }
    
    
    private void OnEnable() => _runStarted.Event += Initialize;
    private void OnDisable() => _runStarted.Event -= Initialize;

    private void Initialize()
    {
        foreach (var chunk in _activeChunk) Destroy(chunk);
        _activeChunk.Clear();
        _lastChunkId = null;
        _frontEdgeWorldZ = 0f;
        if (Seed == 0) Seed = Environment.TickCount;
        _rng = new System.Random(Seed);

        while (_frontEdgeWorldZ < _gameConfig.VisibilityRows * _gameConfig.RowDepthR)
            SpawnNextChunk();
    }

    private void Update()
    {
        float deltaMeters = _speed.Value * Time.deltaTime;
        float deltaRows = deltaMeters / _gameConfig.RowDepthR;
        _distanceRowsInStage.Value += deltaRows;
        _totalDistanceRows.Value += deltaRows;
        _frontEdgeWorldZ -= deltaMeters;

        for (int i = _activeChunk.Count - 1; i >= 0; i--)
        {
            GameObject chunk = _activeChunk[i];
            chunk.transform.position -= new Vector3(0, 0, deltaMeters);
            //Si le chunk arrive derrière le joueur on le supprime
            if (chunk.transform.position.z < -20 * _gameConfig.RowDepthR) //TODO revoir le despawn
            {
                Destroy(chunk);
                _activeChunk.RemoveAt(i);
            }
        }
        if (_frontEdgeWorldZ < _gameConfig.VisibilityRows * _gameConfig.RowDepthR) SpawnNextChunk();
    }

    private void SpawnNextChunk()
    {
        Debug.Log("Spawning chunk");
        GameObject chunkPrefab = PickEligibleChunk();
        GameObject chunk = Instantiate(chunkPrefab, transform);
        chunk.transform.position = new Vector3(0f, 0f, _frontEdgeWorldZ);
        _frontEdgeWorldZ += 10 * _gameConfig.RowDepthR; //TODO rendre variable le nombre de rows d'un chunk
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
