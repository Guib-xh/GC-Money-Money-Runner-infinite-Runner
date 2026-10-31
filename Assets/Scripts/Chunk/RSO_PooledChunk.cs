using System.Collections.Generic;
using UnityEngine;

namespace Chunk
{
    [CreateAssetMenu(fileName = "RSO_PooledChunks", menuName = "RSO/Chunk/RSO_PooledChunks")]
    public class RSO_PooledChunks : RuntimeScriptableObject<List<PooledChunk>>
    {
    }
}