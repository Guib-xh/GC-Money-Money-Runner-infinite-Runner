using System;
using System.Collections.Generic;

namespace Save
{
    [Serializable]
    public class ProfileData
    {
        public string Pseudo;
        public int MoneyChest;
        public int BestScore;
        public int CatchRubberNumber;
        public int MaxStage;
    }


    [Serializable]
    public class SaveFile
    {
        public ProfileData Profils = new();
    }
}