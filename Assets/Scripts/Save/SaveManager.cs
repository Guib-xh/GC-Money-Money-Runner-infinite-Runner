using System;
using System.IO;
using System.Linq;
using Player;
using Stage;
using UnityEngine;

namespace Save
{
    public class SaveManager : MonoBehaviour
    {
        //GAME CONFIG
        [SerializeField] private SSO_GameConfig _gameConfig;
        
        //DISPATCH
        
        //LISTEN
        [SerializeField] private RSE_RunStarted _runStartedEvent;
        [SerializeField] private RSE_GameOver _gameOverEvent;
        
        //WRITE
        
        //READ
        [SerializeField] private RSO_Score _score;
        [SerializeField] private RSO_CurrentStage _currentStage;
        [SerializeField] private RSO_TotalDistanceRows _totalDistanceRows;
        
        private float _runStartTime;
        
        private void Awake() => Load();

        private void OnEnable()
        {
            _runStartedEvent.Event += Initialize;
            _gameOverEvent.Event += Save;
        }
        
        private void OnDisable()
        {
            _runStartedEvent.Event -= Initialize;
            _gameOverEvent.Event -= Save;
        }

        private void Initialize()
        {
            _runStartTime = Time.time;
        } 
        
        

        private SaveFile _saveFile;
        private static string _filePath = Application.persistentDataPath + "moneymoneyrunner_save.json";
        
        private void Load()
        {
            if (File.Exists(_filePath))
            {
                _saveFile = JsonUtility.FromJson<SaveFile>(File.ReadAllText(_filePath));
            } else
            {
                _saveFile = new SaveFile();
                File.WriteAllText(_filePath, JsonUtility.ToJson(_saveFile));
            }
        }

        private void Save()
        {
            float distanceMetres = _totalDistanceRows.Value * _gameConfig.rowDepthR;
            float durationSeconds = Time.time - _runStartTime;
            _saveFile.LeaderBoardDataList.Add(new LeaderBoardData
            {
                Pseudo = "toto", //TODO
                Score = _score.Value,
                MaxStage =  _currentStage.Value,
                DistanceMetres = distanceMetres,
                DureeSecondes = durationSeconds,
                ArgentRecolte = 0 //TODO
            });

            // Tri : score décroissant, puis distance, puis argent (Economic Design 11) ; top 10.
            _saveFile.LeaderBoardDataList = _saveFile.LeaderBoardDataList
                .OrderByDescending(e => e.Score)
                .ThenByDescending(e => e.DistanceMetres)
                .ThenByDescending(e => e.ArgentRecolte)
                .Take(10)
                .ToList();
            
            File.WriteAllText(_filePath, JsonUtility.ToJson(_saveFile));
        }
    }
}