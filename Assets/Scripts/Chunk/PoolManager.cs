using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Chunk
{
    public class PoolManager : MonoBehaviour
    {
        [SerializeField] private SSO_GameConfig _gameConfig;
        [SerializeField] private RSO_PooledChunks _pooledChunk;
        [SerializeField] private List<GameObject> _emptyChunkPrefabs;
        [SerializeField] private Dictionary<string, GameObject> _itemPrefabs;
        [SerializeField] private TextAsset[] _chunkFiles;

        private int _poolSize;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            //foreach (TextAsset chunkFile in _chunkFiles) _chunkDatas.Add(JsonUtility.FromJson<ChunkData>(chunkFile.text));
            _poolSize = _chunkFiles.Length * _emptyChunkPrefabs.Count;
            _pooledChunk.Value = new ();
            foreach (TextAsset chunkFile in _chunkFiles)
            {
                ChunkData chunkData = JsonUtility.FromJson<ChunkData>(chunkFile.text);
                foreach (GameObject emptyChunkPrefab in _emptyChunkPrefabs)
                {
                    GameObject chunk = Instantiate(emptyChunkPrefab, transform);
                    chunk.name = $"{emptyChunkPrefab.name}_{chunkData.id}";
                    PooledChunk pooledChunk = new PooledChunk { Chunk=chunk, Data = chunkData };
                    PopulateChunk(pooledChunk);
                    pooledChunk.Chunk.SetActive(false);
                    _pooledChunk.Value.Add(pooledChunk);
                }
            }
        }

        private void PopulateChunk(PooledChunk pooledChunk)
        {
            pooledChunk.Items = new List<GameObject>();
            RowData[] rows = pooledChunk.Data.rows;
            for (int row = 0; row < rows.Length; row++)
            {
                for (int lane = 0; lane < rows[row].lanes.Length; lane++)
                {
                    string cell = rows[row].lanes[lane];
                    if (cell.Equals("")) continue;
                    GameObject itemPrefab = ResolveItemPrefab(cell);
                    if (itemPrefab == null)
                    {
                        Debug.LogWarning($"Item {cell} not found");
                        continue;
                    }
                    
                    GameObject item = Instantiate(itemPrefab, pooledChunk.Chunk.transform.Find("Gameplay"));
                    float x = (lane - 2) * _gameConfig.laneWidthLv;
                    float z = (rows.Length - row) * _gameConfig.rowDepthR;
                    item.transform.localPosition = new Vector3(x, 0, z);
                    item.SetActive(true);
                    pooledChunk.Items.Add(item);
                }
            }
            Debug.Log("pooled chunk contains " + pooledChunk.Items.Count + " items");
        }

        //TODO Revoir cette methode
        private GameObject ResolveItemPrefab(string cell)
        {
            if (cell.Equals("ALEA_SAUT_GLISSE"))
            {
                List<GameObject> candidates = new List<GameObject>();
                candidates.Add(_itemPrefabs.GetValueOrDefault("OBS_BAS"));
                candidates.Add(_itemPrefabs.GetValueOrDefault("OBS_HAUT"));
                return candidates[Random.Range(0, candidates.Count)];
            }
            if (cell.Equals("ALEA_PLEIN"))
            {
                List<GameObject> candidates = new List<GameObject>();
                candidates.Add(_itemPrefabs.GetValueOrDefault("OBS_PLEIN"));
                candidates.Add(_itemPrefabs.GetValueOrDefault("OBS_PLEIN_2R"));
                candidates.Add(_itemPrefabs.GetValueOrDefault("OBS_PLEIN_3R"));
                return candidates[Random.Range(0, candidates.Count)];     
            }

            return _itemPrefabs.GetValueOrDefault(cell);
        }
    }
    

}