using System;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>, ISubcriber
{
    public event Action<int> OnShowingLevelID = delegate { };
    public event Action<int> OnAddingCurrency = delegate { };

    private LevelGenerator levelGenerator;
    public Level currentLevel;
    public Player Player;
    
    public int LevelID;
    public int PlayerCurrency;

    public LevelGenerator LevelGenerator { get => levelGenerator; set => levelGenerator = value; }

    private void Awake()
    {
        LevelID = JSONSaving.Instance.UserData.Level;
        levelGenerator = FindObjectOfType<LevelGenerator>();
        SubcribeEvent();
        LoadMap();
        Player = levelGenerator.PlayerData as Player;
        PlayerCurrency = Player.OwnerStat.Currency;
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
        Player.OwnerStat.Currency = Formula.WinningGoldEarned(LevelID);
        levelGenerator.SetPlayerOwnedBuildings();
        currentLevel.SetLevelStatus(LevelStatus.Status.Completed);
        LevelID++;
        JSONSaving.Instance.UserData.Level = LevelID;
        SetLevel();      
        levelGenerator.LoadNextLevel();
        GameManager.Instance.SwitchState(GameState.MainMenu);
        OnShowingLevelID?.Invoke(LevelID);
        OnAddingCurrency?.Invoke(Player.OwnerStat.Currency);
    }

    public void GameOver()
    {
        Player.OwnerStat.Currency = Formula.LoseGoldEarned(LevelID);
        OnAddingCurrency?.Invoke(Player.OwnerStat.Currency);
        GameManager.Instance.SwitchState(GameState.GameOver);
    }
}
