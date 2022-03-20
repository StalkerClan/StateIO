using System;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>, ISubcriber
{
    public event Action<int> OnShowingLevelID = delegate { };

    public int LevelID;

    [SerializeField]private Level currentLevel;
    public LevelGenerator levelGenerator;
    public LevelGenerator LevelGenerator { get => levelGenerator; set => levelGenerator = value; }

    private void Awake()
    {
        LevelID = JSONSaving.Instance.UserData.Level;    
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
        GameManager.Instance.SwitchState(GameState.MainMenu);
        levelGenerator.SetPlayerOwnedBuildings();
        currentLevel.SetLevelStatus(true, false, false);
        LevelID++;
        JSONSaving.Instance.UserData.Level = LevelID;
        JSONSaving.Instance.SaveData();
        PlayerPrefs.SetInt("LevelID", LevelID);
        SetLevel();      
        levelGenerator.LoadNextLevel();    
    }

    public void GameOver()
    {
        GameManager.Instance.SwitchState(GameState.GameOver);
    }
}
