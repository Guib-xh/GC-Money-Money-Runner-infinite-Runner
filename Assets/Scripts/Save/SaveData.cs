using System;
using System.Collections.Generic;

namespace Save
{
    [Serializable]
    public class LeaderBoardData
    {
        public string Pseudo;

        public int Score;

        public int MaxStage;
        
        public float DistanceMetres;
        
        public float DureeSecondes;
        
        public int ArgentRecolte;

    }

    [Serializable]
    public class SaveFile
    {
        public List<LeaderBoardData> LeaderBoardDataList = new();
    }
}