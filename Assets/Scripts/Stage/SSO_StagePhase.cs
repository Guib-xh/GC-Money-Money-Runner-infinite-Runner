using System;
using System.Collections.Generic;
using UnityEngine;

public enum PhaseName
{
    Depart,      
    Echauffement,
    Montee,
    Tension,
    PicFinal,
    SasPreBoss
}

[Serializable]
public struct Phase
{
    public PhaseName phaseName;
    public float startThreshold;
    public List<int> authorizedDifficulties;
}

[CreateAssetMenu(fileName = "SSO_StagePhase", menuName="SSO/Stage/SSO_StagePhase")]
public class SSO_StagePhase: ScriptableObject
{
    public Phase[]  Phases = {
        new Phase{phaseName=PhaseName.Depart,startThreshold = 0f, authorizedDifficulties = new List<int>{1}},
        new Phase{phaseName=PhaseName.Echauffement,startThreshold = 0.05f, authorizedDifficulties = new List<int>{1,2}},
        new Phase{phaseName=PhaseName.Montee,startThreshold = 0.25f, authorizedDifficulties = new List<int>{2,3}},
        new Phase{phaseName=PhaseName.Tension,startThreshold = 0.5f, authorizedDifficulties = new List<int>{3,4}},
        new Phase{phaseName=PhaseName.PicFinal,startThreshold = 0.76f, authorizedDifficulties = new List<int>{4,5}},
        new Phase{phaseName=PhaseName.SasPreBoss,startThreshold = 0.94f, authorizedDifficulties = new List<int>{1}},
    };
        
}