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
        currentLevel.SetLevelStatus(LevelStatus.Status.IsPlaying);
    }

    public void LevelCompleted()
    {
        levelGenerator.SetPlayerOwnedBuildings();
        currentLevel.SetLevelStatus(LevelStatus.Status.Completed);
        LevelID++;
        JSONSaving.Instance.UserData.Level = LevelID;
        PlayerPrefs.SetInt("LevelID", LevelID);
        SetLevel();      
        levelGenerator.LoadNextLevel();
        GameManager.Instance.SwitchState(GameState.MainMenu);
    }

    public void GameOver()
    {
        GameManager.Instance.SwitchState(GameState.GameOver);
    }
}
