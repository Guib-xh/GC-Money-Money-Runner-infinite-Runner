using System;
using System.IO;
using System.Linq;
using Player;
using Stage;
using UnityEngine;
using UnityEngine.Serialization;

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
        [SerializeField] private RSE_ReturnToMainMenu _returnToMainMenuEvent;
        
        //WRITE
        
        //READ
        [SerializeField] private RSO_Score _score;
        [SerializeField] private RSO_CurrentStage _currentStage;
        [SerializeField] private RSO_MoneyChest _moneyChest;
        [SerializeField] private RSO_ActiveProfile _activeProfile;
        
        private void Awake() => Load();

        private void OnEnable()
        {
            _gameOverEvent.Event += Save;
            _returnToMainMenuEvent.Event += Save;
        }
        
        private void OnDisable()
        {
            _gameOverEvent.Event -= Save;
            _returnToMainMenuEvent.Event -= Save;
        }
        

        private SaveFile _saveFile;
        private static string _filePath;
        
        private void Load()
        {
            _filePath = Application.persistentDataPath + "moneymoneyrunner_save.json";
            if (File.Exists(_filePath))
            {
                _saveFile = JsonUtility.FromJson<SaveFile>(File.ReadAllText(_filePath));
            } else
            {
                _saveFile = new SaveFile();
                _saveFile.Profils = new ProfileData { Pseudo = "Joueur" };
                
                File.WriteAllText(_filePath, JsonUtility.ToJson(_saveFile));
            }
            _moneyChest.Value = _saveFile.Profils.MoneyChest;
            _activeProfile.Value = _saveFile.Profils;
        }

        private void Save()
        {
            ProfileData profile = _saveFile.Profils;
            profile.MoneyChest = _moneyChest.Value;
            profile.BestScore = Mathf.Max(profile.BestScore, _score.Value);
            profile.MaxStage = Mathf.Max(profile.MaxStage, _currentStage.Value);
            profile.CatchRubberNumber += Mathf.Max(0, _currentStage.Value - 1);
            
            File.WriteAllText(_filePath, JsonUtility.ToJson(_saveFile));
        }
    }
}