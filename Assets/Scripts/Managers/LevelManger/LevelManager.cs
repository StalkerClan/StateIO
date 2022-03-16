using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>, ISubcriber
{
    public event Action<int> OnShowingLevelID = delegate { };

    public int LevelID;

    [SerializeField]private Level currentLevel;
    private LevelGenerator levelGenerator;
    public LevelGenerator LevelGenerator { get => levelGenerator; set => levelGenerator = value; }

    private void Awake()
    {
        LevelID = PlayerPrefs.HasKey("LevelID") ? LevelID = PlayerPrefs.GetInt("LevelID") : LevelID = PlayerPrefs.GetInt("LevelID", 1);       
        levelGenerator = FindObjectOfType<LevelGenerator>();
        SubcribeEvent();
        LoadMap();
    }

    private void OnDisable()
    {
        UnsubcribeEvent();
    }
    public void SubcribeEvent()
    {
        levelGenerator.OnEnemiesOutOfBuildings += LevelCompleted;
        levelGenerator.OnPlayerOutOfBuildings += GameOver;
    }

    public void UnsubcribeEvent()
    {
        levelGenerator.OnEnemiesOutOfBuildings -= LevelCompleted;
        levelGenerator.OnPlayerOutOfBuildings -= GameOver;
    }

    public void LoadMap()
    {
        SetLevel();
        levelGenerator.LoadMap();
    }
    public void SetLevel()
    {
        currentLevel = levelGenerator.CurrentLevel = levelGenerator.ListLevel[LevelID - 1];
        currentLevel.SetLevelStatus(false, true, false);
    }

    public void LevelCompleted()
    {
        ObjectPooler.Instance.DeSpawnAllFighters();
        levelGenerator.SetPlayerOwnedBuildings();
        currentLevel.SetLevelStatus(true, false, false);
        LevelID++;
        PlayerPrefs.SetInt("LevelID", LevelID);
        SetLevel();
        GameManager.Instance.SwitchState(GameState.MainMenu);
        levelGenerator.LoadLevel();    
    }

    public void GameOver()
    {
        GameManager.Instance.SwitchState(GameState.GameOver);
    }
}
