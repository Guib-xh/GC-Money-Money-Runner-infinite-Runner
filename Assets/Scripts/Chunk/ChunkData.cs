using System;
using JetBrains.Annotations;

namespace Chunk
{
    [Serializable]
    public class RowData
    {
        public string[] lanes;
    }
    
    [Serializable]
    public class ChunkData
    {
        public string id;
        public int difficulty;
        public RowData[] rows;
    }
}