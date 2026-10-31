using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chunk
{
    [Serializable]
    public class PooledChunk
    {
        public GameObject Chunk;
        public List<GameObject> Items;
        public ChunkData Data;
    }
}