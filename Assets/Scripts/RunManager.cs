using UnityEngine;

public class RunManager : MonoBehaviour{

    [SerializeField] private RSE_RunStarted _runStarted;

    private void Start()
    {
        InitializeRun();
    }

    private void InitializeRun()
    {
        Debug.Log("Initializing Run Manager");
        _runStarted.Dispatch();
    }
}